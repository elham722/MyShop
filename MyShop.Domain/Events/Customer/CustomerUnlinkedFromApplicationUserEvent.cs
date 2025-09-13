namespace MyShop.Domain.Events.Customer;
public class CustomerUnlinkedFromApplicationUserEvent : BaseDomainEvent
{
    public string ApplicationUserId { get; }

    public CustomerUnlinkedFromApplicationUserEvent(Guid customerId, string applicationUserId)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(applicationUserId, nameof(applicationUserId));
        ApplicationUserId = applicationUserId;
    }
}

