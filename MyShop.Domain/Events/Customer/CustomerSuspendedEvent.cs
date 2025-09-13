namespace MyShop.Domain.Events.Customer;
public class CustomerSuspendedEvent : BaseDomainEvent
{
    public string Reason { get; }
    public CustomerStatus PreviousStatus { get; }

    public CustomerSuspendedEvent(Guid customerId, string reason, CustomerStatus previousStatus)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(reason, nameof(reason));
        Reason = reason;
        PreviousStatus = previousStatus;
    }
}