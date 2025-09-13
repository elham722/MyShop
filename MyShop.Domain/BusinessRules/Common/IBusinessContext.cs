namespace MyShop.Domain.BusinessRules.Common;
public interface IBusinessContext
{
    string? CurrentUserId { get; }

    string? CurrentUserName { get; }

    BusinessOperationType OperationType { get; }

    Dictionary<string, object> AdditionalData { get; }

    DateTime ContextTimestamp { get; }

    string BusinessDomain { get; }
}