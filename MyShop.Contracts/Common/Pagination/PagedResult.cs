namespace MyShop.Contracts.Common.Pagination;

/// <summary>
/// Enhanced paged result with comprehensive metadata
/// </summary>
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public bool IsEmpty => Items.Count == 0;
    public bool IsFirstPage => PageNumber == 1;
    public bool IsLastPage => PageNumber == TotalPages;
    public int StartIndex => (PageNumber - 1) * PageSize + 1;
    public int EndIndex => Math.Min(PageNumber * PageSize, TotalCount);
    public double LoadPercentage => TotalCount > 0 ? (double)Items.Count / TotalCount * 100 : 0;

    public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items.ToList().AsReadOnly();
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    /// <summary>
    /// Creates empty paged result
    /// </summary>
    public static PagedResult<T> Empty(int pageNumber = 1, int pageSize = 20)
    {
        return new PagedResult<T>(Array.Empty<T>(), 0, pageNumber, pageSize);
    }

    /// <summary>
    /// Creates paged result from items
    /// </summary>
    public static PagedResult<T> Create(IEnumerable<T> items, int totalCount, PaginationParams pagination)
    {
        return new PagedResult<T>(items, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    /// <summary>
    /// Creates paged result with single page
    /// </summary>
    public static PagedResult<T> SinglePage(IEnumerable<T> items)
    {
        var itemsList = items.ToList();
        return new PagedResult<T>(itemsList, itemsList.Count, 1, itemsList.Count);
    }

    /// <summary>
    /// Creates paged result with all items
    /// </summary>
    public static PagedResult<T> AllItems(IEnumerable<T> items, int pageSize = 20)
    {
        var itemsList = items.ToList();
        return new PagedResult<T>(itemsList, itemsList.Count, 1, pageSize);
    }

    /// <summary>
    /// Gets pagination metadata
    /// </summary>
    public PaginationInfo GetPaginationInfo()
    {
        return new PaginationInfo
        {
            PageNumber = PageNumber,
            PageSize = PageSize,
            TotalCount = TotalCount,
            TotalPages = TotalPages,
            HasPreviousPage = HasPreviousPage,
            HasNextPage = HasNextPage,
            IsFirstPage = IsFirstPage,
            IsLastPage = IsLastPage,
            StartIndex = StartIndex,
            EndIndex = EndIndex,
            LoadPercentage = LoadPercentage
        };
    }

    /// <summary>
    /// Gets navigation links
    /// </summary>
    public PaginationNavigation GetNavigation()
    {
        return new PaginationNavigation
        {
            FirstPage = 1,
            LastPage = TotalPages,
            PreviousPage = HasPreviousPage ? PageNumber - 1 : null,
            NextPage = HasNextPage ? PageNumber + 1 : null,
            CurrentPage = PageNumber
        };
    }

    /// <summary>
    /// Gets page range for UI
    /// </summary>
    public IEnumerable<int> GetPageRange(int maxPages = 10)
    {
        var start = Math.Max(1, PageNumber - maxPages / 2);
        var end = Math.Min(TotalPages, start + maxPages - 1);
        
        if (end - start + 1 < maxPages)
        {
            start = Math.Max(1, end - maxPages + 1);
        }

        return Enumerable.Range(start, end - start + 1);
    }

    /// <summary>
    /// Converts to different page size
    /// </summary>
    public PagedResult<T> ToPageSize(int newPageSize)
    {
        var newPageNumber = (int)Math.Ceiling((double)StartIndex / newPageSize);
        return new PagedResult<T>(Items, TotalCount, newPageNumber, newPageSize);
    }

    /// <summary>
    /// Maps items to different type
    /// </summary>
    public PagedResult<TResult> Map<TResult>(Func<T, TResult> selector)
    {
        var mappedItems = Items.Select(selector).ToList();
        return new PagedResult<TResult>(mappedItems, TotalCount, PageNumber, PageSize);
    }

    /// <summary>
    /// Gets summary string
    /// </summary>
    public string GetSummary()
    {
        if (IsEmpty)
            return "No items found";

        return $"Showing {StartIndex}-{EndIndex} of {TotalCount} items (Page {PageNumber} of {TotalPages})";
    }

    /// <summary>
    /// Converts to string representation
    /// </summary>
    public override string ToString()
    {
        return GetSummary();
    }
}
