namespace MyShop.Contracts.Common.Pagination;

public class PaginationParams
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 20;
    private const int MinPageSize = 1;
    private const int MaxPageSize = 1000;

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
        set => _pageSize = value < MinPageSize ? DefaultPageSize : Math.Min(value, MaxPageSize);
    }

    public static int MaxAllowedPageSize => MaxPageSize;

    public static int MinAllowedPageSize => MinPageSize;

    public static int DefaultPageSizeValue => DefaultPageSize;

    public int Skip => (PageNumber - 1) * PageSize;

    public int Take => PageSize;

    public int Offset => Skip;

    public int Limit => PageSize;

    public static PaginationParams Default => new();

    public static PaginationParams WithPageSize(int pageSize)
    {
        return new PaginationParams { PageSize = pageSize };
    }

    public static PaginationParams Create(int pageNumber, int pageSize)
    {
        return new PaginationParams { PageNumber = pageNumber, PageSize = pageSize };
    }

    public static PaginationParams FirstPage(int pageSize = DefaultPageSize)
    {
        return new PaginationParams { PageNumber = 1, PageSize = pageSize };
    }

    public static PaginationParams LastPage(int totalCount, int pageSize = DefaultPageSize)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        return new PaginationParams { PageNumber = Math.Max(1, totalPages), PageSize = pageSize };
    }

    public static PaginationParams NextPage(PaginationParams current, int totalCount)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / current.PageSize);
        var nextPageNumber = Math.Min(current.PageNumber + 1, totalPages);
        return new PaginationParams { PageNumber = nextPageNumber, PageSize = current.PageSize };
    }

    public static PaginationParams PreviousPage(PaginationParams current)
    {
        var prevPageNumber = Math.Max(1, current.PageNumber - 1);
        return new PaginationParams { PageNumber = prevPageNumber, PageSize = current.PageSize };
    }

    public bool IsValid()
    {
        return PageNumber >= 1 && 
               PageSize >= MinPageSize && 
               PageSize <= MaxPageSize;
    }

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

    public override string ToString()
    {
        return $"Page {PageNumber} of {PageSize} items";
    }

    public override bool Equals(object? obj)
    {
        return obj is PaginationParams other && 
               PageNumber == other.PageNumber && 
               PageSize == other.PageSize;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PageNumber, PageSize);
    }
}