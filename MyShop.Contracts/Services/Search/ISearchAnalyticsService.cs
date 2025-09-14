using MyShop.Contracts.DTOs.Search;
using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Services.Search;

/// <summary>
/// Service for search analytics and performance metrics
/// </summary>
public interface ISearchAnalyticsService<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Search Analytics

    /// <summary>
    /// Gets comprehensive search analytics
    /// </summary>
    Task<SearchAnalytics> GetSearchAnalyticsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets popular search terms
    /// </summary>
    Task<IEnumerable<PopularSearchTerm>> GetPopularSearchTermsAsync(int limit = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets search performance metrics
    /// </summary>
    Task<SearchPerformanceMetrics> GetSearchPerformanceMetricsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets search trends over time
    /// </summary>
    Task<IEnumerable<SearchTrend>> GetSearchTrendsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets search statistics for a specific period
    /// </summary>
    Task<SearchStatistics> GetSearchStatisticsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

    #endregion

    #region Search Tracking

    /// <summary>
    /// Records a search query for analytics
    /// </summary>
    Task RecordSearchQueryAsync(string searchTerm, int resultCount, TimeSpan executionTime, CancellationToken cancellationToken = default);

    /// <summary>
    /// Records search result click
    /// </summary>
    Task RecordSearchClickAsync(string searchTerm, TId resultId, int position, CancellationToken cancellationToken = default);

    /// <summary>
    /// Records search result impression
    /// </summary>
    Task RecordSearchImpressionAsync(string searchTerm, TId resultId, int position, CancellationToken cancellationToken = default);

    #endregion

    #region Analytics Configuration

    /// <summary>
    /// Updates analytics configuration
    /// </summary>
    Task UpdateAnalyticsConfigurationAsync(SearchAnalyticsConfiguration configuration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets analytics configuration
    /// </summary>
    Task<SearchAnalyticsConfiguration> GetAnalyticsConfigurationAsync(CancellationToken cancellationToken = default);

    #endregion
}