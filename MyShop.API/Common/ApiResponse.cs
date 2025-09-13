namespace MyShop.API.Common;

/// <summary>
/// Standard API response wrapper
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public IReadOnlyList<string> Errors { get; set; } = Array.Empty<string>();
    public object? Meta { get; set; }
    public string TraceId { get; set; } = string.Empty;

    public static ApiResponse<T> SuccessResponse(T data, object? meta = null, string traceId = "")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Meta = meta,
            TraceId = traceId
        };
    }

    public static ApiResponse<T> Failure(IEnumerable<string> errors, string traceId = "")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Errors = errors.ToList().AsReadOnly(),
            TraceId = traceId
        };
    }

    public static ApiResponse<T> Failure(string error, string traceId = "")
    {
        return Failure(new[] { error }, traceId);
    }

    public static ApiResponse<T> FromResult(Result<T> result, object? meta = null, string traceId = "")
    {
        return result.IsSuccess 
            ? SuccessResponse(result.Value, meta, traceId)
            : Failure(result.Errors, traceId);
    }
}

/// <summary>
/// Standard API response wrapper without data
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public IReadOnlyList<string> Errors { get; set; } = Array.Empty<string>();
    public object? Meta { get; set; }
    public string TraceId { get; set; } = string.Empty;

    public static ApiResponse SuccessResponse(object? meta = null, string traceId = "")
    {
        return new ApiResponse
        {
            Success = true,
            Meta = meta,
            TraceId = traceId
        };
    }

    public static ApiResponse Failure(IEnumerable<string> errors, string traceId = "")
    {
        return new ApiResponse
        {
            Success = false,
            Errors = errors.ToList().AsReadOnly(),
            TraceId = traceId
        };
    }

    public static ApiResponse Failure(string error, string traceId = "")
    {
        return Failure(new[] { error }, traceId);
    }

    public static ApiResponse FromResult(Result result, object? meta = null, string traceId = "")
    {
        return result.IsSuccess 
            ? SuccessResponse(meta, traceId)
            : Failure(result.Errors, traceId);
    }
}