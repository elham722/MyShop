using MediatR;
using MyShop.Contracts.Common;
using MyShop.Contracts.CQRS.Commands.Customer;
using MyShop.Contracts.DTOs.Results;
using MyShop.Contracts.Services.BusinessRules;
using MyShop.Contracts.Services.Command;
using MyShop.Contracts.Services.Query;

namespace MyShop.Application.Commands.Customer;

/// <summary>
/// Command handler for deleting customers - demonstrates proper use of Result pattern and shared infrastructure
/// </summary>
public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
{
    private readonly ICommandService _commandService;
    private readonly IQueryService _queryService;
    private readonly IBusinessRuleService _businessRuleService;

    public DeleteCustomerCommandHandler(
        ICommandService commandService,
        IQueryService queryService,
        IBusinessRuleService businessRuleService)
    {
        _commandService = commandService;
        _queryService = queryService;
        _businessRuleService = businessRuleService;
    }

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if customer exists
            var existingCustomer = await _queryService.GetCustomerByIdAsync(request.Id);
            if (existingCustomer == null)
            {
                return Result.Failure("Customer not found");
            }

            // Apply business rules for deletion using shared business rules infrastructure
            var businessRuleResult = await _businessRuleService.ValidateAsync(existingCustomer);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // Execute command using shared command service
            await _commandService.DeleteCustomerAsync(request.Id, request.DeletedBy);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete customer: {ex.Message}");
        }
    }
}