using FluentValidation;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Contracts.DTOs.Results;
using MyShop.Contracts.Services.Validation;

namespace MyShop.Application.Validators.Customers;

/// <summary>
/// Validator for CreateCustomerDto - demonstrates proper use of shared validation infrastructure
/// </summary>
public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>, IValidationRule<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        // Basic validation rules
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters")
            .Matches("^[a-zA-Z\\s]+$")
            .WithMessage("First name can only contain letters and spaces");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters")
            .Matches("^[a-zA-Z\\s]+$")
            .WithMessage("Last name can only contain letters and spaces");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.MobileNumber)
            .Matches("^[0-9+\\-\\s()]+$")
            .WithMessage("Invalid mobile number format")
            .When(x => !string.IsNullOrEmpty(x.MobileNumber));

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today)
            .WithMessage("Date of birth must be in the past")
            .GreaterThan(DateTime.Today.AddYears(-120))
            .WithMessage("Date of birth cannot be more than 120 years ago")
            .When(x => x.DateOfBirth.HasValue);

        // Custom validation rules
        RuleFor(x => x)
            .Must(HaveValidAge)
            .WithMessage("Age must be between 13 and 120 years")
            .When(x => x.DateOfBirth.HasValue);

        RuleFor(x => x)
            .Must(HaveAtLeastOneContactMethod)
            .WithMessage("Either email or mobile number must be provided");
    }

    private static bool HaveValidAge(CreateCustomerDto dto)
    {
        if (!dto.DateOfBirth.HasValue) return true;

        var age = DateTime.Today.Year - dto.DateOfBirth.Value.Year;
        return age >= 13 && age <= 120;
    }

    private static bool HaveAtLeastOneContactMethod(CreateCustomerDto dto)
    {
        return !string.IsNullOrEmpty(dto.Email) || !string.IsNullOrEmpty(dto.MobileNumber);
    }

    public async Task<ValidationResult> ValidateAsync(CreateCustomerDto item)
    {
        var fluentValidationResult = await ValidateAsync(item);
        
        var validationErrors = fluentValidationResult.Errors.Select(error => new ValidationError
        {
            ErrorMessage = error.ErrorMessage,
            PropertyName = error.PropertyName,
            AttemptedValue = error.AttemptedValue,
            Severity = error.Severity == FluentValidation.Severity.Error 
                ? ValidationSeverity.Error 
                : ValidationSeverity.Warning
        }).ToList();

        return new ValidationResult
        {
            IsValid = fluentValidationResult.IsValid,
            Errors = validationErrors
        };
    }
}

/// <summary>
/// Validator for UpdateCustomerDto
/// </summary>
public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>, IValidationRule<UpdateCustomerDto>
{
    public UpdateCustomerDtoValidator()
    {
        // Basic validation rules
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters")
            .Matches("^[a-zA-Z\\s]+$")
            .WithMessage("First name can only contain letters and spaces");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters")
            .Matches("^[a-zA-Z\\s]+$")
            .WithMessage("Last name can only contain letters and spaces");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.MobileNumber)
            .Matches("^[0-9+\\-\\s()]+$")
            .WithMessage("Invalid mobile number format")
            .When(x => !string.IsNullOrEmpty(x.MobileNumber));

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today)
            .WithMessage("Date of birth must be in the past")
            .GreaterThan(DateTime.Today.AddYears(-120))
            .WithMessage("Date of birth cannot be more than 120 years ago")
            .When(x => x.DateOfBirth.HasValue);

        // Custom validation rules
        RuleFor(x => x)
            .Must(HaveValidAge)
            .WithMessage("Age must be between 13 and 120 years")
            .When(x => x.DateOfBirth.HasValue);

        RuleFor(x => x)
            .Must(HaveAtLeastOneContactMethod)
            .WithMessage("Either email or mobile number must be provided");
    }

    private static bool HaveValidAge(UpdateCustomerDto dto)
    {
        if (!dto.DateOfBirth.HasValue) return true;

        var age = DateTime.Today.Year - dto.DateOfBirth.Value.Year;
        return age >= 13 && age <= 120;
    }

    private static bool HaveAtLeastOneContactMethod(UpdateCustomerDto dto)
    {
        return !string.IsNullOrEmpty(dto.Email) || !string.IsNullOrEmpty(dto.MobileNumber);
    }

    public async Task<ValidationResult> ValidateAsync(UpdateCustomerDto item)
    {
        var fluentValidationResult = await ValidateAsync(item);
        
        var validationErrors = fluentValidationResult.Errors.Select(error => new ValidationError
        {
            ErrorMessage = error.ErrorMessage,
            PropertyName = error.PropertyName,
            AttemptedValue = error.AttemptedValue,
            Severity = error.Severity == FluentValidation.Severity.Error 
                ? ValidationSeverity.Error 
                : ValidationSeverity.Warning
        }).ToList();

        return new ValidationResult
        {
            IsValid = fluentValidationResult.IsValid,
            Errors = validationErrors
        };
    }
}