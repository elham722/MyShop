using MyShop.Contracts.Enums.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MyShop.Contracts.DTOs.Identity;

namespace MyShop.Contracts.Identity.Services
{
    /// <summary>
    /// Service for managing roles and permissions
    /// </summary>
    public interface IRolePermissionService
    {
        // Role Management
        Task<RoleDto> CreateRoleAsync(string name, string description, string? category = null,
            int priority = 0, bool isSystemRole = false, string createdBy = "System");
        Task<RoleDto?> GetRoleAsync(string roleId);
        Task<RoleDto?> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<IEnumerable<RoleDto>> GetActiveRolesAsync();
        Task<IEnumerable<RoleDto>> GetRolesByCategoryAsync(string category);
        Task<bool> UpdateRoleAsync(string roleId, string name, string description,
            string? category = null, int? priority = null, string updatedBy = "System");
        Task<bool> DeactivateRoleAsync(string roleId, string deactivatedBy);
        Task<bool> ActivateRoleAsync(string roleId, string activatedBy);
        Task<bool> DeleteRoleAsync(string roleId);

        // Permission Management
        Task<PermissionDto> CreatePermissionAsync(string name, Resource resource, ActionEnum action,
            string description, string? category = null, int priority = 0,
            bool isSystemPermission = false, string createdBy = "System");
        Task<PermissionDto?> GetPermissionAsync(string permissionId);
        Task<PermissionDto?> GetPermissionByNameAsync(string permissionName);
        Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
        Task<IEnumerable<PermissionDto>> GetActivePermissionsAsync();
        Task<IEnumerable<PermissionDto>> GetPermissionsByResourceAsync(Resource resource);
        Task<IEnumerable<PermissionDto>> GetPermissionsByCategoryAsync(string category);
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
        Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(string roleId);
        Task<IEnumerable<RoleDto>> GetRolesWithPermissionAsync(string permissionId);
        Task<bool> HasPermissionAsync(string roleId, string permissionId);

        // User-Role Management
        Task<bool> AssignRoleToUserAsync(string userId, string roleId, string? assignedBy = null,
            DateTime? expiresAt = null, string? assignmentReason = null,
            string? assignmentCategory = null, int priority = 0, bool isTemporary = false,
            string? notes = null);
        Task<bool> RemoveRoleFromUserAsync(string userId, string roleId, string removedBy = "System");
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId);
        Task<IEnumerable<ApplicationUserDto>> GetUsersWithRoleAsync(string roleId);
        Task<bool> UserHasRoleAsync(string userId, string roleId);

        // Permission Checking
        Task<bool> UserHasPermissionAsync(string userId, string permissionName);
        Task<bool> UserHasPermissionAsync(string userId, Resource resource, ActionEnum action);
        Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(string userId);
        Task<IEnumerable<string>> GetUserPermissionNamesAsync(string userId);
    }

}
