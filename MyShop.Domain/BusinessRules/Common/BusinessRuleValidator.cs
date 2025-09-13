namespace MyShop.Domain.BusinessRules.Common;
  public static class BusinessRuleValidator
{
    public static void Validate(params IBusinessRule[] businessRules)
    {
        var brokenRules = businessRules.Where(rule => rule.IsBroken()).ToList();

        if (brokenRules.Any())
        {
            var messages = brokenRules.Select(rule => rule.Message);
            throw new BusinessRuleViolationException(string.Join("; ", messages));
        }
    }

    public static void Validate(IEnumerable<IBusinessRule> businessRules)
    {
        Validate(businessRules.ToArray());
    }

    public static async Task ValidateAsync(params IBusinessRule[] businessRules)
    {
        var brokenRulesTasks = businessRules.Select(async rule => new { Rule = rule, IsBroken = await rule.IsBrokenAsync() });
        var brokenRulesResults = await Task.WhenAll(brokenRulesTasks);
        var brokenRules = brokenRulesResults.Where(result => result.IsBroken).Select(result => result.Rule).ToList();

        if (brokenRules.Any())
        {
            var messages = brokenRules.Select(rule => rule.Message);
            throw new BusinessRuleViolationException(string.Join("; ", messages));
        }
    }

    public static async Task ValidateAsync(IEnumerable<IBusinessRule> businessRules)
    {
        await ValidateAsync(businessRules.ToArray());
    }

    public static bool AreValid(params IBusinessRule[] businessRules)
    {
        return businessRules.All(rule => !rule.IsBroken());
    }

    public static bool AreValid(IEnumerable<IBusinessRule> businessRules)
    {
        return AreValid(businessRules.ToArray());
    }

    public static async Task<bool> AreValidAsync(params IBusinessRule[] businessRules)
    {
        var results = await Task.WhenAll(businessRules.Select(rule => rule.IsBrokenAsync()));
        return results.All(isBroken => !isBroken);
    }

    public static async Task<bool> AreValidAsync(IEnumerable<IBusinessRule> businessRules)
    {
        return await AreValidAsync(businessRules.ToArray());
    }

    public static IEnumerable<string> GetBrokenRuleMessages(params IBusinessRule[] businessRules)
    {
        return businessRules.Where(rule => rule.IsBroken()).Select(rule => rule.Message);
    }

    public static IEnumerable<string> GetBrokenRuleMessages(IEnumerable<IBusinessRule> businessRules)
    {
        return GetBrokenRuleMessages(businessRules.ToArray());
    }

    public static async Task<IEnumerable<string>> GetBrokenRuleMessagesAsync(params IBusinessRule[] businessRules)
    {
        var brokenRulesTasks = businessRules.Select(async rule => new { Rule = rule, IsBroken = await rule.IsBrokenAsync() });
        var brokenRulesResults = await Task.WhenAll(brokenRulesTasks);
        return brokenRulesResults.Where(result => result.IsBroken).Select(result => result.Rule.Message);
    }

    public static async Task<IEnumerable<string>> GetBrokenRuleMessagesAsync(IEnumerable<IBusinessRule> businessRules)
    {
        return await GetBrokenRuleMessagesAsync(businessRules.ToArray());
    }
}
