using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Identity.Enums;
using MyShop.Identity.Constants;

namespace MyShop.Identity.Services;

/// <summary>
/// Service for managing authorization and permission checking
/// </summary>
public interface IAuthorizationService
{
    Task<bool> HasPermissionAsync(string userId, string permissionName);
    Task<bool> HasPermissionAsync(string userId, Resource resource, ActionEnum action);
    Task<bool> HasRoleAsync(string userId, string roleName);
    Task<bool> HasAnyRoleAsync(string userId, params string[] roleNames);
    Task<bool> HasAllRolesAsync(string userId, params string[] roleNames);
    Task<bool> CanAccessResourceAsync(string userId, string resourceName);
    Task<bool> CanPerformActionAsync(string userId, string resourceName, string actionName);
    Task<IEnumerable<string>> GetUserPermissionsAsync(string userId);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    Task<bool> IsSystemAdminAsync(string userId);
    Task<bool> IsAdminAsync(string userId);
    Task<bool> IsManagerOrAboveAsync(string userId);
    Task<bool> IsBusinessUserOrAboveAsync(string userId);
    Task<string?> GetUserHighestRoleAsync(string userId);
    Task<string?> GetUserRoleCategoryAsync(string userId);
}

/// <summary>
/// Implementation of authorization service
/// </summary>
public class AuthorizationService : IAuthorizationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MyShopIdentityDbContext _context;
    private readonly IRolePermissionService _rolePermissionService;

    public AuthorizationService(UserManager<ApplicationUser> userManager, MyShopIdentityDbContext context,
        IRolePermissionService rolePermissionService)
    {
        _userManager = userManager;
        _context = context;
        _rolePermissionService = rolePermissionService;
    }

    public async Task<bool> HasPermissionAsync(string userId, string permissionName)
    {
        return await _rolePermissionService.UserHasPermissionAsync(userId, permissionName);
    }

    public async Task<bool> HasPermissionAsync(string userId, Resource resource, ActionEnum action)
    {
        return await _rolePermissionService.UserHasPermissionAsync(userId, resource, action);
    }

    public async Task<bool> HasRoleAsync(string userId, string roleName)
    {
        return await _rolePermissionService.UserHasRoleAsync(userId, roleName);
    }

    public async Task<bool> HasAnyRoleAsync(string userId, params string[] roleNames)
    {
        var userRoles = await _rolePermissionService.GetUserRolesAsync(userId);
        return roleNames.Any(roleName => userRoles.Any(role => role.Name == roleName));
    }

    public async Task<bool> HasAllRolesAsync(string userId, params string[] roleNames)
    {
        var userRoles = await _rolePermissionService.GetUserRolesAsync(userId);
        return roleNames.All(roleName => userRoles.Any(role => role.Name == roleName));
    }

    public async Task<bool> CanAccessResourceAsync(string userId, string resourceName)
    {
        var resource = ResourceExtensions.ParseFromString(resourceName);
        return await HasPermissionAsync(userId, resource, ActionEnum.Read);
    }

    public async Task<bool> CanPerformActionAsync(string userId, string resourceName, string actionName)
    {
        var resource = ResourceExtensions.ParseFromString(resourceName);
        var action = ActionExtensions.ParseFromString(actionName);
        return await HasPermissionAsync(userId, resource, action);
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(string userId)
    {
        return await _rolePermissionService.GetUserPermissionNamesAsync(userId);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        var roles = await _rolePermissionService.GetUserRolesAsync(userId);
        return roles.Select(r => r.Name);
    }

    public async Task<bool> IsSystemAdminAsync(string userId)
    {
        return await HasAnyRoleAsync(userId, RoleConstants.System.SuperAdmin, RoleConstants.System.SystemAdmin);
    }

    public async Task<bool> IsAdminAsync(string userId)
    {
        return await HasAnyRoleAsync(userId, RoleConstants.System.SuperAdmin, RoleConstants.System.SystemAdmin, RoleConstants.Administrative.Admin);
    }

    public async Task<bool> IsManagerOrAboveAsync(string userId)
    {
        return await HasAnyRoleAsync(userId, 
            RoleConstants.System.SuperAdmin, 
            RoleConstants.System.SystemAdmin, 
            RoleConstants.Administrative.Admin, 
            RoleConstants.Administrative.Manager);
    }

    public async Task<bool> IsBusinessUserOrAboveAsync(string userId)
    {
        return await HasAnyRoleAsync(userId, 
            RoleConstants.System.SuperAdmin, 
            RoleConstants.System.SystemAdmin, 
            RoleConstants.Administrative.Admin, 
            RoleConstants.Administrative.Manager,
            RoleConstants.Business.CustomerService,
            RoleConstants.Business.SalesRep,
            RoleConstants.Business.SupportAgent);
    }

    public async Task<string?> GetUserHighestRoleAsync(string userId)
    {
        var roles = await _rolePermissionService.GetUserRolesAsync(userId);
        
        // Define role priority (lower number = higher priority)
        var rolePriority = new Dictionary<string, int>
        {
            { RoleConstants.System.SuperAdmin, 1 },
            { RoleConstants.System.SystemAdmin, 2 },
            { RoleConstants.Administrative.Admin, 3 },
            { RoleConstants.Administrative.Manager, 4 },
            { RoleConstants.Business.CustomerService, 5 },
            { RoleConstants.Business.SalesRep, 6 },
            { RoleConstants.Business.SupportAgent, 7 },
            { RoleConstants.Specialized.Auditor, 5 },
            { RoleConstants.Specialized.ReportViewer, 6 },
            { RoleConstants.User.Customer, 8 },
            { RoleConstants.User.Guest, 9 }
        };

        return roles
            .Where(r => rolePriority.ContainsKey(r.Name))
            .OrderBy(r => rolePriority[r.Name])
            .FirstOrDefault()?.Name;
    }

    public async Task<string?> GetUserRoleCategoryAsync(string userId)
    {
        var highestRole = await GetUserHighestRoleAsync(userId);
        
        return highestRole switch
        {
            RoleConstants.System.SuperAdmin or RoleConstants.System.SystemAdmin => "System",
            RoleConstants.Administrative.Admin or RoleConstants.Administrative.Manager => "Administrative",
            RoleConstants.Business.CustomerService or RoleConstants.Business.SalesRep or RoleConstants.Business.SupportAgent => "Business",
            RoleConstants.Specialized.Auditor or RoleConstants.Specialized.ReportViewer => "Specialized",
            RoleConstants.User.Customer or RoleConstants.User.Guest => "User",
            _ => "Unknown"
        };
    }
}

