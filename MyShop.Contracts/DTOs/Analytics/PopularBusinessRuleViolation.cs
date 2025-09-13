namespace MyShop.Contracts.DTOs.Analytics;

public class PopularBusinessRuleViolation
{
    public string RuleName { get; set; } = string.Empty;
    
    public string ViolationCode { get; set; } = string.Empty;
    
    public long ViolationCount { get; set; }
    
    public DateTime LastViolation { get; set; }
    
    public string Category { get; set; } = string.Empty;
}