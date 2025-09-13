using MediatR;
using MyShop.Application.Common.Interfaces;
using MyShop.Contracts.CQRS.Commands.Common;
using MyShop.Contracts.CQRS.Commands.Customer;

namespace MyShop.Application.Commands.Customer;

/// <summary>
/// Handler for DeleteCustomerCommand
/// </summary>
public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var result = await HandleAsync(request, cancellationToken);
        if (result.IsFailure)
            throw new InvalidOperationException(string.Join(", ", result.Errors));
        return Unit.Value;
    }

    public async Task<Result> HandleAsync(DeleteCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if customer exists
            var exists = await _customerRepository.ExistsAsync(command.Id, cancellationToken);
            if (!exists)
                return Result.Failure($"Customer with ID '{command.Id}' not found.");

            // Delete customer
            await _customerRepository.DeleteAsync(command.Id, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete customer: {ex.Message}");
        }
    }
}