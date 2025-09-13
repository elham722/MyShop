namespace MyShop.Contracts.DTOs.Options;

public class ValidationConfiguration
{
    public ValidationOptions DefaultOptions { get; set; } = new ValidationOptions();
    
    public bool EnableValidationCaching { get; set; } = true;
    
    public TimeSpan ValidationCacheExpiration { get; set; } = TimeSpan.FromMinutes(30);
    
    public bool EnableValidationLogging { get; set; } = true;
    
    public bool EnableValidationMetrics { get; set; } = true;
    
    public IEnumerable<IValidationRule<object>> GlobalRules { get; set; } = new List<IValidationRule<object>>();
    
    public int ValidationTimeoutMs { get; set; } = 5000;
    
    public int MaxConcurrentValidations { get; set; } = 10;
}