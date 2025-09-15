using MyShop.Contracts.DTOs.Identity.CacheService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.Identity.Services
{
    /// <summary>
    /// Service for managing Identity-related caching
    /// </summary>
    public interface IIdentityCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task RemoveByPatternAsync(string pattern);
        Task ClearAsync();
        Task<bool> ExistsAsync(string key);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        Task InvalidateUserCacheAsync(string userId);
        Task InvalidateRoleCacheAsync(string roleId);
        Task InvalidatePermissionCacheAsync(string permissionId);
        Task InvalidateAllCacheAsync();
        Task<CacheStatistics> GetCacheStatisticsAsync();
        Task<List<CacheEntry>> GetCacheEntriesAsync();
        Task<bool> IsCacheHealthyAsync();
    }

}
