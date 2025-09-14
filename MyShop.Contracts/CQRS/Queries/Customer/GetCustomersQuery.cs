using MyShop.Contracts.DTOs.Common;

namespace MyShop.Contracts.CQRS.Queries.Customer;

public record GetCustomersQuery : IQuery<PagedResult<CustomerDto>>
{
    public QueryOptionsDto Options { get; init; } = new();
}