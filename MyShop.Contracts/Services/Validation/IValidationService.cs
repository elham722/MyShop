namespace MyShop.Contracts.Services.Validation;
public interface IValidationService<T> where T : class
{
    #region Basic Validation Operations

    Task<ValidationResult> ValidateAsync(T entity, CancellationToken cancellationToken = default);

    Task<ValidationResult> ValidateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<bool> IsValidAsync(T entity, CancellationToken cancellationToken = default);

    Task<bool> IsValidAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    #endregion

    #region Advanced Validation Operations

    Task<ValidationResult> ValidateAsync(T entity, IEnumerable<IValidationRule<T>> validationRules, CancellationToken cancellationToken = default);

    Task<ValidationResult> ValidateAsync(T entity, IValidationContext validationContext, CancellationToken cancellationToken = default);

    Task<ValidationResult> ValidateAsync(T entity, ValidationOptions options, CancellationToken cancellationToken = default);

    #endregion

    #region Validation Rules Management

    void AddValidationRule(IValidationRule<T> rule);

    void RemoveValidationRule(IValidationRule<T> rule);

    IEnumerable<IValidationRule<T>> GetValidationRules();

    void ClearValidationRules();

    #endregion

    #region Validation Configuration

    Task UpdateValidationConfigurationAsync(ValidationConfiguration configuration, CancellationToken cancellationToken = default);

    Task<ValidationConfiguration> GetValidationConfigurationAsync(CancellationToken cancellationToken = default);

    #endregion
}
