namespace MyShop.Contracts.CQRS.Queries.Customer;

public record GetCustomersQuery : IQuery<PagedResult<CustomerDto>>
{
    public PaginationParams Pagination { get; init; } = PaginationParams.Default;
    public SortDtoCollection Sorting { get; init; } = new();
    public FilterDtoCollection Filtering { get; init; } = new();
    public string? Search { get; init; }
}