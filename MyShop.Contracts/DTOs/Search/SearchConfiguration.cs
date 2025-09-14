namespace MyShop.Contracts.DTOs.Search;

/// <summary>
/// Search analytics configuration
/// </summary>
public class SearchAnalyticsConfiguration
{
    public bool EnableTracking { get; set; } = true;
    public bool TrackClicks { get; set; } = true;
    public bool TrackImpressions { get; set; } = true;
    public int RetentionDays { get; set; } = 90;
    public bool EnableRealTimeAnalytics { get; set; } = false;
    public Dictionary<string, object> CustomSettings { get; set; } = new();
}

/// <summary>
/// Search suggestion configuration
/// </summary>
public class SearchSuggestionConfiguration
{
    public bool EnableSuggestions { get; set; } = true;
    public int MaxSuggestions { get; set; } = 10;
    public bool EnableAutocomplete { get; set; } = true;
    public bool EnableTrendingSuggestions { get; set; } = true;
    public bool EnableCustomSuggestions { get; set; } = true;
    public int MinQueryLength { get; set; } = 2;
    public Dictionary<string, object> CustomSettings { get; set; } = new();
}

/// <summary>
/// Search configuration
/// </summary>
public class SearchConfiguration
{
    public bool EnableSearch { get; set; } = true;
    public int MaxResults { get; set; } = 1000;
    public int DefaultPageSize { get; set; } = 20;
    public bool EnableFuzzySearch { get; set; } = true;
    public double FuzzyThreshold { get; set; } = 0.7;
    public bool EnableHighlighting { get; set; } = true;
    public bool EnableSpellCheck { get; set; } = true;
    public SearchAnalyticsConfiguration Analytics { get; set; } = new();
    public SearchSuggestionConfiguration Suggestions { get; set; } = new();
    public Dictionary<string, object> CustomSettings { get; set; } = new();
}