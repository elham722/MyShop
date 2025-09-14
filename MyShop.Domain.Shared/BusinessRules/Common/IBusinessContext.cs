namespace MyShop.Domain.Shared.BusinessRules.Common;
public interface IBusinessContext
{
    string? CurrentUserId { get; }

    string? CurrentUserName { get; }

    BusinessOperationType OperationType { get; }

    Dictionary<string, object> AdditionalData { get; }

    DateTime ContextTimestamp { get; }

    string BusinessDomain { get; }
}