namespace MyShop.Domain.Events.Customer;
public class CustomerLinkedToApplicationUserEvent : BaseDomainEvent
{
    public string? OldApplicationUserId { get; }
    public string NewApplicationUserId { get; }

    public CustomerLinkedToApplicationUserEvent(Guid customerId, string? oldApplicationUserId, string newApplicationUserId)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(newApplicationUserId, nameof(newApplicationUserId));
        OldApplicationUserId = oldApplicationUserId;
        NewApplicationUserId = newApplicationUserId;
    }
}
