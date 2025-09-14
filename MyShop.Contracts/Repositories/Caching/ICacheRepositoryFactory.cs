using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Repositories.Caching;
public interface ICacheRepositoryFactory
{
    ICacheRepository<T, TId> CreateCacheRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    ICacheRepository<T, TId> CreateCacheRepository<T, TId>(CacheConfiguration configuration) where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;
}