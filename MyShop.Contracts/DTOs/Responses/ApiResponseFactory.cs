using MyShop.Contracts.DTOs.Search;

namespace MyShop.Contracts.DTOs.Responses;

public static class ApiResponseFactory
{
    #region Success Response Factories

    public static ApiResponse<T> CreateSuccess<T>(T data, string? message = null)
    {
        return ApiResponse<T>.Success(data, message);
    }

    public static ApiResponse<T> CreateSuccess<T>(string? message = null)
    {
        return ApiResponse<T>.Success(message);
    }

    public static PagedApiResponse<T> CreatePagedSuccess<T>(
        IEnumerable<T> items, 
        int pageNumber, 
        int pageSize, 
        int totalCount, 
        string? message = null)
    {
        return PagedApiResponse<T>.Success(items, pageNumber, pageSize, totalCount, message);
    }

    public static ApiResponse<T> CreateCreated<T>(T data, string? message = null)
    {
        return ApiResponse<T>.Success(data, message ?? "Resource created successfully");
    }

    public static ApiResponse<T> CreateUpdated<T>(T data, string? message = null)
    {
        return ApiResponse<T>.Success(data, message ?? "Resource updated successfully");
    }

    public static ApiResponse<T> CreateDeleted<T>(string? message = null)
    {
        return ApiResponse<T>.Success(message ?? "Resource deleted successfully");
    }

    #endregion

    #region Error Response Factories

    public static ApiResponse<T> CreateNotFound<T>(string? message = null)
    {
        return ApiResponse<T>.Error(message ?? "Resource not found", "NOT_FOUND");
    }

    public static ApiResponse<T> CreateBadRequest<T>(string? message = null)
    {
        return ApiResponse<T>.Error(message ?? "Bad request", "BAD_REQUEST");
    }

    public static ApiResponse<T> CreateUnauthorized<T>(string? message = null)
    {
        return ApiResponse<T>.Error(message ?? "Unauthorized", "UNAUTHORIZED");
    }

    public static ApiResponse<T> CreateForbidden<T>(string? message = null)
    {
        return ApiResponse<T>.Error(message ?? "Forbidden", "FORBIDDEN");
    }

    public static ApiResponse<T> CreateConflict<T>(string? message = null)
    {
        return ApiResponse<T>.Error(message ?? "Conflict", "CONFLICT");
    }

    public static ApiResponse<T> CreateInternalServerError<T>(string? message = null)
    {
        return ApiResponse<T>.Error(message ?? "Internal server error", "INTERNAL_SERVER_ERROR");
    }

    public static ApiResponse<T> CreateValidationError<T>(IEnumerable<ValidationError> validationErrors)
    {
        return ApiResponse<T>.ValidationError(validationErrors);
    }

    public static ApiResponse<T> CreateBusinessRuleError<T>(IEnumerable<BusinessRuleViolation> violations)
    {
        return ApiResponse<T>.BusinessRuleError(violations);
    }

    #endregion

    #region Paged Error Response Factories

    public static PagedApiResponse<T> CreatePagedNotFound<T>(
        int pageNumber = 1, 
        int pageSize = 20, 
        string? message = null)
    {
        return PagedApiResponse<T>.Error(message ?? "Resource not found", pageNumber, pageSize, "NOT_FOUND");
    }

    public static PagedApiResponse<T> CreatePagedValidationError<T>(
        IEnumerable<ValidationError> validationErrors, 
        int pageNumber = 1, 
        int pageSize = 20)
    {
        return PagedApiResponse<T>.ValidationError(validationErrors, pageNumber, pageSize);
    }

    public static PagedApiResponse<T> CreatePagedBusinessRuleError<T>(
        IEnumerable<BusinessRuleViolation> violations, 
        int pageNumber = 1, 
        int pageSize = 20)
    {
        return PagedApiResponse<T>.BusinessRuleError(violations, pageNumber, pageSize);
    }

    #endregion

    #region Search Response Factories

    public static ApiResponse<IEnumerable<T>> CreateSearchSuccess<T>(
        SearchResult<T> searchResult, 
        string? message = null)
    {
        return searchResult.ToApiResponse(message);
    }

    public static PagedApiResponse<T> CreatePagedSearchSuccess<T>(
        SearchResult<T> searchResult, 
        int pageNumber, 
        int pageSize, 
        string? message = null)
    {
        return searchResult.ToPagedApiResponse(pageNumber, pageSize, message);
    }

    #endregion

    #region Bulk Operation Response Factories

    public static ApiResponse<T> CreateBulkSuccess<T>(BulkOperationResult bulkResult)
    {
        return bulkResult.ToApiResponse<T>();
    }

    public static ApiResponse<T> CreateBulkError<T>(BulkOperationResult bulkResult)
    {
        return bulkResult.ToApiResponse<T>();
    }

    #endregion

    #region Conditional Response Factories

    public static ApiResponse<T> CreateConditional<T>(
        bool condition, 
        T successData, 
        string successMessage, 
        string errorMessage)
    {
        return condition 
            ? CreateSuccess(successData, successMessage)
            : CreateBadRequest<T>(errorMessage);
    }

    public static ApiResponse<T> CreateConditional<T>(
        T? item, 
        string successMessage, 
        string errorMessage = "Item not found")
    {
        return item != null 
            ? CreateSuccess(item, successMessage)
            : CreateNotFound<T>(errorMessage);
    }

    #endregion

    #region HTTP Status Code Factories

    public static ApiResponse<T> CreateWithStatusCode<T>(
        int statusCode, 
        T? data = default, 
        string? message = null)
    {
        return statusCode switch
        {
            200 => CreateSuccess(data!, message ?? "OK"),
            201 => CreateCreated(data!, message),
            204 => CreateSuccess<T>(message ?? "No Content"),
            400 => CreateBadRequest<T>(message),
            401 => CreateUnauthorized<T>(message),
            403 => CreateForbidden<T>(message),
            404 => CreateNotFound<T>(message),
            409 => CreateConflict<T>(message),
            500 => CreateInternalServerError<T>(message),
            _ => CreateInternalServerError<T>(message ?? "Unknown error")
        };
    }

    #endregion

    #region Async Response Factories

    public static ApiResponse<T> CreateAsyncOperation<T>(
        T data, 
        string operationId, 
        string? message = null)
    {
        return CreateSuccess(data, message ?? "Operation started successfully")
            .WithMetadata("OperationId", operationId)
            .WithMetadata("IsAsync", true);
    }

    public static ApiResponse<T> CreateAsyncCompleted<T>(
        T data, 
        string operationId, 
        TimeSpan duration, 
        string? message = null)
    {
        return CreateSuccess(data, message ?? "Operation completed successfully")
            .WithMetadata("OperationId", operationId)
            .WithMetadata("IsAsync", false)
            .WithMetadata("Duration", duration.TotalMilliseconds);
    }

    #endregion
}