namespace MyShop.Contracts.DTOs.Statistics;

public class BusinessRulesPerformanceMetrics
{
    public long TotalOperations { get; set; }
    
    public long SuccessfulOperations { get; set; }
    
    public long FailedOperations { get; set; }
    
    public double AverageValidationTimeMs { get; set; }
   
    public double MinValidationTimeMs { get; set; }
    
    public double MaxValidationTimeMs { get; set; }
    
    public double SuccessRatePercentage => TotalOperations > 0 ? (double)SuccessfulOperations / TotalOperations * 100 : 0;
    
    public DateTime LastUpdateTime { get; set; } = DateTime.UtcNow;
}