using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Services.Command;
public interface ICommandService<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Basic CRUD Operations

    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

    #endregion

    #region Audit Operations

    Task<T> CreateAsync(T entity, string createdBy, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities, string createdBy, CancellationToken cancellationToken = default);

    Task<T> UpdateAsync(T entity, string updatedBy, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, string updatedBy, CancellationToken cancellationToken = default);

    Task DeleteAsync(TId id, string deletedBy, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<TId> ids, string deletedBy, CancellationToken cancellationToken = default);

    #endregion

    #region Audit Context Operations

    Task<T> CreateAsync(T entity, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task<T> UpdateAsync(T entity, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task DeleteAsync(TId id, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<TId> ids, IAuditContext auditContext, CancellationToken cancellationToken = default);

    #endregion

    #region Validation Operations

    Task<T> CreateAsync(T entity, ValidationOptions validationOptions, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities, ValidationOptions validationOptions, CancellationToken cancellationToken = default);

    Task<T> UpdateAsync(T entity, ValidationOptions validationOptions, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, ValidationOptions validationOptions, CancellationToken cancellationToken = default);

    #endregion

    #region Business Rules Operations

    Task<T> CreateAsync(T entity, BusinessRuleValidationOptions businessRuleOptions, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities, BusinessRuleValidationOptions businessRuleOptions, CancellationToken cancellationToken = default);

    Task<T> UpdateAsync(T entity, BusinessRuleValidationOptions businessRuleOptions, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, BusinessRuleValidationOptions businessRuleOptions, CancellationToken cancellationToken = default);

    #endregion

    #region Combined Validation and Business Rules Operations

    Task<T> CreateAsync(T entity, ValidationOptions validationOptions, BusinessRuleValidationOptions businessRuleOptions, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities, ValidationOptions validationOptions, BusinessRuleValidationOptions businessRuleOptions, CancellationToken cancellationToken = default);

    Task<T> UpdateAsync(T entity, ValidationOptions validationOptions, BusinessRuleValidationOptions businessRuleOptions, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, ValidationOptions validationOptions, BusinessRuleValidationOptions businessRuleOptions, CancellationToken cancellationToken = default);

    #endregion

    #region Business Operations

    Task<T> ExecuteBusinessOperationAsync(TId id, Func<T, Task> operation, CancellationToken cancellationToken = default);

    Task<T> ExecuteBusinessOperationAsync<TResult>(TId id, Func<T, Task<TResult>> operation, CancellationToken cancellationToken = default);

    Task<T> ExecuteBusinessOperationAsync(TId id, Func<T, Task> operation, string updatedBy, CancellationToken cancellationToken = default);

    Task<T> ExecuteBusinessOperationAsync<TResult>(TId id, Func<T, Task<TResult>> operation, string updatedBy, CancellationToken cancellationToken = default);

    Task<T> ExecuteBusinessOperationAsync(TId id, Func<T, Task> operation, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task<T> ExecuteBusinessOperationAsync<TResult>(TId id, Func<T, Task<TResult>> operation, IAuditContext auditContext, CancellationToken cancellationToken = default);

    #endregion

    #region Bulk Operations

    Task<int> BulkCreateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<int> BulkUpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<int> BulkDeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

    Task<BulkOperationResult> BulkCreateAsync(IEnumerable<T> entities, BulkOperationOptions options, CancellationToken cancellationToken = default);

    Task<BulkOperationResult> BulkUpdateAsync(IEnumerable<T> entities, BulkOperationOptions options, CancellationToken cancellationToken = default);

    Task<BulkOperationResult> BulkDeleteAsync(IEnumerable<TId> ids, BulkOperationOptions options, CancellationToken cancellationToken = default);

    #endregion

    #region Bulk Operations with Audit

    Task<int> BulkCreateAsync(IEnumerable<T> entities, string createdBy, CancellationToken cancellationToken = default);

    Task<int> BulkUpdateAsync(IEnumerable<T> entities, string updatedBy, CancellationToken cancellationToken = default);

    Task<int> BulkDeleteAsync(IEnumerable<TId> ids, string deletedBy, CancellationToken cancellationToken = default);

    #endregion

    #region Bulk Operations with Audit Context

    Task<int> BulkCreateAsync(IEnumerable<T> entities, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task<int> BulkUpdateAsync(IEnumerable<T> entities, IAuditContext auditContext, CancellationToken cancellationToken = default);

    Task<int> BulkDeleteAsync(IEnumerable<TId> ids, IAuditContext auditContext, CancellationToken cancellationToken = default);

    #endregion

    #region Domain Events

    Task PublishDomainEventsAsync(CancellationToken cancellationToken = default);

    #endregion
}