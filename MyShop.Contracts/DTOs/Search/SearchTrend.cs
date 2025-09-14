namespace MyShop.Contracts.DTOs.Search;

public class SearchTrend
{
    public DateTime DateTime { get; set; }
    
    public long SearchCount { get; set; }
    
    public double AverageResponseTimeMs { get; set; }
}