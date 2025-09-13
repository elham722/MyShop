namespace MyShop.Domain.Events.Customer;
public class CustomerAddressUpdatedEvent : BaseDomainEvent
{
    public string? OldAddress { get; }
    public string NewAddress { get; }

    public CustomerAddressUpdatedEvent(Guid customerId, string? oldAddress, string newAddress)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(newAddress, nameof(newAddress));
        OldAddress = oldAddress;
        NewAddress = newAddress;
    }
}
