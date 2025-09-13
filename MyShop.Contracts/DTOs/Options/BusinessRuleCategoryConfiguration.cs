namespace MyShop.Contracts.DTOs.Options;

public class BusinessRuleCategoryConfiguration
{
    public string CategoryName { get; set; } = string.Empty;
    
    public bool IsEnabled { get; set; } = true;
    
    public int Priority { get; set; } = 0;
    
    public string Description { get; set; } = string.Empty;
    
    public BusinessRuleValidationOptions ValidationOptions { get; set; } = new BusinessRuleValidationOptions();
}