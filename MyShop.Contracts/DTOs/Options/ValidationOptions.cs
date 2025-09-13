namespace MyShop.Contracts.DTOs.Options;

public class ValidationOptions
{
    public bool StopOnFirstError { get; set; } = false;
    
    public bool IncludeWarnings { get; set; } = true;
    
    public bool IncludeStatistics { get; set; } = false;
    
    public int MaxErrors { get; set; } = 100;
    
    public int MaxWarnings { get; set; } = 50;
    
    public int ValidationTimeoutMs { get; set; } = 5000;
    
    public bool ValidateInParallel { get; set; } = true;
    
    public IEnumerable<IValidationRule<object>> CustomRules { get; set; } = new List<IValidationRule<object>>();
}