using MediatR;
using MyShop.Application.Common.Extensions;
using MyShop.Application.Common.Interfaces;
using MyShop.Contracts.CQRS.Queries.Common;
using MyShop.Contracts.CQRS.Queries.Customer;

namespace MyShop.Application.Queries.Customer;

/// <summary>
/// Handler for GetCustomersQuery
/// </summary>
public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, PagedResult<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<PagedResult<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var result = await HandleAsync(request, cancellationToken);
        return result.GetValueOrThrow();
    }

    public async Task<Result<PagedResult<CustomerDto>>> HandleAsync(GetCustomersQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var customerQuery = await _customerRepository.GetQueryableAsync(cancellationToken);

            // Apply search
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                customerQuery = customerQuery.Where(c => 
                    c.FirstName.Contains(query.Search) || 
                    c.LastName.Contains(query.Search) ||
                    (c.Email != null && c.Email.Value.Contains(query.Search)) ||
                    (c.MobileNumber != null && c.MobileNumber.Value.Contains(query.Search)));
            }

            // Apply filtering
            customerQuery = customerQuery.ApplyFiltering(query.Filtering);

            // Apply sorting
            customerQuery = customerQuery.ApplySorting(query.Sorting);

            // Apply pagination
            var result = await customerQuery.ToPagedResultAsync(query.Pagination, cancellationToken);

            // Convert to DTOs
            var customerDtos = result.Items.ToDto();
            var pagedResult = new PagedResult<CustomerDto>(customerDtos, result.TotalCount, result.PageNumber, result.PageSize);

            return Result<PagedResult<CustomerDto>>.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<CustomerDto>>.Failure($"Failed to get customers: {ex.Message}");
        }
    }
}