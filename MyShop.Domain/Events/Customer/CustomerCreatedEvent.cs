namespace MyShop.Domain.Events.Customer;
public class CustomerCreatedEvent : BaseDomainEvent
{
    public string FirstName { get; }
    public string LastName { get; }
    public string FullName { get; }

    public CustomerCreatedEvent(Guid customerId, string firstName, string lastName)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(firstName, nameof(firstName));
        Guard.AgainstNullOrEmpty(lastName, nameof(lastName));
        FirstName = firstName;
        LastName = lastName;
        FullName = $"{firstName} {lastName}".Trim();
    }
}
