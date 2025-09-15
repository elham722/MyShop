using MediatR;
using MyShop.Contracts.Common;
using MyShop.Contracts.Common.Pagination;
using MyShop.Contracts.CQRS.Queries.Customer;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Contracts.DTOs.Results;
using MyShop.Contracts.Services.Query;
using MyShop.Contracts.Services.Validation;
using MyShop.Contracts.Services.BusinessRules;

namespace MyShop.Application.Queries.Customer;

/// <summary>
/// Query handler for getting customers - demonstrates proper use of Result pattern and shared infrastructure
/// </summary>
public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<PagedResult<CustomerDto>>>
{
    private readonly IQueryService _queryService;
    private readonly IValidationService _validationService;
    private readonly IBusinessRuleService _businessRuleService;

    public GetCustomersQueryHandler(
        IQueryService queryService,
        IValidationService validationService,
        IBusinessRuleService businessRuleService)
    {
        _queryService = queryService;
        _validationService = validationService;
        _businessRuleService = businessRuleService;
    }

    public async Task<Result<PagedResult<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate query options using shared validation infrastructure
            var validationResult = await _validationService.ValidateAsync(request.Options);
            if (!validationResult.IsValid)
            {
                return Result<PagedResult<CustomerDto>>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Apply business rules using shared business rules infrastructure
            var businessRuleResult = await _businessRuleService.ValidateAsync(request.Options);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result<PagedResult<CustomerDto>>.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // Execute query using shared query service
            var customers = await _queryService.GetCustomersAsync(request.Options);
            
            return Result<PagedResult<CustomerDto>>.Success(customers);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<CustomerDto>>.Failure($"Failed to retrieve customers: {ex.Message}");
        }
    }
}