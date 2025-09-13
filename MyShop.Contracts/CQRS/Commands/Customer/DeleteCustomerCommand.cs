namespace MyShop.Contracts.CQRS.Commands.Customer;

public record DeleteCustomerCommand : ICommand
{
    public Guid Id { get; init; }
    public string DeletedBy { get; init; } = string.Empty;
}