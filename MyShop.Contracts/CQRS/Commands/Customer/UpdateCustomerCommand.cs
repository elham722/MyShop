using MyShop.Contracts.DTOs.Customer;

namespace MyShop.Contracts.CQRS.Commands.Customer;

/// <summary>
/// Command to update an existing customer
/// </summary>
public record UpdateCustomerCommand : ICommand<CustomerDto>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime? DateOfBirth { get; init; }
    public string? Email { get; init; }
    public string? MobileNumber { get; init; }
    public string UpdatedBy { get; init; } = string.Empty;
}