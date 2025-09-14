namespace MyShop.Contracts.Common.Pagination;

public class PaginationInfo
{
    #region Core Properties

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    #endregion

    #region Calculated Properties

    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    public bool HasNextPage => PageNumber < TotalPages;

    public bool HasPreviousPage => PageNumber > 1;

    public int CurrentPageItemCount
    {
        get
        {
            if (PageNumber < 1 || PageSize < 1) return 0;
            
            var startIndex = (PageNumber - 1) * PageSize;
            var endIndex = startIndex + PageSize;
            
            return Math.Max(0, Math.Min(PageSize, TotalCount - startIndex));
        }
    }

    public int StartIndex => Math.Max(0, (PageNumber - 1) * PageSize);

    public int EndIndex => Math.Min(TotalCount - 1, StartIndex + PageSize - 1);

    #endregion

    #region Constructors

    public PaginationInfo() { }

    public PaginationInfo(int pageNumber, int pageSize, int totalCount)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    #endregion

    #region Factory Methods

    public static PaginationInfo FirstPage(int pageSize, int totalCount)
    {
        return new PaginationInfo(1, pageSize, totalCount);
    }

    public static PaginationInfo LastPage(int pageSize, int totalCount)
    {
        var totalPages = pageSize > 0 ? (int)Math.Ceiling((double)totalCount / pageSize) : 0;
        return new PaginationInfo(totalPages, pageSize, totalCount);
    }

    public static PaginationInfo ForPage(int pageNumber, int pageSize, int totalCount)
    {
        return new PaginationInfo(pageNumber, pageSize, totalCount);
    }

    #endregion

    #region Validation Methods

    public bool IsValid()
    {
        return PageNumber >= 1 && 
               PageSize >= 1 && 
               TotalCount >= 0 &&
               PageNumber <= TotalPages;
    }

    public IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();

        if (PageNumber < 1)
            errors.Add("Page number must be greater than 0");

        if (PageSize < 1)
            errors.Add("Page size must be greater than 0");

        if (TotalCount < 0)
            errors.Add("Total count cannot be negative");

        if (PageNumber > TotalPages && TotalPages > 0)
            errors.Add($"Page number {PageNumber} exceeds total pages {TotalPages}");

        return errors;
    }

    #endregion

    #region Helper Methods

    public int? GetNextPageNumber()
    {
        return HasNextPage ? PageNumber + 1 : null;
    }

    public int? GetPreviousPageNumber()
    {
        return HasPreviousPage ? PageNumber - 1 : null;
    }

    public IEnumerable<int> GetPageRange(int rangeSize = 5)
    {
        var startPage = Math.Max(1, PageNumber - rangeSize / 2);
        var endPage = Math.Min(TotalPages, startPage + rangeSize - 1);
        
        // Adjust start page if we're near the end
        if (endPage - startPage + 1 < rangeSize)
        {
            startPage = Math.Max(1, endPage - rangeSize + 1);
        }

        return Enumerable.Range(startPage, endPage - startPage + 1);
    }

    public string ToSummaryString()
    {
        return $"Page {PageNumber}/{TotalPages} (Items {StartIndex + 1}-{EndIndex + 1} of {TotalCount})";
    }

    #endregion

    #region Overrides

    public override string ToString()
    {
        return $"PaginationInfo(Page={PageNumber}, Size={PageSize}, Total={TotalCount})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is PaginationInfo other)
        {
            return PageNumber == other.PageNumber &&
                   PageSize == other.PageSize &&
                   TotalCount == other.TotalCount;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PageNumber, PageSize, TotalCount);
    }

    #endregion
}