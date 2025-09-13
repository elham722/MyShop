namespace MyShop.Contracts.DTOs.Options;

public class BusinessRuleValidationOptions
{
    public bool StopOnFirstViolation { get; set; } = false;
    
    public bool IncludeWarnings { get; set; } = true;
    
    public bool IncludeStatistics { get; set; } = false;
    
    public int MaxViolations { get; set; } = 100;
    
    public int MaxWarnings { get; set; } = 50;
    
    public int ValidationTimeoutMs { get; set; } = 10000;
    
    public bool ValidateInParallel { get; set; } = true;
    
    public IEnumerable<IBusinessRule<object>> CustomRules { get; set; } = new List<IBusinessRule<object>>();
    
    public IEnumerable<string> IncludeCategories { get; set; } = new List<string>();
    
    public IEnumerable<string> ExcludeCategories { get; set; } = new List<string>();
}