using MyShop.Contracts.DTOs.Search;

namespace MyShop.Contracts.DTOs.Responses;

public class PagedApiResponse<T> : ApiResponse<IEnumerable<T>>
{
    #region Properties

    public PaginationInfo Pagination { get; set; } = null!;

    #endregion

    #region Constructors

    public PagedApiResponse() { }

    public PagedApiResponse(
        IEnumerable<T> items, 
        int pageNumber, 
        int pageSize, 
        int totalCount, 
        bool isSuccess = true, 
        string? message = null)
    {
        Data = items;
        IsSuccess = isSuccess;
        Message = message;
        Pagination = new PaginationInfo(pageNumber, pageSize, totalCount);
    }

    #endregion

    #region Factory Methods - Success Responses

    public static PagedApiResponse<T> Success(
        IEnumerable<T> items, 
        int pageNumber, 
        int pageSize, 
        int totalCount, 
        string? message = null)
    {
        return new PagedApiResponse<T>
        {
            IsSuccess = true,
            Data = items,
            Message = message ?? "Data retrieved successfully",
            Pagination = new PaginationInfo(pageNumber, pageSize, totalCount)
        };
    }

    public static PagedApiResponse<T> FromSearchResult<T>(
        SearchResult<T> searchResult, 
        int pageNumber, 
        int pageSize)
    {
        return new PagedApiResponse<T>
        {
            IsSuccess = true,
            Data = searchResult.Items,
            Message = "Search completed successfully",
            Pagination = new PaginationInfo(pageNumber, pageSize, searchResult.TotalCount),
            Metadata = new Dictionary<string, object>
            {
                ["SearchStatistics"] = searchResult.Statistics ?? new object(),
                ["Suggestions"] = searchResult.Suggestions,
                ["Highlights"] = searchResult.Highlights
            }
        };
    }

    public static PagedApiResponse<T> FromTupleResult(
        (IEnumerable<T> Items, int TotalCount) result, 
        int pageNumber, 
        int pageSize, 
        string? message = null)
    {
        return Success(result.Items, pageNumber, pageSize, result.TotalCount, message);
    }

    #endregion

    #region Factory Methods - Error Responses

    public static PagedApiResponse<T> Error(
        string error, 
        int pageNumber = 1, 
        int pageSize = 20, 
        string? errorCode = null)
    {
        return new PagedApiResponse<T>
        {
            IsSuccess = false,
            Data = Enumerable.Empty<T>(),
            Message = error,
            ErrorCode = errorCode,
            Errors = new[] { error },
            Pagination = new PaginationInfo(pageNumber, pageSize, 0)
        };
    }

    public static PagedApiResponse<T> Error(
        IEnumerable<string> errors, 
        int pageNumber = 1, 
        int pageSize = 20, 
        string? errorCode = null)
    {
        var errorList = errors.ToList();
        return new PagedApiResponse<T>
        {
            IsSuccess = false,
            Data = Enumerable.Empty<T>(),
            Message = errorList.FirstOrDefault() ?? "An error occurred",
            ErrorCode = errorCode,
            Errors = errorList,
            Pagination = new PaginationInfo(pageNumber, pageSize, 0)
        };
    }

    public static PagedApiResponse<T> ValidationError(
        IEnumerable<ValidationError> validationErrors, 
        int pageNumber = 1, 
        int pageSize = 20)
    {
        var errors = validationErrors.ToList();
        return new PagedApiResponse<T>
        {
            IsSuccess = false,
            Data = Enumerable.Empty<T>(),
            Message = "Validation failed",
            ErrorCode = "VALIDATION_ERROR",
            ValidationErrors = errors,
            Errors = errors.Select(e => e.ErrorMessage),
            Pagination = new PaginationInfo(pageNumber, pageSize, 0)
        };
    }

    public static PagedApiResponse<T> BusinessRuleError(
        IEnumerable<BusinessRuleViolation> violations, 
        int pageNumber = 1, 
        int pageSize = 20)
    {
        var violationList = violations.ToList();
        return new PagedApiResponse<T>
        {
            IsSuccess = false,
            Data = Enumerable.Empty<T>(),
            Message = "Business rule violation",
            ErrorCode = "BUSINESS_RULE_VIOLATION",
            BusinessRuleViolations = violationList,
            Errors = violationList.Select(v => v.ViolationMessage),
            Pagination = new PaginationInfo(pageNumber, pageSize, 0)
        };
    }

    #endregion

    #region Helper Methods

    public List<T> GetCurrentPageItems()
    {
        return Data?.ToList() ?? new List<T>();
    }

    public bool IsCurrentPageEmpty()
    {
        return Data == null || !Data.Any();
    }

    public string GetPaginationSummary()
    {
        return Pagination.ToSummaryString();
    }

    public ApiResponse<IEnumerable<T>> ToApiResponse()
    {
        return new ApiResponse<IEnumerable<T>>
        {
            IsSuccess = IsSuccess,
            Data = Data,
            Message = Message,
            ValidationErrors = ValidationErrors,
            BusinessRuleViolations = BusinessRuleViolations,
            Errors = Errors,
            ErrorCode = ErrorCode,
            Timestamp = Timestamp,
            CorrelationId = CorrelationId,
            Metadata = Metadata
        };
    }

    public PagedApiResponse<T> WithSearchMetadata<TSearchItem>(
        SearchResult<TSearchItem> searchResult)
    {
        Metadata ??= new Dictionary<string, object>();
        Metadata["SearchStatistics"] = searchResult.Statistics ?? new object();
        Metadata["Suggestions"] = searchResult.Suggestions;
        Metadata["Highlights"] = searchResult.Highlights;
        return this;
    }

    #endregion

    #region Overrides

    public override string ToString()
    {
        return $"PagedApiResponse<{typeof(T).Name}>(Success={IsSuccess}, Items={Data?.Count() ?? 0}, {Pagination})";
    }

    #endregion
}