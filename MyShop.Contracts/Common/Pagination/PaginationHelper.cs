namespace MyShop.Contracts.Common.Pagination;

/// <summary>
/// Utility class for pagination operations
/// </summary>
public static class PaginationHelper
{
    /// <summary>
    /// Calculates total pages from total count and page size
    /// </summary>
    public static int CalculateTotalPages(int totalCount, int pageSize)
    {
        if (pageSize <= 0) return 0;
        return (int)Math.Ceiling((double)totalCount / pageSize);
    }

    /// <summary>
    /// Calculates skip value for pagination
    /// </summary>
    public static int CalculateSkip(int pageNumber, int pageSize)
    {
        return Math.Max(0, (pageNumber - 1) * pageSize);
    }

    /// <summary>
    /// Calculates take value for pagination
    /// </summary>
    public static int CalculateTake(int pageNumber, int pageSize, int totalCount)
    {
        var skip = CalculateSkip(pageNumber, pageSize);
        return Math.Min(pageSize, Math.Max(0, totalCount - skip));
    }

    /// <summary>
    /// Validates page number
    /// </summary>
    public static bool IsValidPageNumber(int pageNumber, int totalPages)
    {
        return pageNumber >= 1 && pageNumber <= totalPages;
    }

    /// <summary>
    /// Clamps page number to valid range
    /// </summary>
    public static int ClampPageNumber(int pageNumber, int totalPages)
    {
        return Math.Max(1, Math.Min(pageNumber, totalPages));
    }

    /// <summary>
    /// Gets page range for UI pagination
    /// </summary>
    public static IEnumerable<int> GetPageRange(int currentPage, int totalPages, int maxPages = 10)
    {
        if (totalPages <= 0) return Enumerable.Empty<int>();

        var start = Math.Max(1, currentPage - maxPages / 2);
        var end = Math.Min(totalPages, start + maxPages - 1);

        if (end - start + 1 < maxPages)
        {
            start = Math.Max(1, end - maxPages + 1);
        }

        return Enumerable.Range(start, end - start + 1);
    }

    /// <summary>
    /// Gets start index for current page
    /// </summary>
    public static int GetStartIndex(int pageNumber, int pageSize)
    {
        return (pageNumber - 1) * pageSize + 1;
    }

    /// <summary>
    /// Gets end index for current page
    /// </summary>
    public static int GetEndIndex(int pageNumber, int pageSize, int totalCount)
    {
        return Math.Min(pageNumber * pageSize, totalCount);
    }

    /// <summary>
    /// Calculates load percentage
    /// </summary>
    public static double CalculateLoadPercentage(int currentCount, int totalCount)
    {
        if (totalCount <= 0) return 0;
        return (double)currentCount / totalCount * 100;
    }

    /// <summary>
    /// Creates pagination info
    /// </summary>
    public static PaginationInfo CreatePaginationInfo(int pageNumber, int pageSize, int totalCount)
    {
        var totalPages = CalculateTotalPages(totalCount, pageSize);
        var startIndex = GetStartIndex(pageNumber, pageSize);
        var endIndex = GetEndIndex(pageNumber, pageSize, totalCount);
        var loadPercentage = CalculateLoadPercentage(endIndex - startIndex + 1, totalCount);

        return new PaginationInfo
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages,
            IsFirstPage = pageNumber == 1,
            IsLastPage = pageNumber == totalPages,
            StartIndex = startIndex,
            EndIndex = endIndex,
            LoadPercentage = loadPercentage
        };
    }

    /// <summary>
    /// Creates pagination navigation
    /// </summary>
    public static PaginationNavigation CreateNavigation(int pageNumber, int totalPages)
    {
        return new PaginationNavigation
        {
            FirstPage = 1,
            LastPage = totalPages,
            PreviousPage = pageNumber > 1 ? pageNumber - 1 : null,
            NextPage = pageNumber < totalPages ? pageNumber + 1 : null,
            CurrentPage = pageNumber
        };
    }

    /// <summary>
    /// Gets next page number
    /// </summary>
    public static int? GetNextPageNumber(int currentPage, int totalPages)
    {
        return currentPage < totalPages ? currentPage + 1 : null;
    }

    /// <summary>
    /// Gets previous page number
    /// </summary>
    public static int? GetPreviousPageNumber(int currentPage)
    {
        return currentPage > 1 ? currentPage - 1 : null;
    }

    /// <summary>
    /// Checks if page is valid
    /// </summary>
    public static bool IsValidPage(int pageNumber, int totalPages)
    {
        return pageNumber >= 1 && pageNumber <= totalPages;
    }

    /// <summary>
    /// Gets page size options for UI
    /// </summary>
    public static IEnumerable<int> GetPageSizeOptions(int currentPageSize = 20)
    {
        var options = new[] { 10, 20, 50, 100, 200, 500 };
        return options.Where(size => size <= PaginationParams.MaxAllowedPageSize)
                     .OrderBy(size => size);
    }

    /// <summary>
    /// Calculates optimal page size based on total count
    /// </summary>
    public static int CalculateOptimalPageSize(int totalCount, int maxPageSize = 100)
    {
        if (totalCount <= 10) return 10;
        if (totalCount <= 50) return 20;
        if (totalCount <= 200) return 50;
        if (totalCount <= 1000) return 100;
        return Math.Min(maxPageSize, 200);
    }

    /// <summary>
    /// Creates summary string
    /// </summary>
    public static string CreateSummary(int pageNumber, int pageSize, int totalCount)
    {
        if (totalCount == 0) return "No items found";

        var startIndex = GetStartIndex(pageNumber, pageSize);
        var endIndex = GetEndIndex(pageNumber, pageSize, totalCount);
        var totalPages = CalculateTotalPages(totalCount, pageSize);

        return $"Showing {startIndex}-{endIndex} of {totalCount} items (Page {pageNumber} of {totalPages})";
    }

    /// <summary>
    /// Validates pagination parameters
    /// </summary>
    public static bool ValidatePagination(int pageNumber, int pageSize, int totalCount)
    {
        return pageNumber >= 1 && 
               pageSize >= PaginationParams.MinAllowedPageSize && 
               pageSize <= PaginationParams.MaxAllowedPageSize &&
               totalCount >= 0;
    }

    /// <summary>
    /// Gets validation errors for pagination
    /// </summary>
    public static IEnumerable<string> GetValidationErrors(int pageNumber, int pageSize, int totalCount)
    {
        var errors = new List<string>();

        if (pageNumber < 1)
            errors.Add($"Page number must be >= 1, got {pageNumber}");

        if (pageSize < PaginationParams.MinAllowedPageSize)
            errors.Add($"Page size must be >= {PaginationParams.MinAllowedPageSize}, got {pageSize}");

        if (pageSize > PaginationParams.MaxAllowedPageSize)
            errors.Add($"Page size must be <= {PaginationParams.MaxAllowedPageSize}, got {pageSize}");

        if (totalCount < 0)
            errors.Add($"Total count must be >= 0, got {totalCount}");

        return errors;
    }
}