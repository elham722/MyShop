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
/// Global search configuration - system-wide settings
/// </summary>
public class SearchConfiguration
{
    // System Settings
    public bool EnableSearch { get; set; } = true;
    public bool EnableSpellCheck { get; set; } = true;
    public bool EnableHighlighting { get; set; } = true;
    
    // Default Limits
    public int DefaultMaxResults { get; set; } = 1000;
    public int DefaultPageSize { get; set; } = 20;
    public int DefaultMinSearchTermLength { get; set; } = 2;
    public int DefaultMaxSearchTermLength { get; set; } = 100;
    
    // Default Search Behavior
    public bool DefaultCaseSensitive { get; set; } = false;
    public bool DefaultExactMatch { get; set; } = false;
    public bool DefaultUseWildcards { get; set; } = true;
    public bool DefaultUseFuzzySearch { get; set; } = false;
    public double DefaultFuzzyThreshold { get; set; } = 0.8;
    public bool DefaultUseFullTextSearch { get; set; } = false;
    
    // Sub-configurations
    public SearchAnalyticsConfiguration Analytics { get; set; } = new();
    public SearchSuggestionConfiguration Suggestions { get; set; } = new();
    public Dictionary<string, object> CustomSettings { get; set; } = new();
}