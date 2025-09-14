using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Repositories.Caching;
public interface ICachedRepository<T, TId> : ICommandRepository<T, TId>, IQueryRepository<T, TId>, ICacheRepository<T, TId>
    where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Cached Query Operations

    Task<T?> GetByIdWithCacheAsync(TId id, CancellationToken cancellationToken = default);

    Task<T?> GetByIdWithCacheAsync(TId id, Expression<Func<T, object>>[] includes, CancellationToken cancellationToken = default);

    Task<Dictionary<TId, T>> GetByIdsWithCacheAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

    #endregion

    #region Cached Command Operations

    Task AddWithCacheAsync(T entity, TimeSpan? cacheExpiration = null, CancellationToken cancellationToken = default);

    Task UpdateWithCacheAsync(T entity, TimeSpan? cacheExpiration = null, CancellationToken cancellationToken = default);

    Task DeleteWithCacheAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteWithCacheAsync(TId id, CancellationToken cancellationToken = default);

    #endregion

    #region Cache Management Operations

    Task WarmUpCacheAsync(CancellationToken cancellationToken = default);

    Task WarmUpCacheAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

    Task InvalidateCacheByPatternAsync(string pattern, CancellationToken cancellationToken = default);

    Task InvalidateAllCacheAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Cache Statistics Operations

    Task<CachePerformanceStatistics> GetCachePerformanceStatisticsAsync(CancellationToken cancellationToken = default);

    Task ResetCacheStatisticsAsync(CancellationToken cancellationToken = default);

    #endregion
}
