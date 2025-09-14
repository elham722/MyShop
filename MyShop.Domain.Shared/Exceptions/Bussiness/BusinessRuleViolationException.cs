namespace MyShop.Domain.Shared.Exceptions.Bussiness;
public class BusinessRuleViolationException : DomainException
{
    public new const string ErrorCode = DomainErrorCodes.BusinessRuleViolation;

    public BusinessRuleViolationException(string message) : base(message, ErrorCode)
    {
    }

    public BusinessRuleViolationException(string message, Exception innerException) : base(message, ErrorCode, innerException)
    {
    }

    public override string ToString()
    {
        return $"BusinessRuleViolationException: {Message} (ErrorCode: {ErrorCode})";
    }
}
