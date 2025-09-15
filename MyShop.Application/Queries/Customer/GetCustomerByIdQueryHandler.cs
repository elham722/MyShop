using MediatR;
using MyShop.Contracts.Common;
using MyShop.Contracts.CQRS.Queries.Customer;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Contracts.DTOs.Results;
using MyShop.Contracts.Services.Query;

namespace MyShop.Application.Queries.Customer;

/// <summary>
/// Query handler for getting customer by ID - demonstrates proper use of Result pattern and shared infrastructure
/// </summary>
public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    private readonly IQueryService _queryService;

    public GetCustomerByIdQueryHandler(IQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate input
            if (request.Id == Guid.Empty)
            {
                return Result<CustomerDto>.Failure("Customer ID cannot be empty");
            }

            // Execute query using shared query service
            var customer = await _queryService.GetCustomerByIdAsync(request.Id);
            if (customer == null)
            {
                return Result<CustomerDto>.Failure("Customer not found");
            }

            return Result<CustomerDto>.Success(customer);
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to retrieve customer: {ex.Message}");
        }
    }
}