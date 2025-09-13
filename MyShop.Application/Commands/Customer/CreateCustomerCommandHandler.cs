using MediatR;
using MyShop.Application.Common.Extensions;
using MyShop.Application.Common.Interfaces;
using MyShop.Contracts.CQRS.Commands.Common;
using MyShop.Contracts.CQRS.Commands.Customer;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Domain.Entities.Customer;
using MyShop.Domain.ValueObjects.Customer;

namespace MyShop.Application.Commands.Customer;

/// <summary>
/// Handler for CreateCustomerCommand
/// </summary>
public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var result = await HandleAsync(request, cancellationToken);
        return result.GetValueOrThrow();
    }

    public async Task<Result<CustomerDto>> HandleAsync(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate business rules
            var validationResult = await ValidateCreateCustomerAsync(command, cancellationToken);
            if (validationResult.IsFailure)
                return Result<CustomerDto>.Failure(validationResult.Errors);

            // Create customer
            var customer = new Domain.Entities.Customer.Customer(command.FirstName, command.LastName, command.CreatedBy);

            // Set optional properties
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
            var savedCustomer = await _customerRepository.AddAsync(customer, cancellationToken);

            // Return DTO
            return Result<CustomerDto>.Success(savedCustomer.ToDto());
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to create customer: {ex.Message}");
        }
    }

    private async Task<Result> ValidateCreateCustomerAsync(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        // Validate email uniqueness
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailExists = await _customerRepository.EmailExistsAsync(command.Email, cancellationToken);
            if (emailExists)
                errors.Add($"Email '{command.Email}' is already registered.");
        }

        // Validate phone uniqueness
        if (!string.IsNullOrWhiteSpace(command.MobileNumber))
        {
            var phoneExists = await _customerRepository.PhoneExistsAsync(command.MobileNumber, cancellationToken);
            if (phoneExists)
                errors.Add($"Phone number '{command.MobileNumber}' is already registered.");
        }

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }
}