namespace MyShop.Domain.Events.Customer;
public class CustomerEmailVerifiedEvent : BaseDomainEvent
{
    public string Email { get; }

    public CustomerEmailVerifiedEvent(Guid customerId, string email)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(email, nameof(email));
        Email = email;
    }
}
