namespace MyShop.Contracts.Common.Pagination;

/// <summary>
/// Parameters for pagination
/// </summary>
public class PaginationParams
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

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
        set => _pageSize = value < 1 ? DefaultPageSize : Math.Min(value, MaxPageSize);
    }

    /// <summary>
    /// Maximum allowed page size
    /// </summary>
    public static int MaxAllowedPageSize => MaxPageSize;

    /// <summary>
    /// Calculates the number of items to skip
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Calculates the number of items to take
    /// </summary>
    public int Take => PageSize;

    /// <summary>
    /// Creates default pagination parameters
    /// </summary>
    public static PaginationParams Default => new();
}