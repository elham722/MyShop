using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Identity.Enums;
using MyShop.Identity.Constants;

namespace MyShop.Identity.Services;

/// <summary>
/// Service for managing roles and permissions
/// </summary>
public interface IRolePermissionService
{
    // Role Management
    Task<Role> CreateRoleAsync(string name, string description, string? category = null, 
        int priority = 0, bool isSystemRole = false, string createdBy = "System");
    Task<Role?> GetRoleAsync(string roleId);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<IEnumerable<Role>> GetActiveRolesAsync();
    Task<IEnumerable<Role>> GetRolesByCategoryAsync(string category);
    Task<bool> UpdateRoleAsync(string roleId, string name, string description, 
        string? category = null, int? priority = null, string updatedBy = "System");
    Task<bool> DeactivateRoleAsync(string roleId, string deactivatedBy);
    Task<bool> ActivateRoleAsync(string roleId, string activatedBy);
    Task<bool> DeleteRoleAsync(string roleId);

    // Permission Management
    Task<Permission> CreatePermissionAsync(string name, Resource resource, ActionEnum action, 
        string description, string? category = null, int priority = 0, 
        bool isSystemPermission = false, string createdBy = "System");
    Task<Permission?> GetPermissionAsync(string permissionId);
    Task<Permission?> GetPermissionByNameAsync(string permissionName);
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();
    Task<IEnumerable<Permission>> GetActivePermissionsAsync();
    Task<IEnumerable<Permission>> GetPermissionsByResourceAsync(Resource resource);
    Task<IEnumerable<Permission>> GetPermissionsByCategoryAsync(string category);
    Task<bool> UpdatePermissionAsync(string permissionId, string name, Resource resource, 
        ActionEnum action, string description, string? category = null, 
        int? priority = null, string updatedBy = "System");
    Task<bool> DeactivatePermissionAsync(string permissionId, string deactivatedBy);
    Task<bool> ActivatePermissionAsync(string permissionId, string activatedBy);
    Task<bool> DeletePermissionAsync(string permissionId);

    // Role-Permission Management
    Task<bool> AssignPermissionToRoleAsync(string roleId, string permissionId, 
        string? assignedBy = null, DateTime? expiresAt = null);
    Task<bool> RemovePermissionFromRoleAsync(string roleId, string permissionId, 
        string removedBy = "System");
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(string roleId);
    Task<IEnumerable<Role>> GetRolesWithPermissionAsync(string permissionId);
    Task<bool> HasPermissionAsync(string roleId, string permissionId);

    // User-Role Management
    Task<bool> AssignRoleToUserAsync(string userId, string roleId, string? assignedBy = null, 
        DateTime? expiresAt = null, string? assignmentReason = null, 
        string? assignmentCategory = null, int priority = 0, bool isTemporary = false, 
        string? notes = null);
    Task<bool> RemoveRoleFromUserAsync(string userId, string roleId, string removedBy = "System");
    Task<IEnumerable<Role>> GetUserRolesAsync(string userId);
    Task<IEnumerable<ApplicationUser>> GetUsersWithRoleAsync(string roleId);
    Task<bool> UserHasRoleAsync(string userId, string roleId);

    // Permission Checking
    Task<bool> UserHasPermissionAsync(string userId, string permissionName);
    Task<bool> UserHasPermissionAsync(string userId, Resource resource, ActionEnum action);
    Task<IEnumerable<Permission>> GetUserPermissionsAsync(string userId);
    Task<IEnumerable<string>> GetUserPermissionNamesAsync(string userId);
}

