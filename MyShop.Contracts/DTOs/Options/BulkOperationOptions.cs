namespace MyShop.Contracts.DTOs.Options;

public class BulkOperationOptions
{
    public int BatchSize { get; set; } = 1000;
    
    public bool IgnoreDuplicates { get; set; } = false;
  
    public bool UpdateOnlyChangedFields { get; set; } = true;
   
    public bool ValidateBeforeOperation { get; set; } = true;
    
    public bool ValidateBusinessRulesBeforeOperation { get; set; } = true;
    
    public ValidationOptions? ValidationOptions { get; set; }
    
    public BusinessRuleValidationOptions? BusinessRuleValidationOptions { get; set; }
    
    public bool PublishDomainEvents { get; set; } = true;
    
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
}