namespace MyShop.Domain.Events.Customer;
public class CustomerPhoneVerifiedEvent : BaseDomainEvent
{
    public string PhoneNumber { get; }

    public CustomerPhoneVerifiedEvent(Guid customerId, string phoneNumber)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(phoneNumber, nameof(phoneNumber));
        PhoneNumber = phoneNumber;
    }
}