/// <summary>
/// Authorization handler for resource-based authorization
/// </summary>
public class ResourceAuthorizationHandler : AuthorizationHandler<ResourceRequirement>
{
    private readonly IAuthorizationService _authorizationService;

    public ResourceAuthorizationHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        ResourceRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            return;
        }

        var hasPermission = await _authorizationService.HasPermissionAsync(userId, requirement.Resource, requirement.Action);
        
        if (hasPermission)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}

/// <summary>
/// Authorization requirement for resource-based authorization
/// </summary>
public class ResourceRequirement : IAuthorizationRequirement
{
    public Resource Resource { get; }
    public ActionEnum Action { get; }

    public ResourceRequirement(Resource resource, ActionEnum action)
    {
        Resource = resource;
        Action = action;
    }
}

/// <summary>
/// Authorization policy builder extensions
/// </summary>
public static class AuthorizationPolicyBuilderExtensions
{
    public static AuthorizationPolicyBuilder RequireResourcePermission(this AuthorizationPolicyBuilder builder, 
        Resource resource, ActionEnum action)
    {
        builder.Requirements.Add(new ResourceRequirement(resource, action));
        return builder;
    }

    public static AuthorizationPolicyBuilder RequireSystemAdmin(this AuthorizationPolicyBuilder builder)
    {
        builder.RequireRole(RoleConstants.System.SuperAdmin, RoleConstants.System.SystemAdmin);
        return builder;
    }

    public static AuthorizationPolicyBuilder RequireAdminOrAbove(this AuthorizationPolicyBuilder builder)
    {
        builder.RequireRole(
            RoleConstants.System.SuperAdmin,
            RoleConstants.System.SystemAdmin,
            RoleConstants.Administrative.Admin
        );
        return builder;
    }

    public static AuthorizationPolicyBuilder RequireManagerOrAbove(this AuthorizationPolicyBuilder builder)
    {
        builder.RequireRole(
            RoleConstants.System.SuperAdmin,
            RoleConstants.System.SystemAdmin,
            RoleConstants.Administrative.Admin,
            RoleConstants.Administrative.Manager
        );
        return builder;
    }

    public static AuthorizationPolicyBuilder RequireBusinessUserOrAbove(this AuthorizationPolicyBuilder builder)
    {
        builder.RequireRole(
            RoleConstants.System.SuperAdmin,
            RoleConstants.System.SystemAdmin,
            RoleConstants.Administrative.Admin,
            RoleConstants.Administrative.Manager,
            RoleConstants.Business.CustomerService,
            RoleConstants.Business.SalesRep,
            RoleConstants.Business.SupportAgent
        );
        return builder;
    }
}