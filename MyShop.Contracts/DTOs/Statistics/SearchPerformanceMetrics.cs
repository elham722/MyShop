namespace MyShop.Contracts.DTOs.Statistics;

public class SearchPerformanceMetrics
{
    public long TotalOperations { get; set; }
    
    public long SuccessfulOperations { get; set; }
    
    public long FailedOperations { get; set; }
    
    public double AverageSearchTimeMs { get; set; }
    
    public double MinSearchTimeMs { get; set; }
    
    public double MaxSearchTimeMs { get; set; }
    
    public double SuccessRatePercentage => TotalOperations > 0 ? (double)SuccessfulOperations / TotalOperations * 100 : 0;
    
    public DateTime LastUpdateTime { get; set; } = DateTime.UtcNow;
}