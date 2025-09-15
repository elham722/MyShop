using MyShop.Contracts.DTOs.Customer;
using MyShop.Contracts.DTOs.Common;
using MyShop.Contracts.DTOs.Results;
using MyShop.Contracts.Services.BusinessRules;
using MyShop.Contracts.Enums.Business;

namespace MyShop.Application.Services;

/// <summary>
/// Customer business rules service demonstrating proper use of shared business rules infrastructure
/// </summary>
public class CustomerBusinessRulesService : IBusinessRuleService
{
    public async Task<BusinessRuleValidationResult> ValidateAsync<T>(T item)
    {
        var violations = new List<BusinessRuleViolation>();

        switch (item)
        {
            case CreateCustomerDto createDto:
                violations.AddRange(await ValidateCreateCustomerAsync(createDto));
                break;
            case UpdateCustomerDto updateDto:
                violations.AddRange(await ValidateUpdateCustomerAsync(updateDto));
                break;
            case CustomerDto customerDto:
                violations.AddRange(await ValidateCustomerAsync(customerDto));
                break;
            case QueryOptionsDto queryOptions:
                violations.AddRange(await ValidateQueryOptionsAsync(queryOptions));
                break;
        }

        return new BusinessRuleValidationResult
        {
            AreRulesSatisfied = !violations.Any(),
            Violations = violations
        };
    }

    private async Task<List<BusinessRuleViolation>> ValidateCreateCustomerAsync(CreateCustomerDto dto)
    {
        var violations = new List<BusinessRuleViolation>();

        // Business Rule: Customer must be at least 13 years old
        if (dto.DateOfBirth.HasValue)
        {
            var age = DateTime.Today.Year - dto.DateOfBirth.Value.Year;
            if (age < 13)
            {
                violations.Add(new BusinessRuleViolation
                {
                    ViolationCode = "CUSTOMER_MIN_AGE",
                    ViolationMessage = "Customer must be at least 13 years old",
                    RuleName = "MinimumAgeRule",
                    Category = "AgeValidation",
                    Severity = BusinessRuleSeverity.Error,
                    PropertyName = nameof(dto.DateOfBirth),
                    Value = dto.DateOfBirth
                });
            }
        }

        // Business Rule: Email must be unique (simulated check)
        if (!string.IsNullOrEmpty(dto.Email))
        {
            var isEmailUnique = await CheckEmailUniquenessAsync(dto.Email);
            if (!isEmailUnique)
            {
                violations.Add(new BusinessRuleViolation
                {
                    ViolationCode = "CUSTOMER_EMAIL_DUPLICATE",
                    ViolationMessage = "Email address is already registered",
                    RuleName = "EmailUniquenessRule",
                    Category = "EmailValidation",
                    Severity = BusinessRuleSeverity.Error,
                    PropertyName = nameof(dto.Email),
                    Value = dto.Email
                });
            }
        }

        // Business Rule: Mobile number must be unique (simulated check)
        if (!string.IsNullOrEmpty(dto.MobileNumber))
        {
            var isMobileUnique = await CheckMobileUniquenessAsync(dto.MobileNumber);
            if (!isMobileUnique)
            {
                violations.Add(new BusinessRuleViolation
                {
                    ViolationCode = "CUSTOMER_MOBILE_DUPLICATE",
                    ViolationMessage = "Mobile number is already registered",
                    RuleName = "MobileUniquenessRule",
                    Category = "MobileValidation",
                    Severity = BusinessRuleSeverity.Error,
                    PropertyName = nameof(dto.MobileNumber),
                    Value = dto.MobileNumber
                });
            }
        }

        return violations;
    }

    private async Task<List<BusinessRuleViolation>> ValidateUpdateCustomerAsync(UpdateCustomerDto dto)
    {
        var violations = new List<BusinessRuleViolation>();

        // Business Rule: Customer must be at least 13 years old
        if (dto.DateOfBirth.HasValue)
        {
            var age = DateTime.Today.Year - dto.DateOfBirth.Value.Year;
            if (age < 13)
            {
                violations.Add(new BusinessRuleViolation
                {
                    ViolationCode = "CUSTOMER_MIN_AGE",
                    ViolationMessage = "Customer must be at least 13 years old",
                    RuleName = "MinimumAgeRule",
                    Category = "AgeValidation",
                    Severity = BusinessRuleSeverity.Error,
                    PropertyName = nameof(dto.DateOfBirth),
                    Value = dto.DateOfBirth
                });
            }
        }

        // Business Rule: Email must be unique (excluding current customer)
        if (!string.IsNullOrEmpty(dto.Email))
        {
            var isEmailUnique = await CheckEmailUniquenessAsync(dto.Email, excludeCustomerId: null);
            if (!isEmailUnique)
            {
                violations.Add(new BusinessRuleViolation
                {
                    ViolationCode = "CUSTOMER_EMAIL_DUPLICATE",
                    ViolationMessage = "Email address is already registered",
                    RuleName = "EmailUniquenessRule",
                    Category = "EmailValidation",
                    Severity = BusinessRuleSeverity.Error,
                    PropertyName = nameof(dto.Email),
                    Value = dto.Email
                });
            }
        }

        return violations;
    }

