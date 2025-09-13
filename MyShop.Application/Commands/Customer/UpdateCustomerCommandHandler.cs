using MediatR;
using MyShop.Application.Common.Extensions;
using MyShop.Application.Common.Interfaces;
using MyShop.Contracts.CQRS.Commands.Customer;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Domain.ValueObjects.Customer;

namespace MyShop.Application.Commands.Customer;

/// <summary>
/// Handler for UpdateCustomerCommand
/// </summary>
public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var result = await HandleAsync(request, cancellationToken);
        return result.GetValueOrThrow();
    }

    public async Task<Result<CustomerDto>> HandleAsync(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get existing customer
            var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
            if (customer == null)
                return Result<CustomerDto>.Failure($"Customer with ID '{command.Id}' not found.");

            // Validate business rules
            var validationResult = await ValidateUpdateCustomerAsync(command, customer, cancellationToken);
            if (validationResult.IsFailure)
                return Result<CustomerDto>.Failure(validationResult.Errors);

            // Update customer
            customer.UpdatePersonalInfo(command.FirstName, command.LastName);

            if (command.DateOfBirth.HasValue)
                customer.SetDateOfBirth(command.DateOfBirth.Value);

            if (!string.IsNullOrWhiteSpace(command.Email))
            {
                var email = new Email(command.Email);
                customer.SetEmail(email);
            }

            if (!string.IsNullOrWhiteSpace(command.MobileNumber))
            {
                var phoneNumber = new PhoneNumber(command.MobileNumber);
                customer.SetMobileNumber(phoneNumber);
            }

            // Validate business rules
            await customer.ValidateBusinessRulesAsync();

            // Save customer
            var updatedCustomer = await _customerRepository.UpdateAsync(customer, cancellationToken);

            // Return DTO
            return Result<CustomerDto>.Success(updatedCustomer.ToDto());
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to update customer: {ex.Message}");
        }
    }

    private async Task<Result> ValidateUpdateCustomerAsync(UpdateCustomerCommand command, Domain.Entities.Customer.Customer existingCustomer, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        // Validate email uniqueness (excluding current customer)
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailExists = await _customerRepository.EmailExistsAsync(command.Email, cancellationToken);
            if (emailExists && existingCustomer.Email?.Value != command.Email)
                errors.Add($"Email '{command.Email}' is already registered.");
        }

        // Validate phone uniqueness (excluding current customer)
        if (!string.IsNullOrWhiteSpace(command.MobileNumber))
        {
            var phoneExists = await _customerRepository.PhoneExistsAsync(command.MobileNumber, cancellationToken);
            if (phoneExists && existingCustomer.MobileNumber?.Value != command.MobileNumber)
                errors.Add($"Phone number '{command.MobileNumber}' is already registered.");
        }

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }
}