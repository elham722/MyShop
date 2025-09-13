namespace MyShop.Contracts.Common.Pagination;

public class PaginationParams
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private int _pageNumber = DefaultPageNumber;
    private int _pageSize = DefaultPageSize;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? DefaultPageNumber : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? DefaultPageSize : Math.Min(value, MaxPageSize);
    }

    public static int MaxAllowedPageSize => MaxPageSize;

    public int Skip => (PageNumber - 1) * PageSize;

    public int Take => PageSize;

    public static PaginationParams Default => new();
}