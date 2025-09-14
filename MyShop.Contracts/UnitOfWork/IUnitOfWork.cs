using MyShop.Domain.Shared.Base;
using MyShop.Domain.Shared.Events.Common;

namespace MyShop.Contracts.UnitOfWork;
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    #region Repository Management

    ICommandRepository<T, TId> GetCommandRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    IQueryRepository<T, TId> GetQueryRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    #endregion

    #region Cache Management

    ICacheRepository<T, TId> GetCacheRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    ICachedRepository<T, TId> GetCachedRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    #endregion

    #region Event Store Management

    IEventStore<T, TId> GetEventStore<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    IEventSourcedRepository<T, TId> GetEventSourcedRepository<T, TId>() where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    #endregion

    #region Transaction Management

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Change Tracking

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<bool> HasChangesAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Domain Events

    Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default);

    Task DispatchDomainEventsWithRollbackAsync(CancellationToken cancellationToken = default);

    Task DispatchDomainEventsAsync(Func<Exception, Task> onError, CancellationToken cancellationToken = default);

    Task<IEnumerable<BaseDomainEvent>> GetPendingDomainEventsAsync(CancellationToken cancellationToken = default);

    Task ClearPendingDomainEventsAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Event Sourcing Operations

    Task SaveEventsAsync<T, TId>(TId aggregateId, IEnumerable<BaseDomainEvent> events, int expectedVersion, CancellationToken cancellationToken = default)
        where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    Task<IEnumerable<BaseDomainEvent>> GetEventsAsync<T, TId>(TId aggregateId, CancellationToken cancellationToken = default)
        where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    Task<IEnumerable<BaseDomainEvent>> GetEventsAsync<T, TId>(TId aggregateId, int fromVersion, CancellationToken cancellationToken = default)
        where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>;

    #endregion

    #region Transaction State

    bool IsInTransaction { get; }

    string? TransactionId { get; }

    #endregion
}
