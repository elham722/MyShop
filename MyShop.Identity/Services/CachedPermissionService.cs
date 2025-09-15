using Microsoft.Extensions.Logging;
using MyShop.Contracts.Enums.Identity;
using MyShop.Contracts.Identity.Services;
using MyShop.Identity.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services
{
    /// <summary>
    /// Cached permission service for improved performance
    /// </summary>
    public class CachedPermissionService
    {
        private readonly MyShopIdentityDbContext _context;
        private readonly IIdentityCacheService _cacheService;
        private readonly ILogger<CachedPermissionService> _logger;

        public CachedPermissionService(MyShopIdentityDbContext context, IIdentityCacheService cacheService,
            ILogger<CachedPermissionService> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Permission?> GetPermissionByIdAsync(string permissionId)
        {
            var cacheKey = $"permission:{permissionId}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _context.Permissions.FindAsync(permissionId);
            }, TimeSpan.FromMinutes(30));
        }

        public async Task<Permission?> GetPermissionByNameAsync(string permissionName)
        {
            var cacheKey = $"permission_name:{permissionName}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _context.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName);
            }, TimeSpan.FromMinutes(30));
        }

        public async Task<IList<Permission>> GetPermissionsAsync()
        {
            var cacheKey = "permissions:all";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _context.Permissions.ToListAsync();
            }, TimeSpan.FromMinutes(30));
        }

        public async Task<IList<Permission>> GetPermissionsByResourceAsync(Resource resource)
        {
            var cacheKey = $"permissions_resource:{resource}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _context.Permissions.Where(p => p.Resource == resource).ToListAsync();
            }, TimeSpan.FromMinutes(30));
        }

        public async Task InvalidatePermissionCacheAsync(string permissionId)
        {
            await _cacheService.InvalidatePermissionCacheAsync(permissionId);
        }
    }
}
