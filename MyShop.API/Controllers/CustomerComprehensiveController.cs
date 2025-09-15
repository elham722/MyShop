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
/// Comprehensive Customer controller demonstrating complete use of shared infrastructure
/// This controller shows the full flow: API -> CQRS -> Application -> Domain using Result and ApiResponse patterns
/// </summary>
[ApiController]
[Route("api/[controller]/comprehensive")]
public class CustomerComprehensiveController : BaseController
{
    private readonly IMediator _mediator;

    public CustomerComprehensiveController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Complete customer management workflow demonstrating all shared infrastructure
    /// </summary>
    [HttpPost("workflow")]
    public async Task<ActionResult<ApiResponse<CustomerWorkflowResultDto>>> CompleteCustomerWorkflow(
        [FromBody] CustomerWorkflowRequest request)
    {
        var workflowResult = new CustomerWorkflowResultDto();

        try
        {
            // Step 1: Create customer using Result pattern
            var createCommand = new CreateCustomerCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                CreatedBy = User.Identity?.Name ?? "System"
            };

            var createResult = await _mediator.Send(createCommand);
            if (createResult.IsFailure)
            {
                return BadRequest<CustomerWorkflowResultDto>(
                    string.Join("; ", createResult.Errors), 
                    "CUSTOMER_CREATION_FAILED");
            }

            workflowResult.CreatedCustomer = createResult.Value;

            // Step 2: Get customer statistics using Result pattern
            var statisticsQuery = new GetCustomerStatisticsQuery();
            var statisticsResult = await _mediator.Send(statisticsQuery);
            if (statisticsResult.IsSuccess)
            {
                workflowResult.Statistics = statisticsResult.Value;
            }

            // Step 3: Search for similar customers using advanced filtering
            var searchOptions = QueryOptionsDto.Default
                .SetPagination(1, 5)
                .AddFilter(FilterDto.Equals("Status", "Active"))
                .AddSort(SortDto.Descending("CreatedAt"));

            if (!string.IsNullOrEmpty(request.Email))
            {
                searchOptions.AddFilter(FilterDto.Contains("Email", request.Email.Split('@')[0]));
            }

            var searchQuery = new GetCustomersQuery { Options = searchOptions };
            var searchResult = await _mediator.Send(searchQuery);
            if (searchResult.IsSuccess)
            {
                workflowResult.SimilarCustomers = searchResult.Value.Items.ToList();
            }

            // Step 4: Update customer if requested
            if (request.UpdateData != null)
            {
                var updateCommand = new UpdateCustomerCommand
                {
                    Id = createResult.Value.Id,
                    FirstName = request.UpdateData.FirstName,
                    LastName = request.UpdateData.LastName,
                    DateOfBirth = request.UpdateData.DateOfBirth,
                    Email = request.UpdateData.Email,
                    MobileNumber = request.UpdateData.MobileNumber,
                    UpdatedBy = User.Identity?.Name ?? "System"
                };

                var updateResult = await _mediator.Send(updateCommand);
                if (updateResult.IsSuccess)
                {
                    workflowResult.UpdatedCustomer = updateResult.Value;
                }
                else
                {
                    workflowResult.UpdateErrors = updateResult.Errors.ToList();
                }
            }

            workflowResult.IsSuccess = true;
            workflowResult.Message = "Customer workflow completed successfully";

            return Success(workflowResult, "Workflow completed successfully");
        }
        catch (Exception ex)
        {
            return InternalServerError($"Workflow failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Demonstrate comprehensive filtering capabilities
    /// </summary>
    [HttpPost("advanced-search")]
    public async Task<ActionResult<PagedApiResponse<CustomerDto>>> AdvancedCustomerSearch(
        [FromBody] AdvancedSearchRequest request)
    {
        // Build comprehensive query options using all shared infrastructure components
        var queryOptions = QueryOptionsDto.Default
            .SetPagination(request.Pagination.PageNumber, request.Pagination.PageSize);

        // Add search term
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            queryOptions.SetSearch(request.SearchTerm);
        }

        // Add multiple filters using shared FilterDto infrastructure
        foreach (var filter in request.Filters)
        {
            queryOptions.AddFilter(filter);
        }

        // Add multiple sorting options using shared SortDto infrastructure
        foreach (var sort in request.Sorting)
        {
            queryOptions.AddSort(sort);
        }

        // Add additional options
        foreach (var option in request.AdditionalOptions)
        {
            queryOptions.AddOption(option.Key, option.Value);
        }

        var query = new GetCustomersQuery { Options = queryOptions };
        var result = await _mediator.Send(query);

        return FromResult(result);
    }

    /// <summary>
    /// Demonstrate error handling with different Result scenarios
    /// </summary>
    [HttpPost("error-scenarios")]
    public async Task<ActionResult<ApiResponse<ErrorScenarioResultDto>>> DemonstrateErrorScenarios(
        [FromBody] ErrorScenarioRequest request)
    {
        var result = new ErrorScenarioResultDto();

        // Scenario 1: Validation Error
        if (request.Scenario == "validation")
        {
            var invalidCommand = new CreateCustomerCommand
            {
                FirstName = "", // Invalid: empty first name
                LastName = "", // Invalid: empty last name
                Email = "invalid-email", // Invalid: bad email format
                CreatedBy = "System"
            };

            var validationResult = await _mediator.Send(invalidCommand);
            result.ValidationErrors = validationResult.Errors.ToList();
        }

        // Scenario 2: Business Rule Violation
        if (request.Scenario == "business-rule")
        {
            var businessRuleCommand = new CreateCustomerCommand
            {
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = DateTime.Today.AddYears(-10), // Invalid: too young
                Email = "admin@example.com", // Invalid: duplicate email
                CreatedBy = "System"
            };

            var businessRuleResult = await _mediator.Send(businessRuleCommand);
            result.BusinessRuleViolations = businessRuleResult.Errors.ToList();
        }

        // Scenario 3: Not Found Error
        if (request.Scenario == "not-found")
        {
            var notFoundQuery = new GetCustomerByIdQuery { Id = Guid.NewGuid() };
            var notFoundResult = await _mediator.Send(notFoundQuery);
            result.NotFoundError = notFoundResult.Errors.FirstOrDefault();
        }

        return Success(result, "Error scenarios demonstrated");
    }

    /// <summary>
    /// Demonstrate pagination with different scenarios
    /// </summary>
    [HttpGet("pagination-examples")]
    public async Task<ActionResult<ApiResponse<PaginationExamplesDto>>> DemonstratePagination()
    {
        var examples = new PaginationExamplesDto();

        // Example 1: First page
        var firstPageQuery = new GetCustomersQuery
        {
            Options = QueryOptionsDto.WithPagination(1, 10)
        };
        var firstPageResult = await _mediator.Send(firstPageQuery);
        if (firstPageResult.IsSuccess)
        {
            examples.FirstPage = firstPageResult.Value;
        }

        // Example 2: Large page size
        var largePageQuery = new GetCustomersQuery
        {
            Options = QueryOptionsDto.WithPagination(1, 50)
        };
        var largePageResult = await _mediator.Send(largePageQuery);
        if (largePageResult.IsSuccess)
        {
            examples.LargePage = largePageResult.Value;
        }

        // Example 3: With sorting
        var sortedQuery = new GetCustomersQuery
        {
            Options = QueryOptionsDto.Default
                .SetPagination(1, 20)
                .AddSort(SortDto.Descending("CreatedAt"))
                .AddSort(SortDto.Ascending("FirstName"))
        };
        var sortedResult = await _mediator.Send(sortedQuery);
        if (sortedResult.IsSuccess)
        {
            examples.SortedPage = sortedResult.Value;
        }

        return Success(examples, "Pagination examples demonstrated");
    }
}

/// <summary>
/// Customer workflow request model
/// </summary>
public class CustomerWorkflowRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? MobileNumber { get; set; }
    public UpdateCustomerDto? UpdateData { get; set; }
}

