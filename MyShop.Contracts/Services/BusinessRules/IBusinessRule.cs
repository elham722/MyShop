namespace MyShop.Contracts.Services.BusinessRules;

public interface IBusinessRule<T> where T : class
{
    string RuleName { get; }

    string Description { get; }

    string Category { get; }

    int Priority { get; }

    bool IsEnabled { get; set; }

    Task<bool> IsBrokenAsync(T entity, CancellationToken cancellationToken = default);

    Task<BusinessRuleViolation?> GetViolationAsync(T entity, CancellationToken cancellationToken = default);
}