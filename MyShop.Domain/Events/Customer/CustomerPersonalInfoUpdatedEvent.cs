namespace MyShop.Domain.Events.Customer;
public class CustomerPersonalInfoUpdatedEvent : BaseDomainEvent
{
    public string OldFirstName { get; }
    public string OldLastName { get; }
    public string NewFirstName { get; }
    public string NewLastName { get; }
    public string NewFullName { get; }

    public CustomerPersonalInfoUpdatedEvent(Guid customerId, string oldFirstName, string oldLastName, string newFirstName, string newLastName)
        : base(customerId)
    {
        Guard.AgainstNullOrEmpty(oldFirstName, nameof(oldFirstName));
        Guard.AgainstNullOrEmpty(oldLastName, nameof(oldLastName));
        Guard.AgainstNullOrEmpty(newFirstName, nameof(newFirstName));
        Guard.AgainstNullOrEmpty(newLastName, nameof(newLastName));
        OldFirstName = oldFirstName;
        OldLastName = oldLastName;
        NewFirstName = newFirstName;
        NewLastName = newLastName;
        NewFullName = $"{newFirstName} {newLastName}".Trim();
    }
}