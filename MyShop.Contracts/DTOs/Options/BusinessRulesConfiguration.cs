namespace MyShop.Contracts.DTOs.Options;

public class BusinessRulesConfiguration
{
    public BusinessRuleValidationOptions DefaultOptions { get; set; } = new BusinessRuleValidationOptions();
    
    public bool EnableBusinessRulesCaching { get; set; } = true;
    
    public TimeSpan BusinessRulesCacheExpiration { get; set; } = TimeSpan.FromMinutes(30);
   
    public bool EnableBusinessRulesLogging { get; set; } = true;
    
    public bool EnableBusinessRulesMetrics { get; set; } = true;
    
    public IEnumerable<IBusinessRule<object>> GlobalRules { get; set; } = new List<IBusinessRule<object>>();
    
    public int ValidationTimeoutMs { get; set; } = 10000;
    
    public int MaxConcurrentValidations { get; set; } = 10;
    
    public Dictionary<string, BusinessRuleCategoryConfiguration> CategoryConfigurations { get; set; } = new Dictionary<string, BusinessRuleCategoryConfiguration>();
}