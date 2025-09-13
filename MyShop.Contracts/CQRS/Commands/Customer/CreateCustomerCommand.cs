namespace MyShop.Contracts.CQRS.Commands.Customer;

public record CreateCustomerCommand : ICommand<CustomerDto>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime? DateOfBirth { get; init; }
    public string? Email { get; init; }
    public string? MobileNumber { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
}