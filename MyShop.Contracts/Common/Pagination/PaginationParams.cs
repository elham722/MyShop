namespace MyShop.Contracts.Common.Pagination;

/// <summary>
/// Enhanced pagination parameters with flexible configuration
/// </summary>
public class PaginationParams
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 20;
    private const int MinPageSize = 1;
    private const int MaxPageSize = 1000;

    private int _pageNumber = DefaultPageNumber;
    private int _pageSize = DefaultPageSize;

    /// <summary>
    /// Page number (1-based)
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? DefaultPageNumber : value;
    }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < MinPageSize ? DefaultPageSize : Math.Min(value, MaxPageSize);
    }

    /// <summary>
    /// Maximum allowed page size
    /// </summary>
    public static int MaxAllowedPageSize => MaxPageSize;

    /// <summary>
    /// Minimum allowed page size
    /// </summary>
    public static int MinAllowedPageSize => MinPageSize;

    /// <summary>
    /// Default page size
    /// </summary>
    public static int DefaultPageSizeValue => DefaultPageSize;

    /// <summary>
    /// Number of items to skip (for LINQ)
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Number of items to take (for LINQ)
    /// </summary>
    public int Take => PageSize;

    /// <summary>
    /// Offset for cursor-based pagination
    /// </summary>
    public int Offset => Skip;

    /// <summary>
    /// Limit for cursor-based pagination
    /// </summary>
    public int Limit => PageSize;

    /// <summary>
    /// Creates default pagination parameters
    /// </summary>
    public static PaginationParams Default => new();

    /// <summary>
    /// Creates pagination parameters with custom page size
    /// </summary>
    public static PaginationParams WithPageSize(int pageSize)
    {
        return new PaginationParams { PageSize = pageSize };
    }

    /// <summary>
    /// Creates pagination parameters with custom page number and size
    /// </summary>
    public static PaginationParams Create(int pageNumber, int pageSize)
    {
        return new PaginationParams { PageNumber = pageNumber, PageSize = pageSize };
    }

    /// <summary>
    /// Creates pagination parameters for first page
    /// </summary>
    public static PaginationParams FirstPage(int pageSize = DefaultPageSize)
    {
        return new PaginationParams { PageNumber = 1, PageSize = pageSize };
    }

    /// <summary>
    /// Creates pagination parameters for last page
    /// </summary>
    public static PaginationParams LastPage(int totalCount, int pageSize = DefaultPageSize)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        return new PaginationParams { PageNumber = Math.Max(1, totalPages), PageSize = pageSize };
    }

    /// <summary>
    /// Creates pagination parameters for next page
    /// </summary>
    public static PaginationParams NextPage(PaginationParams current, int totalCount)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / current.PageSize);
        var nextPageNumber = Math.Min(current.PageNumber + 1, totalPages);
        return new PaginationParams { PageNumber = nextPageNumber, PageSize = current.PageSize };
    }

    /// <summary>
    /// Creates pagination parameters for previous page
    /// </summary>
    public static PaginationParams PreviousPage(PaginationParams current)
    {
        var prevPageNumber = Math.Max(1, current.PageNumber - 1);
        return new PaginationParams { PageNumber = prevPageNumber, PageSize = current.PageSize };
    }

    /// <summary>
    /// Validates pagination parameters
    /// </summary>
    public bool IsValid()
    {
        return PageNumber >= 1 && 
               PageSize >= MinPageSize && 
               PageSize <= MaxPageSize;
    }

    /// <summary>
    /// Gets validation errors
    /// </summary>
    public IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();

        if (PageNumber < 1)
            errors.Add($"PageNumber must be >= 1, got {PageNumber}");

        if (PageSize < MinPageSize)
            errors.Add($"PageSize must be >= {MinPageSize}, got {PageSize}");

        if (PageSize > MaxPageSize)
            errors.Add($"PageSize must be <= {MaxPageSize}, got {PageSize}");

        return errors;
    }

    /// <summary>
    /// Converts to string representation
    /// </summary>
    public override string ToString()
    {
        return $"Page {PageNumber} of {PageSize} items";
    }

    /// <summary>
    /// Equality comparison
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is PaginationParams other && 
               PageNumber == other.PageNumber && 
               PageSize == other.PageSize;
    }

    /// <summary>
    /// Hash code
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(PageNumber, PageSize);
    }
}