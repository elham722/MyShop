namespace MyShop.Domain.Exceptions.Common;
public class InvalidDomainOperationException : DomainException
{
    public new const string ErrorCode = DomainErrorCodes.InvalidDomainOperation;

    public string DomainObjectType { get; }

    public string? DomainObjectId { get; }

    public string? Operation { get; }

    public InvalidDomainOperationException(string message, string domainObjectType, string? domainObjectId = null, string? operation = null)
        : base(message, ErrorCode)
    {
        Guard.AgainstNullOrEmpty(domainObjectType, nameof(domainObjectType));
        DomainObjectType = domainObjectType;
        DomainObjectId = domainObjectId;
        Operation = operation;
    }

    public InvalidDomainOperationException(string message, Exception innerException, string domainObjectType, string? domainObjectId = null, string? operation = null)
        : base(message, ErrorCode, innerException)
    {
        Guard.AgainstNullOrEmpty(domainObjectType, nameof(domainObjectType));
        DomainObjectType = domainObjectType;
        DomainObjectId = domainObjectId;
        Operation = operation;
    }

    public static InvalidDomainOperationException Create(string domainObjectType, string domainObjectId, string operation, string reason)
    {
        var message = $"Invalid operation '{operation}' on {domainObjectType} '{domainObjectId}': {reason}";
        return new InvalidDomainOperationException(message, domainObjectType, domainObjectId, operation);
    }

    public override string ToString()
    {
        return $"InvalidDomainOperationException: {Message} (ErrorCode: {ErrorCode}, DomainObjectType: {DomainObjectType}, DomainObjectId: {DomainObjectId}, Operation: {Operation})";
    }
}
