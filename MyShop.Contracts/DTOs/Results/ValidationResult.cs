namespace MyShop.Contracts.DTOs.Results;

public class ValidationResult
{
    public bool IsValid { get; set; }
    
    public IEnumerable<ValidationError> Errors { get; set; } = new List<ValidationError>();
    
    public IEnumerable<ValidationWarning> Warnings { get; set; } = new List<ValidationWarning>();
    
    public ValidationStatistics? Statistics { get; set; }
    
    public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    
    public DateTime ValidationTimestamp { get; set; } = DateTime.UtcNow;
}