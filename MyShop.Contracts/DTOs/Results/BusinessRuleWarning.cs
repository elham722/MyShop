namespace MyShop.Contracts.DTOs.Results;

public class BusinessRuleWarning
{
    public string WarningCode { get; set; } = string.Empty;
    
    public string WarningMessage { get; set; } = string.Empty;
    
    public string RuleName { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public string? PropertyName { get; set; }
    
    public object? Value { get; set; }
    
    public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
}