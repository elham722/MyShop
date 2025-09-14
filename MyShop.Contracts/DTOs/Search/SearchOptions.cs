namespace MyShop.Contracts.DTOs.Search;

public class SearchOptions
{
    public bool CaseSensitive { get; set; } = false;

    public bool ExactMatch { get; set; } = false;
    
    public bool UseWildcards { get; set; } = true;
    
    public int MinSearchTermLength { get; set; } = 2;
    
    public int MaxSearchTermLength { get; set; } = 100;
    
    public bool UseFuzzySearch { get; set; } = false;
    
    public double FuzzyThreshold { get; set; } = 0.8;
    
    public bool UseFullTextSearch { get; set; } = false;
    
    public bool HighlightResults { get; set; } = false;
    
    public int MaxResults { get; set; } = 1000;
    
    public bool IncludeSearchStatistics { get; set; } = false;
}