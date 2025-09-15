using Microsoft.AspNetCore.Mvc;
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
/// Advanced Customer controller demonstrating comprehensive use of shared infrastructure
/// </summary>
[ApiController]
[Route("api/[controller]/advanced")]
public class CustomerAdvancedController : BaseController
{
    private readonly IMediator _mediator;

    public CustomerAdvancedController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get customers with complex filtering and sorting
    /// </summary>
    [HttpPost("filter")]
    public async Task<ActionResult<PagedApiResponse<CustomerDto>>> GetCustomersWithComplexFilter(
        [FromBody] ComplexCustomerFilterRequest request)
    {
        // Build comprehensive query options using shared infrastructure
        var queryOptions = QueryOptionsDto.Default
            .SetPagination(request.PageNumber, request.PageSize);

        // Add search
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            queryOptions.SetSearch(request.Search);
        }

        // Add multiple filters using shared FilterDto infrastructure
        if (request.Filters?.Any() == true)
        {
            foreach (var filter in request.Filters)
            {
                queryOptions.AddFilter(filter);
            }
        }

        // Add multiple sorting options using shared SortDto infrastructure
        if (request.Sorting?.Any() == true)
        {
            foreach (var sort in request.Sorting)
            {
                queryOptions.AddSort(sort);
            }
        }

        // Add additional options
        if (request.IncludeInactive == true)
        {
            queryOptions.AddOption("IncludeInactive", true);
        }

        if (request.MinAge.HasValue)
        {
            queryOptions.AddFilter(FilterDto.GreaterThanOrEqual("Age", request.MinAge.Value.ToString(), "int"));
        }

        if (request.MaxAge.HasValue)
        {
            queryOptions.AddFilter(FilterDto.LessThanOrEqual("Age", request.MaxAge.Value.ToString(), "int"));
        }