/// <summary>
/// Implementation of role and permission service
/// </summary>
public class RolePermissionService : IRolePermissionService
{
    private readonly MyShopIdentityDbContext _context;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RolePermissionService(MyShopIdentityDbContext context, RoleManager<Role> roleManager, 
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    #region Role Management

    public async Task<Role> CreateRoleAsync(string name, string description, string? category = null, 
        int priority = 0, bool isSystemRole = false, string createdBy = "System")
    {
        var role = Role.Create(name, description, category, priority, isSystemRole, createdBy);
        var result = await _roleManager.CreateAsync(role);
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return role;
    }

    public async Task<Role?> GetRoleAsync(string roleId)
    {
        return await _roleManager.FindByIdAsync(roleId);
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _roleManager.FindByNameAsync(roleName);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<IEnumerable<Role>> GetActiveRolesAsync()
    {
        return await _context.Roles.Where(r => r.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Role>> GetRolesByCategoryAsync(string category)
    {
        return await _context.Roles.Where(r => r.Category == category && r.IsActive).ToListAsync();
    }

    public async Task<bool> UpdateRoleAsync(string roleId, string name, string description, 
        string? category = null, int? priority = null, string updatedBy = "System")
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null) return false;

        try
        {
            role.Update(name, description, category, priority, updatedBy);
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeactivateRoleAsync(string roleId, string deactivatedBy)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null) return false;

        try
        {
            role.Deactivate(deactivatedBy);
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ActivateRoleAsync(string roleId, string activatedBy)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null) return false;

        try
        {
            role.Activate(activatedBy);
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null) return false;

        try
        {
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region Permission Management

    public async Task<Permission> CreatePermissionAsync(string name, Resource resource, ActionEnum action, 
        string description, string? category = null, int priority = 0, 
        bool isSystemPermission = false, string createdBy = "System")
    {
        var permission = Permission.Create(name, resource, action, description, category, priority, isSystemPermission, createdBy);
        _context.Permissions.Add(permission);
        await _context.SaveChangesAsync();
        return permission;
    }

    public async Task<Permission?> GetPermissionAsync(string permissionId)
    {
        return await _context.Permissions.FindAsync(permissionId);
    }

    public async Task<Permission?> GetPermissionByNameAsync(string permissionName)
    {
        return await _context.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName);
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        return await _context.Permissions.ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetActivePermissionsAsync()
    {
        return await _context.Permissions.Where(p => p.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByResourceAsync(Resource resource)
    {
        return await _context.Permissions.Where(p => p.Resource == resource && p.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByCategoryAsync(string category)
    {
        return await _context.Permissions.Where(p => p.Category == category && p.IsActive).ToListAsync();
    }

    public async Task<bool> UpdatePermissionAsync(string permissionId, string name, Resource resource, 
        ActionEnum action, string description, string? category = null, 
        int? priority = null, string updatedBy = "System")
    {
        var permission = await _context.Permissions.FindAsync(permissionId);
        if (permission == null) return false;

        try
        {
            permission.Update(name, resource, action, description, category, priority, updatedBy);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeactivatePermissionAsync(string permissionId, string deactivatedBy)
    {
        var permission = await _context.Permissions.FindAsync(permissionId);
        if (permission == null) return false;

        try
        {
            permission.Deactivate(deactivatedBy);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ActivatePermissionAsync(string permissionId, string activatedBy)
    {
        var permission = await _context.Permissions.FindAsync(permissionId);
        if (permission == null) return false;

        try
        {
            permission.Activate(activatedBy);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeletePermissionAsync(string permissionId)
    {
        var permission = await _context.Permissions.FindAsync(permissionId);
        if (permission == null) return false;

        try
        {
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region Role-Permission Management

    public async Task<bool> AssignPermissionToRoleAsync(string roleId, string permissionId, 
        string? assignedBy = null, DateTime? expiresAt = null)
    {
        var existingRolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (existingRolePermission != null)
        {
            if (!existingRolePermission.IsActive)
            {
                existingRolePermission.Activate(assignedBy ?? "System");
                await _context.SaveChangesAsync();
            }
            return true;
        }

        var rolePermission = RolePermission.Create(roleId, permissionId, assignedBy, expiresAt);
        _context.RolePermissions.Add(rolePermission);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemovePermissionFromRoleAsync(string roleId, string permissionId, 
        string removedBy = "System")
    {
        var rolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (rolePermission == null) return false;

        rolePermission.Deactivate(removedBy);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(string roleId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId && rp.IsActive && rp.IsGranted)
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission)
            .ToListAsync();
    }

    public async Task<IEnumerable<Role>> GetRolesWithPermissionAsync(string permissionId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.PermissionId == permissionId && rp.IsActive && rp.IsGranted)
            .Include(rp => rp.Role)
            .Select(rp => rp.Role)
            .ToListAsync();
    }

    public async Task<bool> HasPermissionAsync(string roleId, string permissionId)
    {
        return await _context.RolePermissions
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && 
                           rp.IsActive && rp.IsGranted);
    }

    #endregion

    #region User-Role Management

    public async Task<bool> AssignRoleToUserAsync(string userId, string roleId, string? assignedBy = null, 
        DateTime? expiresAt = null, string? assignmentReason = null, 
        string? assignmentCategory = null, int priority = 0, bool isTemporary = false, 
        string? notes = null)
    {
        var existingUserRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (existingUserRole != null)
        {
            if (!existingUserRole.IsActive)
            {
                existingUserRole.Activate(assignedBy ?? "System");
                await _context.SaveChangesAsync();
            }
            return true;
        }

        var userRole = UserRole.Create(userId, roleId, assignedBy, assignmentReason, expiresAt, 
            assignmentCategory, priority, isTemporary, notes);
        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleId, string removedBy = "System")
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (userRole == null) return false;

        userRole.Deactivate(removedBy);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(string userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId && ur.IsActive)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersWithRoleAsync(string roleId)
    {
        return await _context.UserRoles
            .Where(ur => ur.RoleId == roleId && ur.IsActive)
            .Include(ur => ur.User)
            .Select(ur => ur.User)
            .ToListAsync();
    }

    public async Task<bool> UserHasRoleAsync(string userId, string roleId)
    {
        return await _context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId && ur.IsActive);
    }

    #endregion

    #region Permission Checking

    public async Task<bool> UserHasPermissionAsync(string userId, string permissionName)
    {
        var permission = await GetPermissionByNameAsync(permissionName);
        if (permission == null) return false;

        return await UserHasPermissionAsync(userId, permission.Resource, permission.Action);
    }

    public async Task<bool> UserHasPermissionAsync(string userId, Resource resource, ActionEnum action)
    {
        var userRoles = await GetUserRolesAsync(userId);
        
        foreach (var role in userRoles)
        {
            var rolePermissions = await GetRolePermissionsAsync(role.Id);
            if (rolePermissions.Any(p => p.Resource == resource && p.Action == action))
            {
                return true;
            }
        }

        return false;
    }

    public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(string userId)
    {
        var userRoles = await GetUserRolesAsync(userId);
        var allPermissions = new List<Permission>();

        foreach (var role in userRoles)
        {
            var rolePermissions = await GetRolePermissionsAsync(role.Id);
            allPermissions.AddRange(rolePermissions);
        }

        return allPermissions.DistinctBy(p => p.Id);
    }

    public async Task<IEnumerable<string>> GetUserPermissionNamesAsync(string userId)
    {
        var permissions = await GetUserPermissionsAsync(userId);
        return permissions.Select(p => p.Name);
    }

    #endregion
}