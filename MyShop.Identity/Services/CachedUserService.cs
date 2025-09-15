using Microsoft.Extensions.Logging;
using MyShop.Contracts.Identity.Services;
using MyShop.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Identity.Services
{
    /// <summary>
    /// Cached user service for improved performance
    /// </summary>
    public class CachedUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityCacheService _cacheService;
        private readonly ILogger<CachedUserService> _logger;

        public CachedUserService(UserManager<ApplicationUser> userManager, IIdentityCacheService cacheService,
            ILogger<CachedUserService> logger)
        {
            _userManager = userManager;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            var cacheKey = $"user:{userId}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _userManager.FindByIdAsync(userId);
            }, TimeSpan.FromMinutes(15));
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            var cacheKey = $"user_email:{email}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _userManager.FindByEmailAsync(email);
            }, TimeSpan.FromMinutes(15));
        }

        public async Task<ApplicationUser?> GetUserByUserNameAsync(string userName)
        {
            var cacheKey = $"user_username:{userName}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _userManager.FindByNameAsync(userName);
            }, TimeSpan.FromMinutes(15));
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            var cacheKey = $"user_roles:{user.Id}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _userManager.GetRolesAsync(user);
            }, TimeSpan.FromMinutes(10));
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            var roles = await GetUserRolesAsync(user);
            return roles.Contains(role);
        }

        public async Task InvalidateUserCacheAsync(string userId)
        {
            await _cacheService.InvalidateUserCacheAsync(userId);
        }
    }
}
