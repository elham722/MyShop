namespace MyShop.Contracts.DTOs.Analytics;

public class BusinessRuleTrend
{
    public DateTime DateTime { get; set; }
    
    public long ValidationCount { get; set; }
   
    public long ViolationCount { get; set; }
    
    public double AverageResponseTimeMs { get; set; }
}