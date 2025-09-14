namespace MyShop.Contracts.DTOs.Customer;
public class GetCustomersQueryParams
{
    public PaginationParams Pagination { get; set; } = PaginationParams.Default;
    public SortDtoCollection Sorting { get; set; } = new();
    public List<FilterDto> Filtering { get; set; } = new();
    public string? Search { get; set; }
}