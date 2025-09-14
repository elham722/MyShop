namespace MyShop.Domain.Shared.Exceptions.Common;
public class DomainException : Exception
{
    public string? ErrorCode { get; }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public DomainException(string message, string errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    public override string ToString()
    {
        var errorCodeInfo = !string.IsNullOrEmpty(ErrorCode) ? $" (ErrorCode: {ErrorCode})" : "";
        return $"DomainException: {Message}{errorCodeInfo}";
    }
}