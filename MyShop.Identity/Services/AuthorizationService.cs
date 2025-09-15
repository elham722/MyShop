using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Identity.Constants;
using MyShop.Contracts.Enums.Identity;

namespace MyShop.Identity.Services;

public class AuthorizationService : Contracts.Identity.Services.IAuthorizationService
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


