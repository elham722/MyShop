using MediatR;
using MyShop.Application.Common.Extensions;
using MyShop.Application.Common.Interfaces;
using MyShop.Contracts.CQRS.Queries.Customer;

namespace MyShop.Application.Queries.Customer;

/// <summary>
/// Handler for GetCustomerByIdQuery
/// </summary>
public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await HandleAsync(request, cancellationToken);
        return result.GetValueOrThrow();
    }

    public async Task<Result<CustomerDto>> HandleAsync(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(query.Id, cancellationToken);
            
            if (customer == null)
                return Result<CustomerDto>.Failure($"Customer with ID '{query.Id}' not found.");

            return Result<CustomerDto>.Success(customer.ToDto());
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to get customer: {ex.Message}");
        }
    }
}