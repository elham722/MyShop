using MyShop.Domain.Shared.BusinessRules.Common;

namespace MyShop.Contracts.Services.BusinessRules;
public interface IBusinessRuleService<T> where T : class
{
    #region Basic Business Rules Operations

    Task<BusinessRuleValidationResult> ValidateBusinessRulesAsync(T entity, CancellationToken cancellationToken = default);

    Task<BusinessRuleValidationResult> ValidateBusinessRulesAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<bool> AreBusinessRulesSatisfiedAsync(T entity, CancellationToken cancellationToken = default);

    Task<bool> AreBusinessRulesSatisfiedAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    #endregion

    #region Advanced Business Rules Operations

    Task<BusinessRuleValidationResult> ValidateBusinessRulesAsync(T entity, IEnumerable<IBusinessRule<T>> businessRules, CancellationToken cancellationToken = default);

    Task<BusinessRuleValidationResult> ValidateBusinessRulesAsync(T entity, IBusinessContext businessContext, CancellationToken cancellationToken = default);

    Task<BusinessRuleValidationResult> ValidateBusinessRulesAsync(T entity, BusinessRuleValidationOptions options, CancellationToken cancellationToken = default);

    #endregion

    #region Business Rules Management

    void AddBusinessRule(IBusinessRule<T> rule);

    void RemoveBusinessRule(IBusinessRule<T> rule);

    IEnumerable<IBusinessRule<T>> GetBusinessRules();

    void ClearBusinessRules();

    IEnumerable<IBusinessRule<T>> GetBusinessRulesByCategory(string category);

    #endregion

    #region Business Rules Configuration

    Task UpdateBusinessRulesConfigurationAsync(BusinessRulesConfiguration configuration, CancellationToken cancellationToken = default);

    Task<BusinessRulesConfiguration> GetBusinessRulesConfigurationAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Business Rules Analytics

    Task<BusinessRulesAnalytics> GetBusinessRulesAnalyticsAsync(CancellationToken cancellationToken = default);

    Task<BusinessRulesPerformanceMetrics> GetBusinessRulesPerformanceMetricsAsync(CancellationToken cancellationToken = default);

    #endregion
}