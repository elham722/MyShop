namespace MyShop.Contracts.DTOs.Statistics;

public class ValidationStatistics
{
    public int TotalRulesApplied { get; set; }
    
    public int RulesPassed { get; set; }
    
    public int RulesFailed { get; set; }
    
    public int WarningsGenerated { get; set; }
    
    public long ExecutionTimeMs { get; set; }
    
    public double SuccessRatePercentage => TotalRulesApplied > 0 ? (double)RulesPassed / TotalRulesApplied * 100 : 0;
}