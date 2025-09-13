namespace MyShop.Contracts.DTOs.Analytics;

public class BusinessRulesAnalytics
{
    public long TotalValidations { get; set; }
    
    public long ValidationsToday { get; set; }
    
    public long ValidationsThisWeek { get; set; }
    
    public long ValidationsThisMonth { get; set; }
    
    public double AverageResponseTimeMs { get; set; }
    
    public double AverageViolationsPerValidation { get; set; }
    
    public double SuccessRatePercentage { get; set; }
    
    public IEnumerable<PopularBusinessRuleViolation> PopularViolations { get; set; } = new List<PopularBusinessRuleViolation>();
  
    public IEnumerable<BusinessRuleTrend> Trends { get; set; } = new List<BusinessRuleTrend>();
}