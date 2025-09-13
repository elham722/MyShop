namespace MyShop.Contracts.CQRS.Commands.Customer;

/// <summary>
/// Command to delete a customer
/// </summary>
public record DeleteCustomerCommand : ICommand
{
    public Guid Id { get; init; }
    public string DeletedBy { get; init; } = string.Empty;
}