namespace MyShop.Contracts.DTOs.Results;

public class BulkOperationResult
{
    public int AffectedCount { get; set; }
    
    public int ProcessedCount { get; set; }
    
    public int FailedCount { get; set; }
    
    public List<string> Errors { get; set; } = new();
    
    public List<ValidationError> ValidationErrors { get; set; } = new();
    
    public List<BusinessRuleViolation> BusinessRuleViolations { get; set; } = new();
    
    public bool IsSuccess => FailedCount == 0;
    
    public TimeSpan Duration { get; set; }
    
    public double SuccessRate => ProcessedCount > 0 ? (double)(ProcessedCount - FailedCount) / ProcessedCount * 100 : 0;
}