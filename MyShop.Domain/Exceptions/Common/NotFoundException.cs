namespace MyShop.Domain.Exceptions.Common;
public class NotFoundException : DomainException
{
    public new const string ErrorCode = DomainErrorCodes.EntityNotFound;

    public NotFoundException(string name, object key)
        : base($"{name} with key {key} was not found.", ErrorCode)
    {
    }

    public NotFoundException(string message) : base(message, ErrorCode)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, ErrorCode, innerException)
    {
    }

    public override string ToString()
    {
        return $"NotFoundException: {Message} (ErrorCode: {ErrorCode})";
    }
}