namespace MyShop.Contracts.Repositories.Command;
public interface IBulkCommandRepository<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Bulk Operations

    Task<int> BulkInsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<int> BulkUpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<int> BulkDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> BulkMergeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<int> BulkDeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

    Task<BulkOperationResult> BulkInsertAsync(IEnumerable<T> entities, BulkOperationOptions options, CancellationToken cancellationToken = default);

    Task<BulkOperationResult> BulkUpdateAsync(IEnumerable<T> entities, BulkOperationOptions options, CancellationToken cancellationToken = default);

    Task<BulkOperationResult> BulkDeleteAsync(IEnumerable<TId> ids, BulkOperationOptions options, CancellationToken cancellationToken = default);


    #endregion

    #region Bulk Configuration

    Task ConfigureBulkSettingsAsync(int batchSize = 1000, int bulkCopyTimeout = 30, CancellationToken cancellationToken = default);

    #endregion


    #region Bulk Operations with Audit

    Task<int> BulkInsertAsync(IEnumerable<T> entities, string createdBy, CancellationToken cancellationToken = default);

    Task<int> BulkUpdateAsync(IEnumerable<T> entities, string updatedBy, CancellationToken cancellationToken = default);

    Task<int> BulkDeleteAsync(IEnumerable<TId> ids, string deletedBy, CancellationToken cancellationToken = default);

    #endregion

    #region Bulk Operations with Audit Context

    Task<int> BulkInsertAsync(IEnumerable<T> entities, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task<int> BulkUpdateAsync(IEnumerable<T> entities, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task<int> BulkDeleteAsync(IEnumerable<TId> ids, IAuditContext auditContext, CancellationToken cancellationToken = default);

    #endregion
}