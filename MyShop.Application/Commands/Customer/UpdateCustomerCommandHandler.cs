using MediatR;
using MyShop.Contracts.Common;
using MyShop.Contracts.CQRS.Commands.Customer;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Contracts.DTOs.Results;
using MyShop.Contracts.Services.BusinessRules;
using MyShop.Contracts.Services.Validation;
using MyShop.Contracts.Services.Command;
using MyShop.Contracts.Services.Query;

namespace MyShop.Application.Commands.Customer;

/// <summary>
/// Command handler for updating customers - demonstrates proper use of Result pattern and shared infrastructure
/// </summary>
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<CustomerDto>>
{
    private readonly ICommandService _commandService;
    private readonly IQueryService _queryService;
    private readonly IBusinessRuleService _businessRuleService;
    private readonly IValidationService _validationService;

    public UpdateCustomerCommandHandler(
        ICommandService commandService,
        IQueryService queryService,
        IBusinessRuleService businessRuleService,
        IValidationService validationService)
    {
        _commandService = commandService;
        _queryService = queryService;
        _businessRuleService = businessRuleService;
        _validationService = validationService;
    }

    public async Task<Result<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if customer exists
            var existingCustomer = await _queryService.GetCustomerByIdAsync(request.Id);
            if (existingCustomer == null)
            {
                return Result<CustomerDto>.Failure("Customer not found");
            }

            // Convert command to DTO for validation
            var updateDto = new UpdateCustomerDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                MobileNumber = request.MobileNumber
            };

            // Validate input using shared validation infrastructure
            var validationResult = await _validationService.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return Result<CustomerDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Apply business rules using shared business rules infrastructure
            var businessRuleResult = await _businessRuleService.ValidateAsync(updateDto);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result<CustomerDto>.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // Execute command using shared command service
            var updatedCustomer = await _commandService.UpdateCustomerAsync(request.Id, updateDto, request.UpdatedBy);
            
            return Result<CustomerDto>.Success(updatedCustomer);
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to update customer: {ex.Message}");
        }
    }
}