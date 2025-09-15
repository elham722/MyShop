using MediatR;
using MyShop.Contracts.Common;
using MyShop.Contracts.CQRS.Queries.Customer;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Contracts.DTOs.Results;
using MyShop.Contracts.Services.Query;

namespace MyShop.Application.Queries.Customer;

/// <summary>
/// Query handler for getting customer statistics - demonstrates proper use of Result pattern and shared infrastructure
/// </summary>
public class GetCustomerStatisticsQueryHandler : IRequestHandler<GetCustomerStatisticsQuery, Result<CustomerStatisticsDto>>
{
    private readonly IQueryService _queryService;

    public GetCustomerStatisticsQueryHandler(IQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<Result<CustomerStatisticsDto>> Handle(GetCustomerStatisticsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Execute query using shared query service
            var statistics = await _queryService.GetCustomerStatisticsAsync();
            
            return Result<CustomerStatisticsDto>.Success(statistics);
        }
        catch (Exception ex)
        {
            return Result<CustomerStatisticsDto>.Failure($"Failed to retrieve customer statistics: {ex.Message}");
        }
    }
}