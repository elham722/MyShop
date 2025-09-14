using MyShop.Domain.Shared.Base;
using MyShop.Domain.Shared.Specifications.Common;

namespace MyShop.Contracts.Repositories.Query;

/// <summary>
/// Simplified query repository interface with reduced overloads
/// </summary>
public interface IQueryRepository<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Basic Query Operations

    /// <summary>
    /// Gets entity by ID with optional includes
    /// </summary>
    Task<T?> GetByIdAsync(TId id, Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities with optional includes
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default);

    #endregion

    #region Search Operations

    /// <summary>
    /// Finds entities by predicate with optional includes
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets first entity by predicate with optional includes
    /// </summary>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default);

    #endregion

    #region Specification Operations

    /// <summary>
    /// Finds entities by specification
    /// </summary>
    Task<IEnumerable<T>> FindAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets first entity by specification
    /// </summary>
    Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if entity exists by specification
    /// </summary>
    Task<bool> ExistsAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts entities by specification
    /// </summary>
    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    #endregion

    #region Existence and Counting

    /// <summary>
    /// Checks if entity exists by ID
    /// </summary>
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if entity exists by predicate
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts all entities
    /// </summary>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts entities by predicate
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion

    #region Paging Operations

    /// <summary>
    /// Gets paged entities with optional predicate and includes
    /// </summary>
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? includes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paged entities by specification
    /// </summary>
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    #endregion

    #region Search Operations

    /// <summary>
    /// Searches entities with optional search fields and includes
    /// </summary>
    Task<IEnumerable<T>> SearchAsync(
        string searchTerm,
        Expression<Func<T, object>>[]? searchFields = null,
        Expression<Func<T, object>>[]? includes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches entities with paging
    /// </summary>
    Task<(IEnumerable<T> Items, int TotalCount)> SearchPagedAsync(
        string searchTerm,
        int pageNumber,
        int pageSize,
        Expression<Func<T, object>>[]? searchFields = null,
        Expression<Func<T, object>>[]? includes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts search results
    /// </summary>
    Task<int> SearchCountAsync(
        string searchTerm,
        Expression<Func<T, object>>[]? searchFields = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Advanced search with criteria
    /// </summary>
    Task<IEnumerable<T>> SearchAsync(ISearchCriteria<T, TId> searchCriteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Advanced search with paging
    /// </summary>
    Task<(IEnumerable<T> Items, int TotalCount)> SearchPagedAsync(ISearchCriteria<T, TId> searchCriteria, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Advanced search count
    /// </summary>
    Task<int> SearchCountAsync(ISearchCriteria<T, TId> searchCriteria, CancellationToken cancellationToken = default);

    #endregion

    #region Query Builder

    /// <summary>
    /// Creates a query builder for complex queries
    /// </summary>
    IQueryBuilder<T, TId> CreateQuery();

    #endregion
}
