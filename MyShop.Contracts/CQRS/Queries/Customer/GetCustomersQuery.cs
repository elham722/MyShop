using MyShop.Contracts.DTOs.Customer;

namespace MyShop.Contracts.CQRS.Queries.Customer;

/// <summary>
/// Query to get a paginated list of customers
/// </summary>
public record GetCustomersQuery : IQuery<PagedResult<CustomerDto>>
{
    public PaginationParams Pagination { get; init; } = PaginationParams.Default;
    public SortDtoCollection Sorting { get; init; } = new();
    public FilterDtoCollection Filtering { get; init; } = new();
    public string? Search { get; init; }
}