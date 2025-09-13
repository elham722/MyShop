using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.Common;
using MyShop.Contracts.CQRS.Commands.Customer;
using MyShop.Contracts.CQRS.Queries.Customer;
using MyShop.Contracts.DTOs.Customer;

namespace MyShop.API.Controllers;

/// <summary>
/// Controller for customer operations
/// </summary>
public class CustomersController : BaseController
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of customers
    /// </summary>
    /// <param name="query">Query parameters</param>
    /// <returns>Paginated list of customers</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<CustomerDto>>>> GetCustomers([FromQuery] GetCustomersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result, result.GetMeta());
    }

    /// <summary>
    /// Gets a customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer details</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomer(Guid id)
    {
        var query = new GetCustomerByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new customer
    /// </summary>
    /// <param name="command">Customer creation data</param>
    /// <returns>Created customer</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        var meta = new { 
            CreatedAt = DateTime.UtcNow,
            Operation = "CreateCustomer",
            Version = "1.0"
        };
        return Ok(result, meta);
    }

    /// <summary>
    /// Updates an existing customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="command">Customer update data</param>
    /// <returns>Updated customer</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerCommand command)
    {
        var updateCommand = command with { Id = id };
        var result = await _mediator.Send(updateCommand);
        var meta = new { 
            UpdatedAt = DateTime.UtcNow,
            Operation = "UpdateCustomer",
            CustomerId = id,
            Version = "1.0"
        };
        return Ok(result, meta);
    }

    /// <summary>
    /// Deletes a customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteCustomer(Guid id)
    {
        var command = new DeleteCustomerCommand { Id = id, DeletedBy = "API" };
        await _mediator.Send(command);
        var meta = new { 
            DeletedAt = DateTime.UtcNow,
            Operation = "DeleteCustomer",
            CustomerId = id,
            Version = "1.0"
        };
        return Ok((object?)meta);
    }
}