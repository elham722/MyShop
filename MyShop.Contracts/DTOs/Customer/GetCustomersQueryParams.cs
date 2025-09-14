using MyShop.Contracts.DTOs.Common;

namespace MyShop.Contracts.DTOs.Customer;

public class GetCustomersQueryParams
{
    public QueryOptionsDto Options { get; set; } = new();
}