/// <summary>
/// Customer workflow result model
/// </summary>
public class CustomerWorkflowResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public CustomerDto? CreatedCustomer { get; set; }
    public CustomerDto? UpdatedCustomer { get; set; }
    public CustomerStatisticsDto? Statistics { get; set; }
    public List<CustomerDto> SimilarCustomers { get; set; } = new();
    public List<string> UpdateErrors { get; set; } = new();
}

/// <summary>
/// Advanced search request model
/// </summary>
public class AdvancedSearchRequest
{
    public PaginationParams Pagination { get; set; } = PaginationParams.Default;
    public string? SearchTerm { get; set; }
    public List<FilterDto> Filters { get; set; } = new();
    public List<SortDto> Sorting { get; set; } = new();
    public Dictionary<string, object> AdditionalOptions { get; set; } = new();
}

/// <summary>
/// Error scenario request model
/// </summary>
public class ErrorScenarioRequest
{
    public string Scenario { get; set; } = string.Empty; // "validation", "business-rule", "not-found"
}

/// <summary>
/// Error scenario result model
/// </summary>
public class ErrorScenarioResultDto
{
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> BusinessRuleViolations { get; set; } = new();
    public string? NotFoundError { get; set; }
}

/// <summary>
/// Pagination examples DTO
/// </summary>
public class PaginationExamplesDto
{
    public PagedResult<CustomerDto>? FirstPage { get; set; }
    public PagedResult<CustomerDto>? LargePage { get; set; }
    public PagedResult<CustomerDto>? SortedPage { get; set; }
}