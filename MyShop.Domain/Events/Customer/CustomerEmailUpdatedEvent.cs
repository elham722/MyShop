namespace MyShop.Domain.Events.Customer;
public class CustomerEmailUpdatedEvent : BaseDomainEvent
{
    public string? OldEmail { get; }
    public string NewEmail { get; }

    public CustomerEmailUpdatedEvent(Guid customerId, string? oldEmail, string newEmail)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(newEmail, nameof(newEmail));
        OldEmail = oldEmail;
        NewEmail = newEmail;
    }
}