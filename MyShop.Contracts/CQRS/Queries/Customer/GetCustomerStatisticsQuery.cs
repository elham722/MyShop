using MyShop.Contracts.DTOs.Customer;

namespace MyShop.Contracts.CQRS.Queries.Customer;

public record GetCustomerStatisticsQuery : IQuery<CustomerStatisticsDto>;