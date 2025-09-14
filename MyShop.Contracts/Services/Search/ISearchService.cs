using MyShop.Contracts.DTOs.Search;

namespace MyShop.Contracts.Services.Search;

/// <summary>
/// Core search service interface - simplified and focused on search operations
/// </summary>
public interface ISearchService<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Basic Search Operations

    /// <summary>
    /// Searches entities with optional search fields
    /// </summary>
    Task<IEnumerable<T>> SearchAsync(
        string searchTerm, 
        Expression<Func<T, object>>[]? searchFields = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches entities with paging
    /// </summary>
    Task<(IEnumerable<T> Items, int TotalCount)> SearchPagedAsync(
        string searchTerm, 
        int pageNumber, 
        int pageSize,
        Expression<Func<T, object>>[]? searchFields = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts search results
    /// </summary>
    Task<int> SearchCountAsync(
        string searchTerm, 
        Expression<Func<T, object>>[]? searchFields = null, 
        CancellationToken cancellationToken = default);

    #endregion

    #region Advanced Search Operations

    /// <summary>
    /// Advanced search with criteria
    /// </summary>
    Task<SearchResult<T, TId>> SearchAsync(ISearchCriteria<T, TId> searchCriteria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Advanced search with paging
    /// </summary>
    Task<SearchResult<T, TId>> SearchPagedAsync(ISearchCriteria<T, TId> searchCriteria, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Advanced search count
    /// </summary>
    Task<int> SearchCountAsync(ISearchCriteria<T, TId> searchCriteria, CancellationToken cancellationToken = default);

    #endregion

}