        var query = new GetCustomersQuery { Options = queryOptions };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }

    /// <summary>
    /// Get customers by multiple statuses
    /// </summary>
    [HttpPost("by-statuses")]
    public async Task<ActionResult<PagedApiResponse<CustomerDto>>> GetCustomersByStatuses(
        [FromBody] CustomerStatusFilterRequest request)
    {
        var queryOptions = QueryOptionsDto.Default
            .SetPagination(request.PageNumber, request.PageSize);

        // Use IN filter for multiple statuses
        if (request.Statuses?.Any() == true)
        {
            queryOptions.AddFilter(FilterDto.In("Status", request.Statuses.ToArray()));
        }

        // Add sorting
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            queryOptions.AddSort(SortDto.Create(request.SortBy, request.SortDirection ?? "asc"));
        }

        var query = new GetCustomersQuery { Options = queryOptions };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }

    /// <summary>
    /// Get customers with date range filtering
    /// </summary>
    [HttpPost("by-date-range")]
    public async Task<ActionResult<PagedApiResponse<CustomerDto>>> GetCustomersByDateRange(
        [FromBody] CustomerDateRangeRequest request)
    {
        var queryOptions = QueryOptionsDto.Default
            .SetPagination(request.PageNumber, request.PageSize);

        // Add date range filters
        if (request.CreatedFrom.HasValue)
        {
            queryOptions.AddFilter(FilterDto.GreaterThanOrEqual("CreatedAt", 
                request.CreatedFrom.Value.ToString("yyyy-MM-dd"), "datetime"));
        }

        if (request.CreatedTo.HasValue)
        {
            queryOptions.AddFilter(FilterDto.LessThanOrEqual("CreatedAt", 
                request.CreatedTo.Value.ToString("yyyy-MM-dd"), "datetime"));
        }

        if (request.LastLoginFrom.HasValue)
        {
            queryOptions.AddFilter(FilterDto.GreaterThanOrEqual("LastLoginAt", 
                request.LastLoginFrom.Value.ToString("yyyy-MM-dd"), "datetime"));
        }

        if (request.LastLoginTo.HasValue)
        {
            queryOptions.AddFilter(FilterDto.LessThanOrEqual("LastLoginAt", 
                request.LastLoginTo.Value.ToString("yyyy-MM-dd"), "datetime"));
        }

        // Add sorting
        queryOptions.AddSort(SortDto.Descending("CreatedAt"));

        var query = new GetCustomersQuery { Options = queryOptions };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }

    /// <summary>
    /// Get customers with verification status filtering
    /// </summary>
    [HttpPost("by-verification")]
    public async Task<ActionResult<PagedApiResponse<CustomerDto>>> GetCustomersByVerification(
        [FromBody] CustomerVerificationRequest request)
    {
        var queryOptions = QueryOptionsDto.Default
            .SetPagination(request.PageNumber, request.PageSize);

        // Add verification filters
        if (request.IsEmailVerified.HasValue)
        {
            queryOptions.AddFilter(FilterDto.Equals("IsEmailVerified", 
                request.IsEmailVerified.Value.ToString(), "bool"));
        }

        if (request.IsPhoneVerified.HasValue)
        {
            queryOptions.AddFilter(FilterDto.Equals("IsPhoneVerified", 
                request.IsPhoneVerified.Value.ToString(), "bool"));
        }

        if (request.IsVerified.HasValue)
        {
            queryOptions.AddFilter(FilterDto.Equals("IsVerified", 
                request.IsVerified.Value.ToString(), "bool"));
        }

        if (request.HasCompleteProfile.HasValue)
        {
            queryOptions.AddFilter(FilterDto.Equals("HasCompleteProfile", 
                request.HasCompleteProfile.Value.ToString(), "bool"));
        }

        // Add sorting
        queryOptions.AddSort(SortDto.Descending("CreatedAt"));

        var query = new GetCustomersQuery { Options = queryOptions };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }

    /// <summary>
    /// Get customers with custom field filtering
    /// </summary>
    [HttpPost("custom-filters")]
    public async Task<ActionResult<PagedApiResponse<CustomerDto>>> GetCustomersWithCustomFilters(
        [FromBody] CustomFilterRequest request)
    {
        var queryOptions = QueryOptionsDto.Default
            .SetPagination(request.PageNumber, request.PageSize);

        // Add custom filters
        foreach (var customFilter in request.CustomFilters)
        {
            queryOptions.AddFilter(customFilter);
        }

        // Add sorting
        foreach (var sort in request.Sorting)
        {
            queryOptions.AddSort(sort);
        }

        var query = new GetCustomersQuery { Options = queryOptions };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }

    /// <summary>
    /// Demonstrate all filter operators
    /// </summary>
    [HttpGet("filter-examples")]
    public ActionResult<ApiResponse<FilterExamplesDto>> GetFilterExamples()
    {
        var examples = new FilterExamplesDto
        {
            Examples = new List<FilterExample>
            {
                new() { Operator = "Equals", Description = "Exact match", Example = FilterDto.Equals("Status", "Active") },
                new() { Operator = "NotEquals", Description = "Not equal", Example = FilterDto.NotEquals("Status", "Inactive") },
                new() { Operator = "Contains", Description = "Contains text", Example = FilterDto.Contains("FirstName", "John") },
                new() { Operator = "NotContains", Description = "Does not contain", Example = FilterDto.NotContains("FirstName", "Test") },
                new() { Operator = "StartsWith", Description = "Starts with", Example = FilterDto.StartsWith("Email", "admin") },
                new() { Operator = "EndsWith", Description = "Ends with", Example = FilterDto.EndsWith("Email", ".com") },
                new() { Operator = "GreaterThan", Description = "Greater than", Example = FilterDto.GreaterThan("Age", "18", "int") },
                new() { Operator = "LessThan", Description = "Less than", Example = FilterDto.LessThan("Age", "65", "int") },
                new() { Operator = "GreaterThanOrEqual", Description = "Greater than or equal", Example = FilterDto.GreaterThanOrEqual("Age", "21", "int") },
                new() { Operator = "LessThanOrEqual", Description = "Less than or equal", Example = FilterDto.LessThanOrEqual("Age", "60", "int") },
                new() { Operator = "In", Description = "In list", Example = FilterDto.In("Status", "Active", "Pending") },
                new() { Operator = "NotIn", Description = "Not in list", Example = FilterDto.NotIn("Status", "Deleted", "Suspended") },
                new() { Operator = "IsNull", Description = "Is null", Example = FilterDto.IsNull("LastLoginAt") },
                new() { Operator = "IsNotNull", Description = "Is not null", Example = FilterDto.IsNotNull("LastLoginAt") },
                new() { Operator = "IsEmpty", Description = "Is empty", Example = FilterDto.IsEmpty("Notes") },
                new() { Operator = "IsNotEmpty", Description = "Is not empty", Example = FilterDto.IsNotEmpty("Notes") }
            }
        };

        return Success(examples, "Filter examples retrieved successfully");
    }
}

/// <summary>
/// Complex filter request model
/// </summary>
public class ComplexCustomerFilterRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
    public List<FilterDto>? Filters { get; set; }
    public List<SortDto>? Sorting { get; set; }
    public bool? IncludeInactive { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
}

/// <summary>
/// Customer status filter request model
/// </summary>
public class CustomerStatusFilterRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public List<string>? Statuses { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}

/// <summary>
/// Customer date range request model
/// </summary>
public class CustomerDateRangeRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public DateTime? LastLoginFrom { get; set; }
    public DateTime? LastLoginTo { get; set; }
}

/// <summary>
/// Customer verification request model
/// </summary>
public class CustomerVerificationRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool? IsEmailVerified { get; set; }
    public bool? IsPhoneVerified { get; set; }
    public bool? IsVerified { get; set; }
    public bool? HasCompleteProfile { get; set; }
}

/// <summary>
/// Custom filter request model
/// </summary>
public class CustomFilterRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public List<FilterDto> CustomFilters { get; set; } = new();
    public List<SortDto> Sorting { get; set; } = new();
}

/// <summary>
/// Filter examples DTO
/// </summary>
public class FilterExamplesDto
{
    public List<FilterExample> Examples { get; set; } = new();
}

/// <summary>
/// Filter example
/// </summary>
public class FilterExample
{
    public string Operator { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FilterDto Example { get; set; } = new();
}