using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MyShop.Contracts.DTOs.Identity.CacheService;
using MyShop.Contracts.Enums.Identity;
using MyShop.Contracts.Identity.Services;
using MyShop.Identity.Context;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services;
/// <summary>
/// Implementation of identity cache service
/// </summary>
public class IdentityCacheService : IIdentityCacheService
{
    private readonly IMemoryCache _cache;
    private readonly MyShopIdentityDbContext _context;
    private readonly ILogger<IdentityCacheService> _logger;
    private readonly Dictionary<string, int> _hitCounts = new();
    private readonly Dictionary<string, int> _missCounts = new();

    public IdentityCacheService(IMemoryCache cache, MyShopIdentityDbContext context,
        ILogger<IdentityCacheService> logger)
    {
        _cache = cache;
        _context = context;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            if (_cache.TryGetValue(key, out var value))
            {
                _hitCounts[key] = _hitCounts.GetValueOrDefault(key, 0) + 1;
                return (T)value;
            }

            _missCounts[key] = _missCounts.GetValueOrDefault(key, 0) + 1;
            return default(T);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache value for key: {Key}", key);
            return default(T);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var options = new MemoryCacheEntryOptions
            {
                SlidingExpiration = expiration ?? TimeSpan.FromMinutes(15),
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1),
                Priority = CacheItemPriority.Normal
            };

            _cache.Set(key, value, options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting cache value for key: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            _cache.Remove(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache value for key: {Key}", key);
        }
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        try
        {
            // In a real implementation, you would need to track cache keys
            // For now, we'll just log the pattern
            _logger.LogInformation("Removing cache entries matching pattern: {Pattern}", pattern);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache entries for pattern: {Pattern}", pattern);
        }
    }

    public async Task ClearAsync()
    {
        try
        {
            if (_cache is MemoryCache memoryCache)
            {
                memoryCache.Clear();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cache");
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        try
        {
            return _cache.TryGetValue(key, out _);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking cache existence for key: {Key}", key);
            return false;
        }
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        try
        {
            if (_cache.TryGetValue(key, out var value))
            {
                _hitCounts[key] = _hitCounts.GetValueOrDefault(key, 0) + 1;
                return (T)value;
            }

            _missCounts[key] = _missCounts.GetValueOrDefault(key, 0) + 1;
            var newValue = await factory();
            await SetAsync(key, newValue, expiration);
            return newValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetOrSet for key: {Key}", key);
            return await factory();
        }
    }

    public async Task InvalidateUserCacheAsync(string userId)
    {
        try
        {
            var keysToRemove = new[]
            {
                $"user:{userId}",
                $"user_roles:{userId}",
                $"user_permissions:{userId}",
                $"user_profile:{userId}",
                $"user_security:{userId}",
                $"user_activity:{userId}"
            };

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }

            _logger.LogInformation("Invalidated cache for user: {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating user cache for userId: {UserId}", userId);
        }
    }

    public async Task InvalidateRoleCacheAsync(string roleId)
    {
        try
        {
            var keysToRemove = new[]
            {
                $"role:{roleId}",
                $"role_permissions:{roleId}",
                $"role_users:{roleId}"
            };

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }

            _logger.LogInformation("Invalidated cache for role: {RoleId}", roleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating role cache for roleId: {RoleId}", roleId);
        }
    }

    public async Task InvalidatePermissionCacheAsync(string permissionId)
    {
        try
        {
            var keysToRemove = new[]
            {
                $"permission:{permissionId}",
                $"permission_roles:{permissionId}",
                $"permission_users:{permissionId}"
            };

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }

            _logger.LogInformation("Invalidated cache for permission: {PermissionId}", permissionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating permission cache for permissionId: {PermissionId}", permissionId);
        }
    }

    public async Task InvalidateAllCacheAsync()
    {
        try
        {
            await ClearAsync();
            _logger.LogInformation("Invalidated all cache");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating all cache");
        }
    }

    public async Task<CacheStatistics> GetCacheStatisticsAsync()
    {
        try
        {
            var totalHits = _hitCounts.Values.Sum();
            var totalMisses = _missCounts.Values.Sum();
            var totalRequests = totalHits + totalMisses;
            var hitRate = totalRequests > 0 ? (double)totalHits / totalRequests * 100 : 0;

            return new CacheStatistics
            {
                TotalCachedItems = _hitCounts.Count + _missCounts.Count,
                TotalCacheSizeBytes = 0, // Placeholder
                CacheHits = totalHits,
                CacheMisses = totalMisses,
                MemoryUsagePercentage = hitRate
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache statistics");
            return new CacheStatistics();
        }
    }

    public async Task<List<CacheEntry>> GetCacheEntriesAsync()
    {
        try
        {
            var entries = new List<CacheEntry>();

            foreach (var hit in _hitCounts)
            {
                entries.Add(new CacheEntry
                {
                    Key = hit.Key,
                    Type = "Hit",
                    CreatedAt = DateTime.UtcNow,
                    AccessCount = hit.Value,
                    LastAccessed = DateTime.UtcNow
                });
            }

            foreach (var miss in _missCounts)
            {
                entries.Add(new CacheEntry
                {
                    Key = miss.Key,
                    Type = "Miss",
                    CreatedAt = DateTime.UtcNow,
                    AccessCount = miss.Value,
                    LastAccessed = DateTime.UtcNow
                });
            }

            return entries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache entries");
            return new List<CacheEntry>();
        }
    }

    public async Task<bool> IsCacheHealthyAsync()
    {
        try
        {
            // Test cache functionality
            var testKey = "health_check_" + Guid.NewGuid();
            var testValue = "test_value";

            await SetAsync(testKey, testValue, TimeSpan.FromSeconds(1));
            var retrievedValue = await GetAsync<string>(testKey);
            await RemoveAsync(testKey);

            return retrievedValue == testValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache health check failed");
            return false;
        }
    }
}
