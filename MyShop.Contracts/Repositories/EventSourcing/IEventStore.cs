namespace MyShop.Contracts.Repositories.EventSourcing;
public interface IEventStore<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Event Storage Operations

    Task SaveEventsAsync(TId aggregateId, IEnumerable<BaseDomainEvent> events, int expectedVersion, CancellationToken cancellationToken = default);

    Task SaveEventsAsync(TId aggregateId, IEnumerable<BaseDomainEvent> events, int expectedVersion, IAuditContext auditContext, CancellationToken cancellationToken = default);

    #endregion

    #region Event Retrieval Operations

    Task<IEnumerable<BaseDomainEvent>> GetEventsAsync(TId aggregateId, CancellationToken cancellationToken = default);

    Task<IEnumerable<BaseDomainEvent>> GetEventsAsync(TId aggregateId, int fromVersion, CancellationToken cancellationToken = default);

    Task<IEnumerable<BaseDomainEvent>> GetEventsAsync(TId aggregateId, int fromVersion, int toVersion, CancellationToken cancellationToken = default);

    #endregion

    #region Aggregate State Operations

    Task<int> GetCurrentVersionAsync(TId aggregateId, CancellationToken cancellationToken = default);

    Task<bool> AggregateExistsAsync(TId aggregateId, CancellationToken cancellationToken = default);

    Task<DateTime?> GetAggregateCreatedAtAsync(TId aggregateId, CancellationToken cancellationToken = default);

    Task<DateTime?> GetAggregateLastModifiedAtAsync(TId aggregateId, CancellationToken cancellationToken = default);

    #endregion

    #region Event Store Management

    Task<IEnumerable<TId>> GetAllAggregateIdsAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<TId>> GetAggregateIdsByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

    Task<long> GetTotalEventCountAsync(CancellationToken cancellationToken = default);

    Task<long> GetTotalAggregateCountAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Snapshot Operations

    Task SaveSnapshotAsync(TId aggregateId, byte[] snapshot, int version, CancellationToken cancellationToken = default);

    Task<byte[]?> GetLatestSnapshotAsync(TId aggregateId, CancellationToken cancellationToken = default);

    Task<byte[]?> GetSnapshotAsync(TId aggregateId, int version, CancellationToken cancellationToken = default);

    #endregion
}
