using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Repositories.EventSourcing;
public interface IEventStoreFactory
{
    IEventStore<T, TId> CreateEventStore<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    IEventStore<T, TId> CreateEventStore<T, TId>(EventStoreConfiguration configuration) where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;
}
