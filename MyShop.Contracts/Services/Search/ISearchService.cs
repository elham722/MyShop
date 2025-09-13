namespace MyShop.Contracts.Services.Search;
public interface ISearchService<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Basic Search Operations

    Task<IEnumerable<T>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> SearchAsync(string searchTerm, Expression<Func<T, object>>[] searchFields, CancellationToken cancellationToken = default);

    Task<(IEnumerable<T> Items, int TotalCount)> SearchPagedAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    #endregion

    #region Advanced Search Operations

    Task<SearchResult<T, TId>> SearchAsync(ISearchCriteria<T, TId> searchCriteria, CancellationToken cancellationToken = default);

    Task<SearchResult<T, TId>> SearchPagedAsync(ISearchCriteria<T, TId> searchCriteria, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    #endregion

    #region Search Suggestions

    Task<IEnumerable<string>> GetSearchSuggestionsAsync(string searchTerm, int maxSuggestions = 10, CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetSearchSuggestionsAsync(string searchTerm, Expression<Func<T, object>>[] searchFields, int maxSuggestions = 10, CancellationToken cancellationToken = default);

    #endregion

    #region Search Analytics

    Task<SearchAnalytics> GetSearchAnalyticsAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<PopularSearchTerm>> GetPopularSearchTermsAsync(int limit = 20, CancellationToken cancellationToken = default);

    Task<SearchPerformanceMetrics> GetSearchPerformanceMetricsAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Search Configuration

    Task UpdateSearchConfigurationAsync(SearchConfiguration configuration, CancellationToken cancellationToken = default);

    Task<SearchConfiguration> GetSearchConfigurationAsync(CancellationToken cancellationToken = default);

    #endregion
}