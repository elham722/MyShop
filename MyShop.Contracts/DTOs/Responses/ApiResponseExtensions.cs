using Microsoft.AspNetCore.Http;
using MyShop.Contracts.DTOs.Search;
using System.Net.Http;

namespace MyShop.Contracts.DTOs.Responses;

public static class ApiResponseExtensions
{

    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result, HttpContext context)
    {
        return new ApiResponse<T>
        {
            IsSuccess = result.IsSuccess,
            Data = result.Value,
            Errors = result.Errors,
            Timestamp = DateTime.UtcNow,
            CorrelationId = context.TraceIdentifier,
            Message = result.IsSuccess ? "Operation successful" : "Operation failed"
        };
    }

    public static ApiResponse ToApiResponse(this Result result, HttpContext context)
    {
        return new ApiResponse
        {
            IsSuccess = result.IsSuccess,
            Errors = result.Errors,
            Timestamp = DateTime.UtcNow,
            CorrelationId = context.TraceIdentifier,
            Message = result.IsSuccess ? "Operation successful" : "Operation failed"
        };
    }

    #region SearchResult Extensions

    public static ApiResponse<IEnumerable<T>> ToApiResponse<T>(
        this SearchResult<T> searchResult, 
        string? message = null)
    {
        return ApiResponse<IEnumerable<T>>.Success(
            searchResult.Items, 
            message ?? "Search completed successfully")
            .WithMetadata("SearchStatistics", searchResult.Statistics ?? new object())
            .WithMetadata("Suggestions", searchResult.Suggestions)
            .WithMetadata("Highlights", searchResult.Highlights);
    }

    public static PagedApiResponse<T> ToPagedApiResponse<T>(
        this SearchResult<T> searchResult, 
        int pageNumber, 
        int pageSize, 
        string? message = null)
    {
        return PagedApiResponse<T>.FromSearchResult(searchResult, pageNumber, pageSize);
    }

    #endregion

    #region Tuple Result Extensions

    public static ApiResponse<IEnumerable<T>> ToApiResponse<T>(
        this (IEnumerable<T> Items, int TotalCount) result, 
        string? message = null)
    {
        return ApiResponse<IEnumerable<T>>.Success(result.Items, message);
    }

    public static PagedApiResponse<T> ToPagedApiResponse<T>(
        this (IEnumerable<T> Items, int TotalCount) result, 
        int pageNumber, 
        int pageSize, 
        string? message = null)
    {
        return PagedApiResponse<T>.FromTupleResult(result, pageNumber, pageSize, message);
    }

    #endregion

    #region ValidationResult Extensions

    public static ApiResponse<T> ToApiResponse<T>(this ValidationResult validationResult)
    {
        if (validationResult.IsValid)
        {
            return ApiResponse<T>.Success(message: "Validation passed");
        }

        return ApiResponse<T>.ValidationError(validationResult.Errors);
    }

    #endregion

    #region BusinessRuleValidationResult Extensions

    public static ApiResponse<T> ToApiResponse<T>(this BusinessRuleValidationResult businessRuleResult)
    {
        if (businessRuleResult.AreRulesSatisfied)
        {
            return ApiResponse<T>.Success(message: "Business rules satisfied");
        }

        return ApiResponse<T>.BusinessRuleError(businessRuleResult.Violations);
    }

    #endregion

    #region BulkOperationResult Extensions

    public static ApiResponse<T> ToApiResponse<T>(this BulkOperationResult bulkResult)
    {
        if (bulkResult.IsSuccess)
        {
            return ApiResponse<T>.Success(
                message: $"Bulk operation completed successfully. {bulkResult.AffectedCount} items processed.");
        }

        var errors = new List<string>();
        
        if (bulkResult.Errors.Any())
            errors.AddRange(bulkResult.Errors);
            
        if (bulkResult.ValidationErrors.Any())
            errors.AddRange(bulkResult.ValidationErrors.Select(e => e.ErrorMessage));
            
        if (bulkResult.BusinessRuleViolations.Any())
            errors.AddRange(bulkResult.BusinessRuleViolations.Select(v => v.ViolationMessage));

        return ApiResponse<T>.Error(errors, "BULK_OPERATION_FAILED");
    }

    #endregion

    #region Generic Collection Extensions

    public static ApiResponse<IEnumerable<T>> ToApiResponse<T>(
        this IEnumerable<T> items, 
        string? message = null)
    {
        return ApiResponse<IEnumerable<T>>.Success(items, message);
    }

    public static PagedApiResponse<T> ToPagedApiResponse<T>(
        this IEnumerable<T> items, 
        int pageNumber, 
        int pageSize, 
        int totalCount, 
        string? message = null)
    {
        return PagedApiResponse<T>.Success(items, pageNumber, pageSize, totalCount, message);
    }

    #endregion

    #region Single Item Extensions

    public static ApiResponse<T> ToApiResponse<T>(this T item, string? message = null)
    {
        return ApiResponse<T>.Success(item, message);
    }

    public static ApiResponse<T> ToApiResponseNullable<T>(this T? item, string? message = null)
    {
        if (item == null)
        {
            return ApiResponse<T>.Error("Item not found", "NOT_FOUND");
        }

        return ApiResponse<T>.Success(item, message);
    }

    #endregion

    #region Error Handling Extensions

    public static ApiResponse<T> ToApiResponse<T>(this Exception exception, string? errorCode = null)
    {
        return ApiResponse<T>.Error(exception.Message, errorCode ?? "INTERNAL_ERROR");
    }

    public static ApiResponse<T> ToApiResponse<T>(this IEnumerable<Exception> exceptions, string? errorCode = null)
    {
        var messages = exceptions.Select(e => e.Message);
        return ApiResponse<T>.Error(messages, errorCode ?? "MULTIPLE_ERRORS");
    }

    #endregion

    #region Response Enhancement Extensions

    public static ApiResponse<T> WithCorrelationId<T>(this ApiResponse<T> response, string correlationId)
    {
        response.CorrelationId = correlationId;
        return response;
    }

    public static ApiResponse<T> WithMetadata<T>(this ApiResponse<T> response, string key, object value)
    {
        response.Metadata ??= new Dictionary<string, object>();
        response.Metadata[key] = value;
        return response;
    }

    public static ApiResponse<T> WithMetadata<T>(this ApiResponse<T> response, Dictionary<string, object> metadata)
    {
        response.Metadata ??= new Dictionary<string, object>();
        foreach (var kvp in metadata)
        {
            response.Metadata[kvp.Key] = kvp.Value;
        }
        return response;
    }

    public static ApiResponse<T> WithExecutionTime<T>(this ApiResponse<T> response, TimeSpan executionTime)
    {
        return response.WithMetadata("ExecutionTime", executionTime.TotalMilliseconds);
    }

    public static ApiResponse<T> WithRequestId<T>(this ApiResponse<T> response, string requestId)
    {
        return response.WithMetadata("RequestId", requestId);
    }

    #endregion

    #region Conditional Extensions

    public static ApiResponse<T> ToConditionalResponse<T>(
        this bool condition, 
        T data, 
        string successMessage, 
        string errorMessage)
    {
        return condition 
            ? ApiResponse<T>.Success(data, successMessage)
            : ApiResponse<T>.Error(errorMessage);
    }

    public static ApiResponse<T> ToConditionalResponse<T>(
        this T? item, 
        string successMessage, 
        string errorMessage = "Item not found")
    {
        return item != null 
            ? ApiResponse<T>.Success(item, successMessage)
            : ApiResponse<T>.Error(errorMessage, "NOT_FOUND");
    }

    #endregion
}