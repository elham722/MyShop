namespace MyShop.Contracts.Services.Validation;
public interface IValidationRule<T> where T : class
{
    string RuleName { get; }

    string ErrorMessage { get; }

    ValidationSeverity Severity { get; }

    Task<bool> IsValidAsync(T entity, CancellationToken cancellationToken = default);

    Task<ValidationError?> GetValidationErrorAsync(T entity, CancellationToken cancellationToken = default);
}