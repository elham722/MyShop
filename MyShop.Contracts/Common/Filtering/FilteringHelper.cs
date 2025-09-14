namespace MyShop.Contracts.Common.Filtering;

public static class FilteringHelper
{
    public static List<FilterDto> CreateSingle(string field, FilterOperator @operator, string value = "", string? valueType = null)
    {
        return new List<FilterDto> { new FilterDto { Field = field, Operator = @operator, Value = value, ValueType = valueType } };
    }

    public static List<FilterDto> CreateMultiple(params FilterDto[] filters)
    {
        return filters.ToList();
    }

    public static List<FilterDto> CreateMultiple(IEnumerable<FilterDto> filters)
    {
        return filters.ToList();
    }

    public static bool IsValid(this IEnumerable<FilterDto> filters)
    {
        return filters.All(filter => filter.IsValid());
    }

    public static IEnumerable<FilterDto> GetInvalidFilters(this IEnumerable<FilterDto> filters)
    {
        return filters.Where(filter => !filter.IsValid());
    }

    public static IEnumerable<FilterDto> GetValidFilters(this IEnumerable<FilterDto> filters)
    {
        return filters.Where(filter => filter.IsValid());
    }

    public static List<FilterDto> Empty()
    {
        return new List<FilterDto>();
    }

    public static FilterDto Equals(string field, string value, string? valueType = null)
    {
        return FilterDto.Equals(field, value, valueType);
    }

    public static FilterDto Contains(string field, string value, string? valueType = null)
    {
        return FilterDto.Contains(field, value, valueType);
    }

    public static FilterDto GreaterThan(string field, string value, string? valueType = null)
    {
        return FilterDto.GreaterThan(field, value, valueType);
    }

    public static FilterDto LessThan(string field, string value, string? valueType = null)
    {
        return FilterDto.LessThan(field, value, valueType);
    }

    public static FilterDto In(string field, params string[] values)
    {
        return FilterDto.In(field, values);
    }

    public static FilterDto IsNull(string field)
    {
        return FilterDto.IsNull(field);
    }

    public static FilterDto IsNotNull(string field)
    {
        return FilterDto.IsNotNull(field);
    }
}