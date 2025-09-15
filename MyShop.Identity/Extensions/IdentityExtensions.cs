using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Models;
using MyShop.Identity.Enums;
using MyShop.Identity.Constants;
using MyShop.Identity.Context;

namespace MyShop.Identity.Extensions;

/// <summary>
/// Extension methods for Identity operations
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    /// Checks if user has a specific permission
    /// </summary>
    public static async Task<bool> HasPermissionAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user, string permissionName, MyShopIdentityDbContext context)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        
        // Check if any of the user's roles have the permission
        var hasPermission = await context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .Where(rp => userRoles.Contains(rp.Role.Name) && 
                        rp.Permission.Name == permissionName &&
                        rp.IsActive &&
                        rp.IsGranted &&
                        (rp.ExpiresAt == null || rp.ExpiresAt > DateTime.UtcNow))
            .AnyAsync();

        return hasPermission;
    }

    /// <summary>
    /// Checks if user has a specific role
    /// </summary>
    public static async Task<bool> HasRoleAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user, string roleName)
    {
        var roles = await userManager.GetRolesAsync(user);
        return roles.Contains(roleName);
    }

    /// <summary>
    /// Checks if user is a system administrator
    /// </summary>
    public static async Task<bool> IsSystemAdminAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user)
    {
        return await userManager.HasRoleAsync(user, RoleConstants.System.SuperAdmin) ||
               await userManager.HasRoleAsync(user, RoleConstants.System.SystemAdmin);
    }

    /// <summary>
    /// Checks if user is an administrator (any level)
    /// </summary>
    public static async Task<bool> IsAdminAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user)
    {
        return await userManager.IsSystemAdminAsync(user) ||
               await userManager.HasRoleAsync(user, RoleConstants.Administrative.Admin);
    }

    /// <summary>
    /// Checks if user is a manager or above
    /// </summary>
    public static async Task<bool> IsManagerOrAboveAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user)
    {
        return await userManager.IsAdminAsync(user) ||
               await userManager.HasRoleAsync(user, RoleConstants.Administrative.Manager);
    }

    /// <summary>
    /// Checks if user is a business user or above
    /// </summary>
    public static async Task<bool> IsBusinessUserOrAboveAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user)
    {
        return await userManager.IsManagerOrAboveAsync(user) ||
               await userManager.HasRoleAsync(user, RoleConstants.Business.CustomerService) ||
               await userManager.HasRoleAsync(user, RoleConstants.Business.SalesRep) ||
               await userManager.HasRoleAsync(user, RoleConstants.Business.SupportAgent);
    }

    /// <summary>
    /// Gets user's highest priority role
    /// </summary>
    public static async Task<string?> GetHighestPriorityRoleAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        
        // Define role priority (lower number = higher priority)
        var rolePriority = new Dictionary<string, int>
        {
            { RoleConstants.System.SuperAdmin, Constants.IdentityConstants.RolePriority.SuperAdmin },
            { RoleConstants.System.SystemAdmin, Constants.IdentityConstants.RolePriority.SystemAdmin },
            { RoleConstants.Administrative.Admin, Constants.IdentityConstants.RolePriority.Admin },
            { RoleConstants.Administrative.Manager, Constants.IdentityConstants.RolePriority.Manager },
            { RoleConstants.Business.CustomerService, Constants.IdentityConstants.RolePriority.CustomerService },
            { RoleConstants.Business.SalesRep, Constants.IdentityConstants.RolePriority.SalesRep },
            { RoleConstants.Business.SupportAgent, Constants.IdentityConstants.RolePriority.SupportAgent },
            { RoleConstants.Specialized.Auditor, Constants.IdentityConstants.RolePriority.Auditor },
            { RoleConstants.Specialized.ReportViewer, Constants.IdentityConstants.RolePriority.ReportViewer },
            { RoleConstants.User.Customer, Constants.IdentityConstants.RolePriority.Customer },
            { RoleConstants.User.Guest, Constants.IdentityConstants.RolePriority.Guest }
        };

        return roles
            .Where(r => rolePriority.ContainsKey(r))
            .OrderBy(r => rolePriority[r])
            .FirstOrDefault();
    }

    /// <summary>
    /// Gets user's role category
    /// </summary>
    public static async Task<string?> GetRoleCategoryAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user)
    {
        var highestRole = await userManager.GetHighestPriorityRoleAsync(user);
        
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

    /// <summary>
    /// Checks if user can perform action on resource
    /// </summary>
    public static async Task<bool> CanPerformActionAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user, Resource resource, ActionEnum action, MyShopIdentityDbContext context)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        
        // System admins can do everything
        if (userRoles.Contains(RoleConstants.System.SuperAdmin) || 
            userRoles.Contains(RoleConstants.System.SystemAdmin))
        {
            return true;
        }

        // Check specific permissions from database
        var permissionName = $"{resource.ToStringValue()}.{action.ToStringValue()}";
        
        var hasPermission = await context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .Where(rp => userRoles.Contains(rp.Role.Name) && 
                        rp.Permission.Name == permissionName &&
                        rp.IsActive &&
                        rp.IsGranted &&
                        (rp.ExpiresAt == null || rp.ExpiresAt > DateTime.UtcNow))
            .AnyAsync();

        return hasPermission;
    }

    /// <summary>
    /// Gets all permissions for a user through their roles
    /// </summary>
    public static async Task<IEnumerable<string>> GetUserPermissionsAsync(this UserManager<ApplicationUser> userManager, 
        ApplicationUser user, MyShopIdentityDbContext context)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        
        var permissions = await context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .Where(rp => userRoles.Contains(rp.Role.Name) && 
                        rp.IsActive &&
                        rp.IsGranted &&
                        (rp.ExpiresAt == null || rp.ExpiresAt > DateTime.UtcNow))
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToListAsync();

        return permissions;
    }

    /// <summary>
    /// Gets user's display name
    /// </summary>
    public static string GetDisplayName(this ApplicationUser user)
    {
        return !string.IsNullOrEmpty(user.UserName) ? user.UserName : user.Email ?? "Unknown User";
    }

    /// <summary>
    /// Checks if user account is in good standing
    /// </summary>
    public static bool IsInGoodStanding(this ApplicationUser user)
    {
        return user.IsActive && !user.IsLocked && !user.IsAccountLocked;
    }

    /// <summary>
    /// Gets user's account status
    /// </summary>
    public static string GetAccountStatus(this ApplicationUser user)
    {
        if (!user.IsActive)
            return "Inactive";
        
        if (user.IsLocked)
            return "Locked";
        
        if (user.IsAccountLocked)
            return "Account Locked";
        
        return "Active";
    }

    /// <summary>
    /// Checks if user needs password change
    /// </summary>
    public static bool NeedsPasswordChange(this ApplicationUser user)
    {
        return user.RequiresPasswordChange;
    }

    /// <summary>
    /// Gets days until password expires
    /// </summary>
    public static int? GetDaysUntilPasswordExpires(this ApplicationUser user)
    {
        if (user.LastPasswordChangeAt == null)
            return null;

        var daysSinceChange = DateTime.UtcNow.Subtract(user.LastPasswordChangeAt.Value).Days;
        return Math.Max(0, Constants.IdentityConstants.PasswordPolicy.ExpirationDays - daysSinceChange);
    }
}