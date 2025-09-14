namespace MyShop.Contracts.Common.Filtering;

/// <summary>
/// Helper class for creating and managing filter collections
/// </summary>
public static class FilteringHelper
{
    /// <summary>
    /// Creates a single filter
    /// </summary>
    public static List<FilterDto> CreateSingle(string field, FilterOperator @operator, string value = "", string? valueType = null)
    {
        return new List<FilterDto> { new FilterDto { Field = field, Operator = @operator, Value = value, ValueType = valueType } };
    }

    /// <summary>
    /// Creates multiple filters from array
    /// </summary>
    public static List<FilterDto> CreateMultiple(params FilterDto[] filters)
    {
        return filters.ToList();
    }

    /// <summary>
    /// Creates multiple filters from collection
    /// </summary>
    public static List<FilterDto> CreateMultiple(IEnumerable<FilterDto> filters)
    {
        return filters.ToList();
    }

    /// <summary>
    /// Validates all filters in the collection
    /// </summary>
    public static bool IsValid(this IEnumerable<FilterDto> filters)
    {
        return filters.All(filter => filter.IsValid());
    }

    /// <summary>
    /// Gets invalid filters from the collection
    /// </summary>
    public static IEnumerable<FilterDto> GetInvalidFilters(this IEnumerable<FilterDto> filters)
    {
        return filters.Where(filter => !filter.IsValid());
    }

    /// <summary>
    /// Gets valid filters from the collection
    /// </summary>
    public static IEnumerable<FilterDto> GetValidFilters(this IEnumerable<FilterDto> filters)
    {
        return filters.Where(filter => filter.IsValid());
    }

    /// <summary>
    /// Creates an empty filter collection
    /// </summary>
    public static List<FilterDto> Empty()
    {
        return new List<FilterDto>();
    }

    /// <summary>
    /// Creates a filter for string equality
    /// </summary>
    public static FilterDto Equals(string field, string value, string? valueType = null)
    {
        return FilterDto.Equals(field, value, valueType);
    }

    /// <summary>
    /// Creates a filter for string contains
    /// </summary>
    public static FilterDto Contains(string field, string value, string? valueType = null)
    {
        return FilterDto.Contains(field, value, valueType);
    }

    /// <summary>
    /// Creates a filter for numeric greater than
    /// </summary>
    public static FilterDto GreaterThan(string field, string value, string? valueType = null)
    {
        return FilterDto.GreaterThan(field, value, valueType);
    }

    /// <summary>
    /// Creates a filter for numeric less than
    /// </summary>
    public static FilterDto LessThan(string field, string value, string? valueType = null)
    {
        return FilterDto.LessThan(field, value, valueType);
    }

    /// <summary>
    /// Creates a filter for IN operation
    /// </summary>
    public static FilterDto In(string field, params string[] values)
    {
        return FilterDto.In(field, values);
    }

    /// <summary>
    /// Creates a filter for null check
    /// </summary>
    public static FilterDto IsNull(string field)
    {
        return FilterDto.IsNull(field);
    }

    /// <summary>
    /// Creates a filter for not null check
    /// </summary>
    public static FilterDto IsNotNull(string field)
    {
        return FilterDto.IsNotNull(field);
    }
}