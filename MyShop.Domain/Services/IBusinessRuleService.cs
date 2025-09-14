using MyShop.Domain.Shared.BusinessRules.Common;

namespace MyShop.Domain.Services;
public interface IBusinessRuleService
{
    void Validate(params IBusinessRule[] businessRules);
    void Validate(IEnumerable<IBusinessRule> businessRules);
    Task ValidateAsync(params IBusinessRule[] businessRules);
    Task ValidateAsync(IEnumerable<IBusinessRule> businessRules);
    bool AreValid(params IBusinessRule[] businessRules);
    bool AreValid(IEnumerable<IBusinessRule> businessRules);
    IEnumerable<string> GetBrokenRuleMessages(params IBusinessRule[] businessRules);
    IEnumerable<string> GetBrokenRuleMessages(IEnumerable<IBusinessRule> businessRules);
}
