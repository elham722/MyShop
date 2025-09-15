using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Contracts.Enums.Identity;

namespace MyShop.Contracts.Identity.Services
{
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

}
