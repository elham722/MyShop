using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MyShop.Contracts.Enums.Identity;
using MyShop.Identity.Context;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services;

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

/// <summary>
/// Cache statistics model
/// </summary>
public class CacheStatistics
{
    public int TotalEntries { get; set; }
    public long TotalSize { get; set; }
    public int HitCount { get; set; }
    public int MissCount { get; set; }
    public double HitRate { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Cache entry model
/// </summary>
public class CacheEntry
{
    public string Key { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public long Size { get; set; }
    public int AccessCount { get; set; }
    public DateTime LastAccessed { get; set; }
}

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
                TotalEntries = _hitCounts.Count + _missCounts.Count,
                TotalSize = 0, // Placeholder
                HitCount = totalHits,
                MissCount = totalMisses,
                HitRate = hitRate
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