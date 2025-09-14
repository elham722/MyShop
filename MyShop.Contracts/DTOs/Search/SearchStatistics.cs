namespace MyShop.Contracts.DTOs.Search;

public class SearchStatistics
{
    public long ExecutionTimeMs { get; set; }
    
    public int FieldsSearched { get; set; }
    
    public int FiltersApplied { get; set; }
    
    public bool FuzzySearchUsed { get; set; }
    
    public bool FullTextSearchUsed { get; set; }
    
    public double RelevanceScore { get; set; }
    
    public DateTime SearchTimestamp { get; set; } = DateTime.UtcNow;
}