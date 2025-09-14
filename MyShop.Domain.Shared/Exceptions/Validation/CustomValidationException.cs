namespace MyShop.Domain.Shared.Exceptions.Validation;
public class CustomValidationException : DomainException
{
    public new const string ErrorCode = DomainErrorCodes.ValidationFailed;

    public IReadOnlyCollection<string> Errors { get; }

    public CustomValidationException(string message) : base(message, ErrorCode)
    {
        Errors = new List<string> { message };
    }

    public CustomValidationException(IEnumerable<string> errors)
        : base($"Validation failed: {string.Join("; ", errors)}", ErrorCode)
    {
        Guard.AgainstNull(errors, nameof(errors));
        Errors = errors.ToList();
    }

    public CustomValidationException(IEnumerable<string> errors, Exception innerException)
        : base($"Validation failed: {string.Join("; ", errors)}", ErrorCode, innerException)
    {
        Guard.AgainstNull(errors, nameof(errors));
        Errors = errors.ToList();
    }

    public CustomValidationException(string message, Exception innerException) : base(message, ErrorCode, innerException)
    {
        Errors = new List<string> { message };
    }

    public override string ToString()
    {
        var errorsList = string.Join("; ", Errors);
        return $"CustomValidationException: {Message} (ErrorCode: {ErrorCode}, Errors: [{errorsList}])";
    }
}
