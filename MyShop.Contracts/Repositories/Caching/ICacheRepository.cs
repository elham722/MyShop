namespace MyShop.Contracts.Repositories.Caching;
public interface ICacheRepository<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Basic Cache Operations

    Task<T?> GetCachedAsync(TId id, CancellationToken cancellationToken = default);

    Task SetCacheAsync(TId id, T entity, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

    Task RemoveCacheAsync(TId id, CancellationToken cancellationToken = default);

    Task ClearCacheAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Batch Cache Operations

    Task<Dictionary<TId, T>> GetCachedAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

    Task SetCacheAsync(Dictionary<TId, T> entities, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

    Task RemoveCacheAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

    #endregion

    #region Cache Pattern Operations

    Task<Dictionary<TId, T>> GetCachedByPatternAsync(string pattern, CancellationToken cancellationToken = default);

    Task RemoveCacheByPatternAsync(string pattern, CancellationToken cancellationToken = default);

    #endregion

    #region Cache Information Operations

    Task<bool> ExistsInCacheAsync(TId id, CancellationToken cancellationToken = default);

    Task<DateTime?> GetCacheExpirationAsync(TId id, CancellationToken cancellationToken = default);

    Task<CacheStatistics> GetCacheStatisticsAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Cache Refresh Operations

    Task RefreshCacheAsync(TId id, TimeSpan expiration, CancellationToken cancellationToken = default);

    Task ExtendCacheAsync(TId id, TimeSpan additionalTime, CancellationToken cancellationToken = default);

    #endregion
}