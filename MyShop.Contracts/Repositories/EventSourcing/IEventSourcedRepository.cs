using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Repositories.EventSourcing;
public interface IEventSourcedRepository<T, TId> : ICommandRepository<T, TId>, IEventStore<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Event Sourcing Operations

    Task SaveAggregateAsync(T aggregate, CancellationToken cancellationToken = default);

    Task SaveAggregateAsync(T aggregate, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task<T?> LoadAggregateAsync(TId aggregateId, CancellationToken cancellationToken = default);

    Task<T?> LoadAggregateAsync(TId aggregateId, int toVersion, CancellationToken cancellationToken = default);

    Task<T?> LoadAggregateAsync(TId aggregateId, int fromVersion, int toVersion, CancellationToken cancellationToken = default);

    #endregion

    #region Snapshot Operations

    Task CreateSnapshotAsync(T aggregate, CancellationToken cancellationToken = default);

    Task<T?> LoadAggregateFromSnapshotAsync(TId aggregateId, CancellationToken cancellationToken = default);

    #endregion

    #region Event Replay Operations

    Task ReplayEventsAsync(TId aggregateId, CancellationToken cancellationToken = default);

    Task ReplayEventsAsync(TId aggregateId, int fromVersion, CancellationToken cancellationToken = default);

    Task ReplayEventsAsync(TId aggregateId, int fromVersion, int toVersion, CancellationToken cancellationToken = default);

    #endregion

    #region Event Store Statistics

    Task<EventStoreStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default);

    Task<AggregateStatistics> GetAggregateStatisticsAsync(TId aggregateId, CancellationToken cancellationToken = default);

    #endregion
}