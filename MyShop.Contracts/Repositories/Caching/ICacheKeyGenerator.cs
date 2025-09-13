namespace MyShop.Contracts.Repositories.Caching;
public interface ICacheKeyGenerator
{
    string GenerateKey<T, TId>(TId id) where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    string GeneratePattern<T>() where T : BaseAggregateRoot<Guid>;

    string GenerateKey(string prefix, object id);
}
