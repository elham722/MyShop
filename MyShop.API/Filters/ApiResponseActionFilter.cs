using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyShop.Contracts.DTOs.Responses;
using System.Text.Json;

namespace MyShop.API.Filters;

/// <summary>
/// Action filter to standardize API responses
/// </summary>
public class ApiResponseActionFilter : IActionFilter
{
    private readonly ILogger<ApiResponseActionFilter> _logger;

    public ApiResponseActionFilter(ILogger<ApiResponseActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Log request details
        _logger.LogDebug("Executing action {ActionName} on controller {ControllerName}", 
            context.ActionDescriptor.DisplayName, 
            context.Controller.GetType().Name);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            // Let exception handling middleware handle exceptions
            return;
        }

        var result = context.Result;
        if (result == null) return;

        // Convert different result types to ApiResponse
        var apiResponse = ConvertToApiResponse(result, context.HttpContext);
        
        if (apiResponse != null)
        {
            context.Result = new JsonResult(apiResponse)
            {
                StatusCode = GetStatusCode(result),
                ContentType = "application/json"
            };
        }
    }

    private static ApiResponse? ConvertToApiResponse(IActionResult result, HttpContext context)
    {
        // ObjectResult (Ok, Created, etc.)
        if (result is ObjectResult objectResult)
            return ConvertObjectResult(objectResult);
        
        // StatusCodeResult (NoContent, NotFound, etc.)
        if (result is StatusCodeResult statusCodeResult)
            return ConvertStatusCodeResult(statusCodeResult);
        
        // BadRequestObjectResult
        if (result is BadRequestObjectResult badRequestResult)
            return ConvertBadRequestResult(badRequestResult);
        
        // NotFoundObjectResult
        if (result is NotFoundObjectResult notFoundResult)
            return ConvertNotFoundResult(notFoundResult);
        
        // UnauthorizedObjectResult
        if (result is UnauthorizedObjectResult unauthorizedResult)
            return ConvertUnauthorizedResult(unauthorizedResult);
        
        // ConflictObjectResult
        if (result is ConflictObjectResult conflictResult)
            return ConvertConflictResult(conflictResult);
        
        // RedirectResult, FileResult, etc. - don't convert
        if (result is RedirectResult or FileResult)
            return null;
        
        // Default case
        return ConvertGenericResult(result);
    }

    private static ApiResponse ConvertObjectResult(ObjectResult objectResult)
    {
        var statusCode = objectResult.StatusCode ?? 200;
        
        // If it's already an ApiResponse, return as is
        if (objectResult.Value is ApiResponse apiResponse)
        {
            return apiResponse;
        }

        // If it's a generic ApiResponse<T>, convert to non-generic
        if (objectResult.Value?.GetType().IsGenericType == true && 
            objectResult.Value.GetType().GetGenericTypeDefinition() == typeof(ApiResponse<>))
        {
            var genericApiResponse = objectResult.Value;
            var isSuccessProperty = genericApiResponse.GetType().GetProperty("IsSuccess");
            var messageProperty = genericApiResponse.GetType().GetProperty("Message");
            var dataProperty = genericApiResponse.GetType().GetProperty("Data");
            var errorCodeProperty = genericApiResponse.GetType().GetProperty("ErrorCode");
            var errorsProperty = genericApiResponse.GetType().GetProperty("Errors");

            return new ApiResponse
            {
                IsSuccess = (bool)(isSuccessProperty?.GetValue(genericApiResponse) ?? true),
                Message = messageProperty?.GetValue(genericApiResponse)?.ToString(),
                ErrorCode = errorCodeProperty?.GetValue(genericApiResponse)?.ToString(),
                Errors = errorsProperty?.GetValue(genericApiResponse) as IEnumerable<string>
            };
        }

        // Convert other object results
        return statusCode switch
        {
            >= 200 and < 300 => ApiResponse.Success("Operation completed successfully"),
            >= 400 and < 500 => ApiResponse.Fail("Client error occurred", "CLIENT_ERROR"),
            >= 500 => ApiResponse.Fail("Server error occurred", "SERVER_ERROR"),
            _ => ApiResponse.Success("Operation completed")
        };
    }

    private static ApiResponse ConvertStatusCodeResult(StatusCodeResult statusCodeResult)
    {
        var statusCode = statusCodeResult.StatusCode;
        
        return statusCode switch
        {
            200 => ApiResponse.Success("Operation completed successfully"),
            201 => ApiResponse.Success("Resource created successfully"),
            204 => ApiResponse.Success("Operation completed successfully"),
            400 => ApiResponse.Fail("Bad request", "BAD_REQUEST"),
            401 => ApiResponse.Fail("Unauthorized", "UNAUTHORIZED"),
            403 => ApiResponse.Fail("Forbidden", "FORBIDDEN"),
            404 => ApiResponse.Fail("Resource not found", "NOT_FOUND"),
            409 => ApiResponse.Fail("Conflict", "CONFLICT"),
            422 => ApiResponse.Fail("Unprocessable entity", "UNPROCESSABLE_ENTITY"),
            500 => ApiResponse.Fail("Internal server error", "INTERNAL_SERVER_ERROR"),
            _ => statusCode >= 400 
                ? ApiResponse.Fail($"Error occurred (Status: {statusCode})", "ERROR")
                : ApiResponse.Success("Operation completed")
        };
    }

    private static ApiResponse ConvertBadRequestResult(BadRequestObjectResult badRequestResult)
    {
        if (badRequestResult.Value is string message)
        {
            return ApiResponse.Fail(message, "BAD_REQUEST");
        }

        if (badRequestResult.Value is IEnumerable<string> errors)
        {
            return ApiResponse.Fail(errors, "BAD_REQUEST");
        }

        return ApiResponse.Fail("Bad request", "BAD_REQUEST");
    }

    private static ApiResponse ConvertNotFoundResult(NotFoundObjectResult notFoundResult)
    {
        if (notFoundResult.Value is string message)
        {
            return ApiResponse.Fail(message, "NOT_FOUND");
        }

        return ApiResponse.Fail("Resource not found", "NOT_FOUND");
    }

    private static ApiResponse ConvertUnauthorizedResult(UnauthorizedObjectResult unauthorizedResult)
    {
        if (unauthorizedResult.Value is string message)
        {
            return ApiResponse.Fail(message, "UNAUTHORIZED");
        }

        return ApiResponse.Fail("Unauthorized", "UNAUTHORIZED");
    }

    private static ApiResponse ConvertConflictResult(ConflictObjectResult conflictResult)
    {
        if (conflictResult.Value is string message)
        {
            return ApiResponse.Fail(message, "CONFLICT");
        }

        return ApiResponse.Fail("Conflict", "CONFLICT");
    }

    private static ApiResponse ConvertGenericResult(IActionResult result)
    {
        return ApiResponse.Success("Operation completed");
    }

    private static int GetStatusCode(IActionResult result)
    {
        if (result is ObjectResult objectResult)
            return objectResult.StatusCode ?? 200;
        
        if (result is StatusCodeResult statusCodeResult)
            return statusCodeResult.StatusCode;
        
        if (result is BadRequestObjectResult)
            return 400;
        
        if (result is NotFoundObjectResult)
            return 404;
        
        if (result is UnauthorizedObjectResult)
            return 401;
        
        if (result is ConflictObjectResult)
            return 409;
        
        return 200;
    }
}