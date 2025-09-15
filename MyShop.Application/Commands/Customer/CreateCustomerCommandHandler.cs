using MediatR;
using MyShop.Contracts.Common;
using MyShop.Contracts.CQRS.Commands.Customer;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Contracts.DTOs.Results;
using MyShop.Contracts.Services.BusinessRules;
using MyShop.Contracts.Services.Validation;
using MyShop.Contracts.Services.Command;

namespace MyShop.Application.Commands.Customer;

/// <summary>
/// Command handler for creating customers - demonstrates proper use of Result pattern and shared infrastructure
/// </summary>
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    private readonly ICommandService _commandService;
    private readonly IBusinessRuleService _businessRuleService;
    private readonly IValidationService _validationService;

    public CreateCustomerCommandHandler(
        ICommandService commandService,
        IBusinessRuleService businessRuleService,
        IValidationService validationService)
    {
        _commandService = commandService;
        _businessRuleService = businessRuleService;
        _validationService = validationService;
    }

    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Convert command to DTO for validation
            var createDto = new CreateCustomerDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                MobileNumber = request.MobileNumber
            };

            // Validate input using shared validation infrastructure
            var validationResult = await _validationService.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return Result<CustomerDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Apply business rules using shared business rules infrastructure
            var businessRuleResult = await _businessRuleService.ValidateAsync(createDto);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result<CustomerDto>.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // Execute command using shared command service
            var customer = await _commandService.CreateCustomerAsync(createDto, request.CreatedBy);
            
            return Result<CustomerDto>.Success(customer);
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to create customer: {ex.Message}");
        }
    }
}