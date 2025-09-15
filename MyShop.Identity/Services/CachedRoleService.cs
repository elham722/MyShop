using Microsoft.Extensions.Logging;
using MyShop.Contracts.Identity.Services;
using MyShop.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Identity.Services
{
    /// <summary>
    /// Cached role service for improved performance
    /// </summary>
    public class CachedRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IIdentityCacheService _cacheService;
        private readonly ILogger<CachedRoleService> _logger;

        public CachedRoleService(RoleManager<Role> roleManager, IIdentityCacheService cacheService,
            ILogger<CachedRoleService> logger)
        {
            _roleManager = roleManager;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Role?> GetRoleByIdAsync(string roleId)
        {
            var cacheKey = $"role:{roleId}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _roleManager.FindByIdAsync(roleId);
            }, TimeSpan.FromMinutes(30));
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            var cacheKey = $"role_name:{roleName}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _roleManager.FindByNameAsync(roleName);
            }, TimeSpan.FromMinutes(30));
        }

        public async Task<IList<Role>> GetRolesAsync()
        {
            var cacheKey = "roles:all";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _roleManager.Roles.ToListAsync();
            }, TimeSpan.FromMinutes(30));
        }

        public async Task InvalidateRoleCacheAsync(string roleId)
        {
            await _cacheService.InvalidateRoleCacheAsync(roleId);
        }
    }

}
