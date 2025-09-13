namespace MyShop.Contracts.DTOs.Results;

public class BusinessRuleViolation
{
    public string ViolationCode { get; set; } = string.Empty;
    
    public string ViolationMessage { get; set; } = string.Empty;
    
    public string RuleName { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public BusinessRuleSeverity Severity { get; set; } = BusinessRuleSeverity.Error;
    
    public string? PropertyName { get; set; }
    
    public object? Value { get; set; }
    
    public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
}