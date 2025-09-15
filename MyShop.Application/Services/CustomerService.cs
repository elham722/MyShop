using MyShop.Contracts.Common;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Contracts.DTOs.Common;
using MyShop.Contracts.Common.Pagination;
using MyShop.Contracts.Common.Filtering;
using MyShop.Contracts.Common.Sorting;
using MyShop.Contracts.DTOs.Results;
using MyShop.Contracts.Services.Query;
using MyShop.Contracts.Services.Command;
using MyShop.Contracts.Services.BusinessRules;
using MyShop.Contracts.Services.Validation;

namespace MyShop.Application.Services;

/// <summary>
/// Customer service demonstrating proper use of Result pattern and shared infrastructure
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly IQueryService _queryService;
    private readonly ICommandService _commandService;
    private readonly IBusinessRuleService _businessRuleService;
    private readonly IValidationService _validationService;

    public CustomerService(
        IQueryService queryService,
        ICommandService commandService,
        IBusinessRuleService businessRuleService,
        IValidationService validationService)
    {
        _queryService = queryService;
        _commandService = commandService;
        _businessRuleService = businessRuleService;
        _validationService = validationService;
    }

    /// <summary>
    /// Get customers with advanced query options
    /// </summary>
    public async Task<Result<PagedResult<CustomerDto>>> GetCustomersAsync(QueryOptionsDto options)
    {
        try
        {
            // Validate query options
            var validationResult = await _validationService.ValidateAsync(options);
            if (!validationResult.IsValid)
            {
                return Result<PagedResult<CustomerDto>>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Apply business rules
            var businessRuleResult = await _businessRuleService.ValidateAsync(options);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result<PagedResult<CustomerDto>>.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // Execute query using shared infrastructure
            var customers = await _queryService.GetCustomersAsync(options);
            
            return Result<PagedResult<CustomerDto>>.Success(customers);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<CustomerDto>>.Failure($"Failed to retrieve customers: {ex.Message}");
        }
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    public async Task<Result<CustomerDto>> GetCustomerByIdAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return Result<CustomerDto>.Failure("Customer ID cannot be empty");
            }

            var customer = await _queryService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return Result<CustomerDto>.Failure("Customer not found");
            }

            return Result<CustomerDto>.Success(customer);
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to retrieve customer: {ex.Message}");
        }
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    public async Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createDto, string createdBy)
    {
        try
        {
            // Validate input
            var validationResult = await _validationService.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return Result<CustomerDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Apply business rules
            var businessRuleResult = await _businessRuleService.ValidateAsync(createDto);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result<CustomerDto>.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // Create customer
            var customer = await _commandService.CreateCustomerAsync(createDto, createdBy);
            
            return Result<CustomerDto>.Success(customer);
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to create customer: {ex.Message}");
        }
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    public async Task<Result<CustomerDto>> UpdateCustomerAsync(Guid id, UpdateCustomerDto updateDto, string updatedBy)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return Result<CustomerDto>.Failure("Customer ID cannot be empty");
            }

            // Validate input
            var validationResult = await _validationService.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return Result<CustomerDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Check if customer exists
            var existingCustomer = await _queryService.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
            {
                return Result<CustomerDto>.Failure("Customer not found");
            }

            // Apply business rules
            var businessRuleResult = await _businessRuleService.ValidateAsync(updateDto);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result<CustomerDto>.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // Update customer
            var updatedCustomer = await _commandService.UpdateCustomerAsync(id, updateDto, updatedBy);
            
            return Result<CustomerDto>.Success(updatedCustomer);
        }
        catch (Exception ex)
        {
            return Result<CustomerDto>.Failure($"Failed to update customer: {ex.Message}");
        }
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    public async Task<Result> DeleteCustomerAsync(Guid id, string deletedBy)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return Result.Failure("Customer ID cannot be empty");
            }

            // Check if customer exists
            var existingCustomer = await _queryService.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
            {
                return Result.Failure("Customer not found");
            }

            // Apply business rules for deletion
            var businessRuleResult = await _businessRuleService.ValidateAsync(existingCustomer);
            if (!businessRuleResult.AreRulesSatisfied)
            {
                return Result.Failure(
                    businessRuleResult.Violations.Select(v => v.ViolationMessage));
            }

            // Delete customer
            await _commandService.DeleteCustomerAsync(id, deletedBy);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete customer: {ex.Message}");
        }
    }

    /// <summary>
    /// Get customer statistics
    /// </summary>
    public async Task<Result<CustomerStatisticsDto>> GetCustomerStatisticsAsync()
    {
        try
        {
            var statistics = await _queryService.GetCustomerStatisticsAsync();
            return Result<CustomerStatisticsDto>.Success(statistics);
        }
        catch (Exception ex)
        {
            return Result<CustomerStatisticsDto>.Failure($"Failed to retrieve customer statistics: {ex.Message}");
        }
    }

    /// <summary>
    /// Search customers with advanced filtering
    /// </summary>
    public async Task<Result<PagedResult<CustomerDto>>> SearchCustomersAsync(
        string? searchTerm,
        List<FilterDto>? filters,
        List<SortDto>? sorting,
        PaginationParams pagination)
    {
        try
        {
            // Build query options using shared infrastructure
            var queryOptions = QueryOptionsDto.Default
                .SetPagination(pagination.PageNumber, pagination.PageSize);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                queryOptions.SetSearch(searchTerm);
            }

            if (filters?.Any() == true)
            {
                foreach (var filter in filters)
                {
                    queryOptions.AddFilter(filter);
                }
            }

            if (sorting?.Any() == true)
            {
                foreach (var sort in sorting)
                {
                    queryOptions.AddSort(sort);
                }
            }

            return await GetCustomersAsync(queryOptions);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<CustomerDto>>.Failure($"Failed to search customers: {ex.Message}");
        }
    }

    /// <summary>
    /// Get customers by status
    /// </summary>
    public async Task<Result<PagedResult<CustomerDto>>> GetCustomersByStatusAsync(
        string status, 
        PaginationParams pagination)
    {
        try
        {
            var queryOptions = QueryOptionsDto.Default
                .SetPagination(pagination.PageNumber, pagination.PageSize)
                .AddFilter(FilterDto.Equals("Status", status));

            return await GetCustomersAsync(queryOptions);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<CustomerDto>>.Failure($"Failed to retrieve customers by status: {ex.Message}");
        }
    }

    /// <summary>
    /// Get verified customers
    /// </summary>
    public async Task<Result<PagedResult<CustomerDto>>> GetVerifiedCustomersAsync(PaginationParams pagination)
    {
        try
        {
            var queryOptions = QueryOptionsDto.Default
                .SetPagination(pagination.PageNumber, pagination.PageSize)
                .AddFilter(FilterDto.Equals("IsVerified", "true", "bool"));

            return await GetCustomersAsync(queryOptions);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<CustomerDto>>.Failure($"Failed to retrieve verified customers: {ex.Message}");
        }
    }
}

/// <summary>
/// Customer service interface
/// </summary>
public interface ICustomerService
{
    Task<Result<PagedResult<CustomerDto>>> GetCustomersAsync(QueryOptionsDto options);
    Task<Result<CustomerDto>> GetCustomerByIdAsync(Guid id);
    Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createDto, string createdBy);
    Task<Result<CustomerDto>> UpdateCustomerAsync(Guid id, UpdateCustomerDto updateDto, string updatedBy);
    Task<Result> DeleteCustomerAsync(Guid id, string deletedBy);
    Task<Result<CustomerStatisticsDto>> GetCustomerStatisticsAsync();
    Task<Result<PagedResult<CustomerDto>>> SearchCustomersAsync(
        string? searchTerm,
        List<FilterDto>? filters,
        List<SortDto>? sorting,
        PaginationParams pagination);
    Task<Result<PagedResult<CustomerDto>>> GetCustomersByStatusAsync(string status, PaginationParams pagination);
    Task<Result<PagedResult<CustomerDto>>> GetVerifiedCustomersAsync(PaginationParams pagination);
}