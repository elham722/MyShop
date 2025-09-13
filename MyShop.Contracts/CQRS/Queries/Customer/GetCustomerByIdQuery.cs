namespace MyShop.Contracts.CQRS.Queries.Customer;

public record GetCustomerByIdQuery : IQuery<CustomerDto>
{
    public Guid Id { get; init; }
}