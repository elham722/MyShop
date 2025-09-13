namespace MyShop.Domain.Events.Customer;
public class CustomerStatusChangedEvent : BaseDomainEvent
{
    public CustomerStatus OldStatus { get; }
    public CustomerStatus NewStatus { get; }

    public CustomerStatusChangedEvent(Guid customerId, CustomerStatus oldStatus, CustomerStatus newStatus)
        : base(customerId)
    {
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}

