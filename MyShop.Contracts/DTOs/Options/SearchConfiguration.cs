namespace MyShop.Contracts.DTOs.Options;

public class SearchConfiguration
{
    public string[] DefaultSearchFields { get; set; } = Array.Empty<string>();
    
    public int MinSearchTermLength { get; set; } = 2;
    
    public int MaxSearchTermLength { get; set; } = 100;
    
    public int DefaultPageSize { get; set; } = 20;
    
    public int MaxPageSize { get; set; } = 100;
    
    public bool EnableFuzzySearch { get; set; } = true;
    
    public bool EnableFullTextSearch { get; set; } = true;
    
    public bool EnableSearchSuggestions { get; set; } = true;
    
    public bool EnableSearchAnalytics { get; set; } = true;
    
    public int SearchTimeoutMs { get; set; } = 5000;
    
    public int MaxResults { get; set; } = 1000;
}