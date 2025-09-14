using MyShop.Contracts.Common.Filtering;
using MyShop.Contracts.Common.Pagination;
using MyShop.Contracts.Common.Sorting;

namespace MyShop.Contracts.DTOs.Common;

/// <summary>
/// Unified query options containing pagination, sorting, filtering, and search
/// </summary>
public class QueryOptionsDto
{
    /// <summary>
    /// Pagination parameters
    /// </summary>
    public PaginationParams Pagination { get; set; } = PaginationParams.Default;

    /// <summary>
    /// Sorting options
    /// </summary>
    public List<SortDto> Sorting { get; set; } = new();

    /// <summary>
    /// Filtering options
    /// </summary>
    public List<FilterDto> Filtering { get; set; } = new();

    /// <summary>
    /// Search term
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Additional search options
    /// </summary>
    public Dictionary<string, object> AdditionalOptions { get; set; } = new();

    /// <summary>
    /// Creates default query options
    /// </summary>
    public static QueryOptionsDto Default => new();

    /// <summary>
    /// Creates query options with pagination
    /// </summary>
    public static QueryOptionsDto WithPagination(int pageNumber, int pageSize)
    {
        return new QueryOptionsDto
        {
            Pagination = new PaginationParams { PageNumber = pageNumber, PageSize = pageSize }
        };
    }

    /// <summary>
    /// Creates query options with search
    /// </summary>
    public static QueryOptionsDto WithSearch(string searchTerm)
    {
        return new QueryOptionsDto { Search = searchTerm };
    }

    /// <summary>
    /// Creates query options with sorting
    /// </summary>
    public static QueryOptionsDto WithSorting(params SortDto[] sorts)
    {
        return new QueryOptionsDto { Sorting = sorts.ToList() };
    }

    /// <summary>
    /// Creates query options with filtering
    /// </summary>
    public static QueryOptionsDto WithFiltering(params FilterDto[] filters)
    {
        return new QueryOptionsDto { Filtering = filters.ToList() };
    }

    /// <summary>
    /// Adds a sort option
    /// </summary>
    public QueryOptionsDto AddSort(SortDto sort)
    {
        Sorting.Add(sort);
        return this;
    }

    /// <summary>
    /// Adds a filter option
    /// </summary>
    public QueryOptionsDto AddFilter(FilterDto filter)
    {
        Filtering.Add(filter);
        return this;
    }

    /// <summary>
    /// Sets search term
    /// </summary>
    public QueryOptionsDto SetSearch(string searchTerm)
    {
        Search = searchTerm;
        return this;
    }

    /// <summary>
    /// Sets pagination
    /// </summary>
    public QueryOptionsDto SetPagination(int pageNumber, int pageSize)
    {
        Pagination = new PaginationParams { PageNumber = pageNumber, PageSize = pageSize };
        return this;
    }

    /// <summary>
    /// Adds additional option
    /// </summary>
    public QueryOptionsDto AddOption(string key, object value)
    {
        AdditionalOptions[key] = value;
        return this;
    }

    /// <summary>
    /// Checks if any query options are specified
    /// </summary>
    public bool HasOptions()
    {
        return !string.IsNullOrWhiteSpace(Search) ||
               Sorting.Count > 0 ||
               Filtering.Count > 0 ||
               AdditionalOptions.Count > 0 ||
               !Pagination.Equals(PaginationParams.Default);
    }

    /// <summary>
    /// Gets a summary of the query options
    /// </summary>
    public string GetSummary()
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(Search))
            parts.Add($"Search: '{Search}'");

        if (Sorting.Count > 0)
            parts.Add($"Sort: {string.Join(", ", Sorting.Select(s => s.ToString()))}");

        if (Filtering.Count > 0)
            parts.Add($"Filters: {Filtering.Count}");

        if (!Pagination.Equals(PaginationParams.Default))
            parts.Add($"Page: {Pagination.PageNumber}/{Pagination.PageSize}");

        return parts.Count > 0 ? string.Join(" | ", parts) : "No options";
    }
}