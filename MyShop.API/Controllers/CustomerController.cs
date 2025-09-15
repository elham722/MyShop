using Microsoft.AspNetCore.Mvc;
using MyShop.Contracts.CQRS.Commands.Customer;
using MyShop.Contracts.CQRS.Queries.Customer;
using MyShop.Contracts.DTOs.Customer;
using MyShop.Contracts.DTOs.Common;
using MyShop.Contracts.DTOs.Responses;
using MyShop.Contracts.Common.Pagination;
using MyShop.Contracts.Common.Filtering;
using MyShop.Contracts.Common.Sorting;
using MediatR;

namespace MyShop.API.Controllers;

/// <summary>
/// Customer management controller demonstrating proper use of shared infrastructure
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : BaseController
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all customers with pagination, filtering, and sorting
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20)</param>
    /// <param name="search">Search term</param>
    /// <param name="sortBy">Sort field (e.g., "FirstName", "LastName", "CreatedAt")</param>
    /// <param name="sortDirection">Sort direction ("asc" or "desc")</param>
    /// <param name="status">Filter by status</param>
    /// <param name="isVerified">Filter by verification status</param>
    /// <returns>Paged list of customers</returns>
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<CustomerDto>>> GetCustomers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortDirection = "asc",
        [FromQuery] string? status = null,
        [FromQuery] bool? isVerified = null)
    {
        // Build query options using shared infrastructure
        var queryOptions = QueryOptionsDto.Default
            .SetPagination(pageNumber, pageSize);

        // Add search if provided
        if (!string.IsNullOrWhiteSpace(search))
        {
            queryOptions.SetSearch(search);
        }

        // Add sorting if provided
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            queryOptions.AddSort(SortDto.Create(sortBy, sortDirection));
        }

        // Add filters if provided
        if (!string.IsNullOrWhiteSpace(status))
        {
            queryOptions.AddFilter(FilterDto.Equals("Status", status));
        }

        if (isVerified.HasValue)
        {
            queryOptions.AddFilter(FilterDto.Equals("IsVerified", isVerified.Value.ToString(), "bool"));
        }

        var query = new GetCustomersQuery { Options = queryOptions };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer details</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomer(Guid id)
    {
        var query = new GetCustomerByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="createCustomerDto">Customer creation data</param>
    /// <returns>Created customer</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationError(ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => new ValidationError
                {
                    ErrorMessage = e.ErrorMessage ?? "Validation error",
                    PropertyName = e.ErrorMessage
                }));
        }

        var command = new CreateCustomerCommand
        {
            FirstName = createCustomerDto.FirstName,
            LastName = createCustomerDto.LastName,
            DateOfBirth = createCustomerDto.DateOfBirth,
            Email = createCustomerDto.Email,
            MobileNumber = createCustomerDto.MobileNumber,
            CreatedBy = User.Identity?.Name ?? "System"
        };

        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? Created(result.Value, "Customer created successfully")
            : FromResult(result);
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="updateCustomerDto">Customer update data</param>
    /// <returns>Updated customer</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateCustomer(
        Guid id, 
        [FromBody] UpdateCustomerDto updateCustomerDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationError(ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => new ValidationError
                {
                    ErrorMessage = e.ErrorMessage ?? "Validation error",
                    PropertyName = e.ErrorMessage
                }));
        }

        var command = new UpdateCustomerCommand
        {
            Id = id,
            FirstName = updateCustomerDto.FirstName,
            LastName = updateCustomerDto.LastName,
            DateOfBirth = updateCustomerDto.DateOfBirth,
            Email = updateCustomerDto.Email,
            MobileNumber = updateCustomerDto.MobileNumber,
            UpdatedBy = User.Identity?.Name ?? "System"
        };

        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? Updated(result.Value, "Customer updated successfully")
            : FromResult(result);
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Deletion confirmation</returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteCustomer(Guid id)
    {
        var command = new DeleteCustomerCommand 
        { 
            Id = id,
            DeletedBy = User.Identity?.Name ?? "System"
        };

        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? Deleted("Customer deleted successfully")
            : FromResult(result);
    }

    /// <summary>
    /// Get customers with advanced filtering
    /// </summary>
    /// <param name="filters">Advanced filter options</param>
    /// <returns>Filtered customers</returns>
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<CustomerDto>>> SearchCustomers([FromBody] CustomerSearchRequest filters)
    {
        var queryOptions = QueryOptionsDto.Default
            .SetPagination(filters.PageNumber, filters.PageSize);

        // Add search term
        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            queryOptions.SetSearch(filters.Search);
        }

        // Add multiple filters
        if (filters.Filters?.Any() == true)
        {
            foreach (var filter in filters.Filters)
            {
                queryOptions.AddFilter(filter);
            }
        }

        // Add sorting
        if (filters.Sorting?.Any() == true)
        {
            foreach (var sort in filters.Sorting)
            {
                queryOptions.AddSort(sort);
            }
        }

        var query = new GetCustomersQuery { Options = queryOptions };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }

    /// <summary>
    /// Get customer statistics
    /// </summary>
    /// <returns>Customer statistics</returns>
    [HttpGet("statistics")]
    public async Task<ActionResult<ApiResponse<CustomerStatisticsDto>>> GetCustomerStatistics()
    {
        var query = new GetCustomerStatisticsQuery();
        var result = await _mediator.Send(query);

        return FromResult(result);
    }
}

/// <summary>
/// Advanced search request model
/// </summary>
public class CustomerSearchRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
    public List<FilterDto>? Filters { get; set; }
    public List<SortDto>? Sorting { get; set; }
}