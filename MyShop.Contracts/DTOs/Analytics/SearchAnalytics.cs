namespace MyShop.Contracts.DTOs.Analytics;

public class SearchAnalytics
{
    public long TotalSearches { get; set; }
    
    public long SearchesToday { get; set; }
    
    public long SearchesThisWeek { get; set; }
    
    public long SearchesThisMonth { get; set; }
   
    public double AverageResponseTimeMs { get; set; }
    
    public double AverageResultsPerSearch { get; set; }
    
    public double SuccessRatePercentage { get; set; }
    
    public IEnumerable<PopularSearchTerm> PopularTerms { get; set; } = new List<PopularSearchTerm>();
    
    public IEnumerable<SearchTrend> Trends { get; set; } = new List<SearchTrend>();
}