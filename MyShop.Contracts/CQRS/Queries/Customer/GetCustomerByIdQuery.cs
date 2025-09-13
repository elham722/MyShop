using MyShop.Contracts.DTOs.Customer;

namespace MyShop.Contracts.CQRS.Queries.Customer;

/// <summary>
/// Query to get a customer by ID
/// </summary>
public record GetCustomerByIdQuery : IQuery<CustomerDto>
{
    public Guid Id { get; init; }
}