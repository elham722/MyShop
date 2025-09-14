using MyShop.Contracts.Common.Pagination;

namespace MyShop.Contracts.DTOs.Search;

/// <summary>
/// Search result that extends PagedResult with search-specific functionality
/// </summary>
public class SearchResult<T> : PagedResult<T>
{
    #region Search-Specific Properties

    /// <summary>
    /// Search execution statistics
    /// </summary>
    public SearchStatistics? Statistics { get; set; }

    /// <summary>
    /// Search suggestions for user input
    /// </summary>
    public IEnumerable<string> Suggestions { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Highlighted text fragments for search results
    /// </summary>
    public Dictionary<string, string> Highlights { get; set; } = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a search result from paged data
    /// </summary>
    public SearchResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize) 
        : base(items, totalCount, pageNumber, pageSize)
    {
    }

    /// <summary>
    /// Creates a search result from PagedResult
    /// </summary>
    public SearchResult(PagedResult<T> pagedResult) 
        : base(pagedResult.Items, pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize)
    {
    }

    /// <summary>
    /// Creates an empty search result
    /// </summary>
    public static new SearchResult<T> Empty(int pageNumber = 1, int pageSize = 20)
    {
        return new SearchResult<T>(Array.Empty<T>(), 0, pageNumber, pageSize);
    }

    /// <summary>
    /// Creates a search result from pagination parameters
    /// </summary>
    public static new SearchResult<T> Create(IEnumerable<T> items, int totalCount, PaginationParams pagination)
    {
        return new SearchResult<T>(items, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    /// <summary>
    /// Creates a search result with single page
    /// </summary>
    public static new SearchResult<T> SinglePage(IEnumerable<T> items)
    {
        var itemsList = items.ToList();
        return new SearchResult<T>(itemsList, itemsList.Count, 1, itemsList.Count);
    }

    #endregion

    #region Search-Specific Methods

    /// <summary>
    /// Adds search suggestions
    /// </summary>
    public SearchResult<T> WithSuggestions(params string[] suggestions)
    {
        Suggestions = suggestions;
        return this;
    }

    /// <summary>
    /// Adds search suggestions
    /// </summary>
    public SearchResult<T> WithSuggestions(IEnumerable<string> suggestions)
    {
        Suggestions = suggestions.ToList();
        return this;
    }

    /// <summary>
    /// Adds highlighted text
    /// </summary>
    public SearchResult<T> WithHighlight(string field, string highlightedText)
    {
        Highlights[field] = highlightedText;
        return this;
    }

    /// <summary>
    /// Adds multiple highlights
    /// </summary>
    public SearchResult<T> WithHighlights(Dictionary<string, string> highlights)
    {
        foreach (var highlight in highlights)
        {
            Highlights[highlight.Key] = highlight.Value;
        }
        return this;
    }

    /// <summary>
    /// Sets search statistics
    /// </summary>
    public SearchResult<T> WithStatistics(SearchStatistics statistics)
    {
        Statistics = statistics;
        return this;
    }

    /// <summary>
    /// Converts to PagedResult (removes search-specific data)
    /// </summary>
    public PagedResult<T> ToPagedResult()
    {
        return new PagedResult<T>(Items, TotalCount, PageNumber, PageSize);
    }

    /// <summary>
    /// Gets search summary including pagination info
    /// </summary>
    public string GetSearchSummary()
    {
        var baseSummary = GetSummary();
        var suggestionsCount = Suggestions.Count();
        var highlightsCount = Highlights.Count;
        
        var searchInfo = new List<string>();
        if (suggestionsCount > 0) searchInfo.Add($"{suggestionsCount} suggestions");
        if (highlightsCount > 0) searchInfo.Add($"{highlightsCount} highlights");
        if (Statistics != null) searchInfo.Add($"executed in {Statistics.ExecutionTimeMs}ms");
        
        var searchDetails = searchInfo.Any() ? $" ({string.Join(", ", searchInfo)})" : "";
        return $"{baseSummary}{searchDetails}";
    }

    #endregion
}

/// <summary>
/// Extension methods for SearchResult
/// </summary>
public static class SearchResultExtensions
{
    /// <summary>
    /// Converts PagedResult to SearchResult
    /// </summary>
    public static SearchResult<T> ToSearchResult<T>(this PagedResult<T> pagedResult)
    {
        return new SearchResult<T>(pagedResult);
    }

    /// <summary>
    /// Converts PagedResult to SearchResult with search-specific data
    /// </summary>
    public static SearchResult<T> ToSearchResult<T>(
        this PagedResult<T> pagedResult, 
        SearchStatistics? statistics = null,
        IEnumerable<string>? suggestions = null,
        Dictionary<string, string>? highlights = null)
    {
        var result = new SearchResult<T>(pagedResult);
        
        if (statistics != null)
            result = result.WithStatistics(statistics);
            
        result = result.WithSuggestions(suggestions ?? Array.Empty<string>())
                      .WithHighlights(highlights ?? new Dictionary<string, string>());
                      
        return result;
    }
}