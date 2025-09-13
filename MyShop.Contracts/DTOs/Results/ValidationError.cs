namespace MyShop.Contracts.DTOs.Results;

public class ValidationError
{
    public string ErrorCode { get; set; } = string.Empty;
    
    public string ErrorMessage { get; set; } = string.Empty;
    
    public string? PropertyName { get; set; }
    
    public object? AttemptedValue { get; set; }
    
    public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
    
    public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
}