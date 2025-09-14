namespace MyShop.Contracts.DTOs.Search;

/// <summary>
/// Represents a search suggestion
/// </summary>
public class SearchSuggestion
{
    public string Text { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Frequency { get; set; }
    public double RelevanceScore { get; set; }
    public string? Description { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents an autocomplete suggestion
/// </summary>
public class AutocompleteSuggestion
{
    public string Text { get; set; } = string.Empty;
    public string DisplayText { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double Score { get; set; }
    public string? Icon { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}