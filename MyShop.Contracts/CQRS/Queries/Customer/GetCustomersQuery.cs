namespace MyShop.Contracts.CQRS.Queries.Customer;

public record GetCustomersQuery : IQuery<PagedResult<CustomerDto>>
{
    public PaginationParams Pagination { get; init; } = PaginationParams.Default;
    public List<SortDto> Sorting { get; init; } = new();
    public List<FilterDto> Filtering { get; init; } = new();
    public string? Search { get; init; }
}