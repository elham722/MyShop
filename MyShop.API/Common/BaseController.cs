using Microsoft.AspNetCore.Mvc;
using MyShop.Contracts.Common;

namespace MyShop.API.Common;

/// <summary>
/// Base controller providing common functionality for API responses
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Gets the current trace ID from the HTTP context
    /// </summary>
    protected string TraceId => HttpContext.TraceIdentifier;

    /// <summary>
    /// Returns a successful API response with data
    /// </summary>
    protected new ActionResult<ApiResponse<T>> Ok<T>(T data, object? meta = null)
    {
        return base.Ok(ApiResponse<T>.SuccessResponse(data, meta, TraceId));
    }

    /// <summary>
    /// Returns a successful API response without data
    /// </summary>
    protected new ActionResult<ApiResponse> Ok(object? meta = null)
    {
        return base.Ok(ApiResponse.SuccessResponse(meta, TraceId));
    }

    /// <summary>
    /// Returns a successful API response from a Result
    /// </summary>
    protected ActionResult<ApiResponse<T>> Ok<T>(Result<T> result, object? meta = null)
    {
        return base.Ok(ApiResponse<T>.FromResult(result, meta, TraceId));
    }

    /// <summary>
    /// Returns a successful API response from a Result without data
    /// </summary>
    protected ActionResult<ApiResponse> Ok(Result result, object? meta = null)
    {
        return base.Ok(ApiResponse.FromResult(result, meta, TraceId));
    }

    /// <summary>
    /// Returns a failed API response with errors
    /// </summary>
    protected ActionResult<ApiResponse<T>> BadRequest<T>(IEnumerable<string> errors)
    {
        return base.BadRequest(ApiResponse<T>.Failure(errors, TraceId));
    }

    /// <summary>
    /// Returns a failed API response with a single error
    /// </summary>
    protected ActionResult<ApiResponse<T>> BadRequest<T>(string error)
    {
        return base.BadRequest(ApiResponse<T>.Failure(error, TraceId));
    }

    /// <summary>
    /// Returns a failed API response without data
    /// </summary>
    protected ActionResult<ApiResponse> BadRequest(IEnumerable<string> errors)
    {
        return base.BadRequest(ApiResponse.Failure(errors, TraceId));
    }

    /// <summary>
    /// Returns a failed API response without data with a single error
    /// </summary>
    protected ActionResult<ApiResponse> BadRequest(string error)
    {
        return base.BadRequest(ApiResponse.Failure(error, TraceId));
    }

    /// <summary>
    /// Returns a failed API response from a Result
    /// </summary>
    protected ActionResult<ApiResponse<T>> BadRequest<T>(Result<T> result)
    {
        return base.BadRequest(ApiResponse<T>.FromResult(result, TraceId));
    }

    /// <summary>
    /// Returns a failed API response from a Result without data
    /// </summary>
    protected ActionResult<ApiResponse> BadRequest(Result result)
    {
        return base.BadRequest(ApiResponse.FromResult(result, TraceId));
    }

    /// <summary>
    /// Returns a not found API response
    /// </summary>
    protected ActionResult<ApiResponse<T>> NotFound<T>(string message = "Resource not found")
    {
        return base.NotFound(ApiResponse<T>.Failure(message, TraceId));
    }

    /// <summary>
    /// Returns a not found API response without data
    /// </summary>
    protected ActionResult<ApiResponse> NotFound(string message = "Resource not found")
    {
        return base.NotFound(ApiResponse.Failure(message, TraceId));
    }

    /// <summary>
    /// Returns an internal server error API response
    /// </summary>
    protected ActionResult<ApiResponse<T>> InternalServerError<T>(string message = "An internal server error occurred")
    {
        return StatusCode(500, ApiResponse<T>.Failure(message, TraceId));
    }

    /// <summary>
    /// Returns an internal server error API response without data
    /// </summary>
    protected ActionResult<ApiResponse> InternalServerError(string message = "An internal server error occurred")
    {
        return StatusCode(500, ApiResponse.Failure(message, TraceId));
    }
}