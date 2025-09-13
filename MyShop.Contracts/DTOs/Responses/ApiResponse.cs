namespace MyShop.Contracts.DTOs.Responses;

public class ApiResponse<T>
{
    #region Core Properties

    public bool IsSuccess { get; set; }

    public T? Data { get; set; }

    public string? Message { get; set; }

    #endregion

    #region Error Properties

    public IEnumerable<ValidationError>? ValidationErrors { get; set; }

    public IEnumerable<BusinessRuleViolation>? BusinessRuleViolations { get; set; }

    public IEnumerable<string>? Errors { get; set; }

    public string? ErrorCode { get; set; }

    #endregion

    #region Metadata Properties

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string? CorrelationId { get; set; }

    public Dictionary<string, object>? Metadata { get; set; }

    #endregion

    #region Factory Methods - Success Responses

    public static ApiResponse<T> Success(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message ?? "Operation completed successfully"
        };
    }

    public static ApiResponse<T> Success(string? message = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message ?? "Operation completed successfully"
        };
    }

    #endregion

    #region Factory Methods - Error Responses

    public static ApiResponse<T> Error(string error, string? errorCode = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = error,
            ErrorCode = errorCode,
            Errors = new[] { error }
        };
    }

    public static ApiResponse<T> Error(IEnumerable<string> errors, string? errorCode = null)
    {
        var errorList = errors.ToList();
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = errorList.FirstOrDefault() ?? "An error occurred",
            ErrorCode = errorCode,
            Errors = errorList
        };
    }

    public static ApiResponse<T> ValidationError(IEnumerable<ValidationError> validationErrors)
    {
        var errors = validationErrors.ToList();
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = "Validation failed",
            ErrorCode = "VALIDATION_ERROR",
            ValidationErrors = errors,
            Errors = errors.Select(e => e.ErrorMessage)
        };
    }

    public static ApiResponse<T> BusinessRuleError(IEnumerable<BusinessRuleViolation> violations)
    {
        var violationList = violations.ToList();
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = "Business rule violation",
            ErrorCode = "BUSINESS_RULE_VIOLATION",
            BusinessRuleViolations = violationList,
            Errors = violationList.Select(v => v.ViolationMessage)
        };
    }

    #endregion

    #region Helper Methods

    public ApiResponse<T> WithMetadata(string key, object value)
    {
        Metadata ??= new Dictionary<string, object>();
        Metadata[key] = value;
        return this;
    }

    public ApiResponse<T> WithCorrelationId(string correlationId)
    {
        CorrelationId = correlationId;
        return this;
    }

    public PagedApiResponse<TItem> ToPagedResponse<TItem>(
        int pageNumber, 
        int pageSize, 
        int totalCount) where TItem : class
    {
        if (Data is IEnumerable<TItem> items)
        {
            return PagedApiResponse<TItem>.Success(
                items, 
                pageNumber, 
                pageSize, 
                totalCount, 
                Message);
        }

        throw new InvalidOperationException("Data must be enumerable to convert to paged response");
    }

    #endregion
}