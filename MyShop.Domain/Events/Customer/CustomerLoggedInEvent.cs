namespace MyShop.Domain.Events.Customer;
public class CustomerLoggedInEvent : BaseDomainEvent
{
    public DateTime LoginTime { get; }

    public CustomerLoggedInEvent(Guid customerId, DateTime loginTime)
        : base(customerId)
    {
        LoginTime = loginTime;
    }
}
