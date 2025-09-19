using Microsoft.AspNetCore.Mvc;
using MyShop.Contracts.Common.Pagination;
using MyShop.Contracts.DTOs.Responses;
using MyShop.Contracts.DTOs.Results;
using MyShop.API.Filters;

namespace MyShop.API.Controllers.Common.V1;

/// <summary>
/// Base controller with standardized response methods
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiResponse]
public abstract class BaseController : ControllerBase
{
    protected IActionResult FromResult<T>(Result<T> result)
    {
        var response = result.ToApiResponse(HttpContext);
        return result.IsSuccess ? Ok(response) : BadRequest(response);
    }

    protected IActionResult FromResult(Result result)
    {
        var response = result.ToApiResponse(HttpContext);
        return result.IsSuccess ? Ok(response) : BadRequest(response);
    }

    #region Success Responses

    /// <summary>
    /// Returns a successful response with data
    /// </summary>
    protected ActionResult<ApiResponse<T>> Success<T>(T data, string? message = null)
    {
        return Ok(ApiResponse<T>.Success(data, message));
    }

    /// <summary>
    /// Returns a successful response without data
    /// </summary>
    protected ActionResult<ApiResponse> Success(string? message = null)
    {
        return Ok(ApiResponse.Success(message));
    }

    /// <summary>
    /// Returns a successful response for created resource
    /// </summary>
    protected ActionResult<ApiResponse<T>> Created<T>(T data, string? message = null)
    {
        return StatusCode(201, ApiResponse<T>.Success(data, message ?? "Resource created successfully"));
    }

    /// <summary>
    /// Returns a successful response for updated resource
    /// </summary>
    protected ActionResult<ApiResponse<T>> Updated<T>(T data, string? message = null)
    {
        return Ok(ApiResponse<T>.Success(data, message ?? "Resource updated successfully"));
    }

    /// <summary>
    /// Returns a successful response for deleted resource
    /// </summary>
    protected ActionResult<ApiResponse> Deleted(string? message = null)
    {
        return Ok(ApiResponse.Success(message ?? "Resource deleted successfully"));
    }

    /// <summary>
    /// Returns a successful response for paged data
    /// </summary>
    protected ActionResult<PagedApiResponse<T>> PagedSuccess<T>(PagedResult<T> pagedResult, string? message = null)
    {
        return Ok(PagedApiResponse<T>.Success(
            pagedResult.Items, 
            pagedResult.PageNumber, 
            pagedResult.PageSize, 
            pagedResult.TotalCount, 
            message ?? "Data retrieved successfully"));
    }

    #endregion

    #region Error Responses

    /// <summary>
    /// Returns a bad request response
    /// </summary>
    protected ActionResult<ApiResponse> BadRequest(string message, string? errorCode = null)
    {
        return StatusCode(400, ApiResponse.Fail(message, errorCode ?? "BAD_REQUEST"));
    }

    /// <summary>
    /// Returns a bad request response with multiple errors
    /// </summary>
    protected ActionResult<ApiResponse> BadRequest(IEnumerable<string> errors, string? errorCode = null)
    {
        return StatusCode(400, ApiResponse.Fail(errors, errorCode ?? "BAD_REQUEST"));
    }

    /// <summary>
    /// Returns a bad request response with generic type
    /// </summary>
    protected ActionResult<ApiResponse<T>> BadRequest<T>(string message, string? errorCode = null)
    {
        return StatusCode(400, ApiResponse<T>.Error(message, errorCode ?? "BAD_REQUEST"));
    }

    /// <summary>
    /// Returns a bad request response with multiple errors and generic type
    /// </summary>
    protected ActionResult<ApiResponse<T>> BadRequest<T>(IEnumerable<string> errors, string? errorCode = null)
    {
        return StatusCode(400, ApiResponse<T>.Error(errors, errorCode ?? "BAD_REQUEST"));
    }

    /// <summary>
    /// Returns a not found response
    /// </summary>
    protected ActionResult<ApiResponse> NotFound(string message = "Resource not found", string? errorCode = null)
    {
        return StatusCode(404, ApiResponse.Fail(message, errorCode ?? "NOT_FOUND"));
    }

    /// <summary>
    /// Returns a not found response with generic type
    /// </summary>
    protected ActionResult<ApiResponse<T>> NotFound<T>(string message = "Resource not found", string? errorCode = null)
    {
        return StatusCode(404, ApiResponse<T>.Error(message, errorCode ?? "NOT_FOUND"));
    }

    /// <summary>
    /// Returns an unauthorized response
    /// </summary>
    protected ActionResult<ApiResponse> Unauthorized(string message = "Unauthorized access", string? errorCode = null)
    {
        return StatusCode(401, ApiResponse.Fail(message, errorCode ?? "UNAUTHORIZED"));
    }

    /// <summary>
    /// Returns a forbidden response
    /// </summary>
    protected ActionResult<ApiResponse> Forbidden(string message = "Access forbidden", string? errorCode = null)
    {
        return StatusCode(403, ApiResponse.Fail(message, errorCode ?? "FORBIDDEN"));
    }

    /// <summary>
    /// Returns a conflict response
    /// </summary>
    protected ActionResult<ApiResponse> Conflict(string message, string? errorCode = null)
    {
        return StatusCode(409, ApiResponse.Fail(message, errorCode ?? "CONFLICT"));
    }

    /// <summary>
    /// Returns an unprocessable entity response
    /// </summary>
    protected ActionResult<ApiResponse> UnprocessableEntity(string message, string? errorCode = null)
    {
        return StatusCode(422, ApiResponse.Fail(message, errorCode ?? "UNPROCESSABLE_ENTITY"));
    }

    /// <summary>
    /// Returns an internal server error response
    /// </summary>
    protected ActionResult<ApiResponse> InternalServerError(string message = "An internal server error occurred", string? errorCode = null)
    {
        return StatusCode(500, ApiResponse.Fail(message, errorCode ?? "INTERNAL_SERVER_ERROR"));
    }

    #endregion

    #region Validation Responses

    /// <summary>
    /// Returns a validation error response
    /// </summary>
    protected ActionResult<ApiResponse> ValidationError(IEnumerable<ValidationError> validationErrors)
    {
        return StatusCode(400, ApiResponse.ValidationError(validationErrors));
    }

    /// <summary>
    /// Returns a business rule violation response
    /// </summary>
    protected ActionResult<ApiResponse> BusinessRuleError(IEnumerable<BusinessRuleViolation> violations)
    {
        return StatusCode(400, ApiResponse.BusinessRuleError(violations));
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets the correlation ID from request headers
    /// </summary>
    protected string GetCorrelationId()
    {
        if (Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            return correlationId.ToString();
        }
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Adds correlation ID to response
    /// </summary>
    protected ApiResponse<T> WithCorrelationId<T>(ApiResponse<T> response)
    {
        return response.WithCorrelationId(GetCorrelationId());
    }

    /// <summary>
    /// Adds correlation ID to response
    /// </summary>
    protected ApiResponse WithCorrelationId(ApiResponse response)
    {
        return response.WithCorrelationId(GetCorrelationId());
    }

    #endregion
}