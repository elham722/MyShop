namespace MyShop.Contracts.Repositories.Command;
public interface ICommandRepository<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Basic CRUD Operations

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion

    #region Audit Operations

    Task AddAsync(T entity, string createdBy, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<T> entities, string createdBy, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, string updatedBy, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<T> entities, string updatedBy, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, string deletedBy, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<T> entities, string deletedBy, CancellationToken cancellationToken = default);

    Task DeleteAsync(TId id, string deletedBy, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<TId> ids, string deletedBy, CancellationToken cancellationToken = default);

    #endregion

    #region Audit Context Operations

    Task AddAsync(T entity, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<T> entities, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<T> entities, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<T> entities, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task DeleteAsync(TId id, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<TId> ids, IAuditContext auditContext, CancellationToken cancellationToken = default);

    #endregion

  
}