    private async Task<List<BusinessRuleViolation>> ValidateCustomerAsync(CustomerDto customer)
    {
        var violations = new List<BusinessRuleViolation>();

        // Business Rule: Cannot delete verified customers
        if (customer.IsVerified)
        {
            violations.Add(new BusinessRuleViolation
            {
                ViolationCode = "CUSTOMER_CANNOT_DELETE_VERIFIED",
                ViolationMessage = "Cannot delete verified customers",
                RuleName = "VerifiedCustomerDeletionRule",
                Category = "DeletionValidation",
                Severity = BusinessRuleSeverity.Error,
                PropertyName = nameof(customer.IsVerified),
                Value = customer.IsVerified
            });
        }

        // Business Rule: Cannot delete customers with recent activity
        if (customer.LastLoginAt.HasValue && customer.LastLoginAt.Value > DateTime.UtcNow.AddDays(-30))
        {
            violations.Add(new BusinessRuleViolation
            {
                ViolationCode = "CUSTOMER_CANNOT_DELETE_RECENT_ACTIVITY",
                ViolationMessage = "Cannot delete customers with recent activity (within 30 days)",
                RuleName = "RecentActivityDeletionRule",
                Category = "DeletionValidation",
                Severity = BusinessRuleSeverity.Warning,
                PropertyName = nameof(customer.LastLoginAt),
                Value = customer.LastLoginAt
            });
        }

        return violations;
    }

    private async Task<List<BusinessRuleViolation>> ValidateQueryOptionsAsync(QueryOptionsDto options)
    {
        var violations = new List<BusinessRuleViolation>();

        // Business Rule: Maximum page size limit
        if (options.Pagination.PageSize > 100)
        {
            violations.Add(new BusinessRuleViolation
            {
                ViolationCode = "QUERY_MAX_PAGE_SIZE",
                ViolationMessage = "Page size cannot exceed 100",
                RuleName = "MaxPageSizeRule",
                Category = "QueryValidation",
                Severity = BusinessRuleSeverity.Error,
                PropertyName = nameof(options.Pagination.PageSize),
                Value = options.Pagination.PageSize
            });
        }

        // Business Rule: Maximum number of filters
        if (options.Filtering.Count > 10)
        {
            violations.Add(new BusinessRuleViolation
            {
                ViolationCode = "QUERY_MAX_FILTERS",
                ViolationMessage = "Cannot apply more than 10 filters",
                RuleName = "MaxFiltersRule",
                Category = "QueryValidation",
                Severity = BusinessRuleSeverity.Error,
                PropertyName = nameof(options.Filtering),
                Value = options.Filtering.Count
            });
        }

        // Business Rule: Maximum number of sort options
        if (options.Sorting.Count > 5)
        {
            violations.Add(new BusinessRuleViolation
            {
                ViolationCode = "QUERY_MAX_SORTS",
                ViolationMessage = "Cannot apply more than 5 sort options",
                RuleName = "MaxSortsRule",
                Category = "QueryValidation",
                Severity = BusinessRuleSeverity.Error,
                PropertyName = nameof(options.Sorting),
                Value = options.Sorting.Count
            });
        }

        return violations;
    }

    private async Task<bool> CheckEmailUniquenessAsync(string email, Guid? excludeCustomerId = null)
    {
        // Simulate database check
        await Task.Delay(10);
        
        // In real implementation, this would check the database
        // For demo purposes, we'll simulate some duplicates
        var duplicateEmails = new[] { "admin@example.com", "test@example.com" };
        return !duplicateEmails.Contains(email.ToLowerInvariant());
    }

    private async Task<bool> CheckMobileUniquenessAsync(string mobileNumber)
    {
        // Simulate database check
        await Task.Delay(10);
        
        // In real implementation, this would check the database
        // For demo purposes, we'll simulate some duplicates
        var duplicateMobiles = new[] { "+1234567890", "+0987654321" };
        return !duplicateMobiles.Contains(mobileNumber);
    }
}