namespace MyShop.Domain.Events.Customer;
public class CustomerMobileNumberUpdatedEvent : BaseDomainEvent
{
    public string? OldMobileNumber { get; }
    public string NewMobileNumber { get; }

    public CustomerMobileNumberUpdatedEvent(Guid customerId, string? oldMobileNumber, string newMobileNumber)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(newMobileNumber, nameof(newMobileNumber));
        OldMobileNumber = oldMobileNumber;
        NewMobileNumber = newMobileNumber;
    }
}
