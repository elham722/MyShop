namespace MyShop.Contracts.Services.Validation;
public interface IValidationContext
{
    string? CurrentUserId { get; }

    string? CurrentUserName { get; }

    Dictionary<string, object> AdditionalData { get; }

    DateTime ValidationTimestamp { get; }

    ValidationOperationType OperationType { get; }
}
