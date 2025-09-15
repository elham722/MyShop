using Microsoft.AspNetCore.Identity;
using MyShop.Identity.Models;
using MyShop.Identity.Enums;
using MyShop.Identity.Constants;

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
        ApplicationUser user, string permissionName)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        // Implementation would check if any of the user's roles have the permission
        // This is a simplified version - in real implementation, you'd query the database
        return true; // Placeholder
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
        ApplicationUser user, Resource resource, ActionEnum action)
    {
        // This is a simplified implementation
        // In a real scenario, you'd check the user's roles and their permissions
        
        var userRoles = await userManager.GetRolesAsync(user);
        
        // System admins can do everything
        if (userRoles.Contains(RoleConstants.System.SuperAdmin) || 
            userRoles.Contains(RoleConstants.System.SystemAdmin))
        {
            return true;
        }

        // Check specific permissions based on role and resource/action
        return await CheckResourceActionPermissionAsync(userRoles, resource, action);
    }

    private static async Task<bool> CheckResourceActionPermissionAsync(IList<string> userRoles, 
        Resource resource, ActionEnum action)
    {
        // Simplified permission checking logic
        // In real implementation, you'd query the database for role-permission mappings
        
        var permissionName = $"{resource.ToStringValue()}.{action.ToStringValue()}";
        
        // Define which roles can perform which actions
        var rolePermissions = new Dictionary<string, List<string>>
        {
            { RoleConstants.Administrative.Admin, new List<string> { "Customer.Create", "Customer.Read", "Customer.Update", "Customer.List", "Order.Create", "Order.Read", "Order.Update", "Order.List", "Product.Create", "Product.Read", "Product.Update", "Product.List" } },
            { RoleConstants.Administrative.Manager, new List<string> { "Customer.Read", "Customer.List", "Customer.Update", "Order.Read", "Order.List", "Order.Approve", "Product.Read", "Product.List" } },
            { RoleConstants.Business.CustomerService, new List<string> { "Customer.Read", "Customer.List", "Customer.Update", "Order.Read", "Order.List", "Product.Read", "Product.List" } },
            { RoleConstants.Business.SalesRep, new List<string> { "Customer.Create", "Customer.Read", "Customer.List", "Order.Create", "Order.Read", "Order.List", "Product.Read", "Product.List" } },
            { RoleConstants.Business.SupportAgent, new List<string> { "Customer.Read", "Customer.List", "Order.Read", "Order.List", "Product.Read", "Product.List" } },
            { RoleConstants.User.Customer, new List<string> { "Order.Create", "Order.Read", "Order.List", "Product.Read", "Product.List", "Payment.Create", "Payment.Read", "Payment.List" } },
            { RoleConstants.Specialized.Auditor, new List<string> { "Audit.Read", "Audit.List", "User.Read", "User.List" } },
            { RoleConstants.Specialized.ReportViewer, new List<string> { "Report.Read", "Report.List", "Report.Execute" } }
        };

        foreach (var role in userRoles)
        {
            if (rolePermissions.ContainsKey(role) && 
                rolePermissions[role].Contains(permissionName))
            {
                return true;
            }
        }

        return false;
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
        return Math.Max(0, 90 - daysSinceChange);
    }
}