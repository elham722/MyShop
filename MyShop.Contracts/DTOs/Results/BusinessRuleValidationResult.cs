namespace MyShop.Contracts.DTOs.Results;

public class BusinessRuleValidationResult
{
    public bool AreRulesSatisfied { get; set; }
    
    public IEnumerable<BusinessRuleViolation> Violations { get; set; } = new List<BusinessRuleViolation>();
    
    public IEnumerable<BusinessRuleWarning> Warnings { get; set; } = new List<BusinessRuleWarning>();
    
    public BusinessRuleValidationStatistics? Statistics { get; set; }
    
    public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    
    public DateTime ValidationTimestamp { get; set; } = DateTime.UtcNow;
}