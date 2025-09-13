namespace MyShop.Domain.Events.Customer;
public class CustomerDateOfBirthUpdatedEvent : BaseDomainEvent
{
    public DateTime DateOfBirth { get; }

    public CustomerDateOfBirthUpdatedEvent(Guid customerId, DateTime dateOfBirth)
        : base(customerId)
    {
        DateOfBirth = dateOfBirth;
    }
}
