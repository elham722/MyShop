namespace MyShop.Contracts.DTOs.Search;

/// <summary>
/// Per-request search options - overrides global configuration
/// </summary>
public class SearchOptions
{
    // Search Behavior Overrides
    public bool? CaseSensitive { get; set; } = null; // null = use global default
    public bool? ExactMatch { get; set; } = null;
    public bool? UseWildcards { get; set; } = null;
    public bool? UseFuzzySearch { get; set; } = null;
    public double? FuzzyThreshold { get; set; } = null;
    public bool? UseFullTextSearch { get; set; } = null;
    
    // Result Limits Overrides
    public int? MaxResults { get; set; } = null;
    public int? MinSearchTermLength { get; set; } = null;
    public int? MaxSearchTermLength { get; set; } = null;
    
    // Result Processing
    public bool HighlightResults { get; set; } = false;
    public bool IncludeSearchStatistics { get; set; } = false;
    public bool IncludeRelevanceScore { get; set; } = false;
    
    // Performance Options
    public int? TimeoutMs { get; set; } = null;
    public bool UseCache { get; set; } = true;
    
    // Custom Options
    public Dictionary<string, object> CustomOptions { get; set; } = new();
}