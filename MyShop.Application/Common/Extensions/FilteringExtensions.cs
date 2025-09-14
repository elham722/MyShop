namespace MyShop.Application.Common.Extensions;

public static class FilteringExtensions
{
    public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, FilterDto filterDto)
    {
        if (filterDto == null || !filterDto.IsValid())
            return query;

        var (expression, values) = BuildFilterExpression(filterDto);
        return query.Where(expression, values);
    }

    public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, FilterDtoCollection filterDtos)
    {
        if (filterDtos == null || !filterDtos.IsValid())
            return query;

        foreach (var filterDto in filterDtos)
        {
            query = query.ApplyFiltering(filterDto);
        }

        return query;
    }

    public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, string field, FilterOperator @operator, string value = "")
    {
        var filterDto = new FilterDto { Field = field, Operator = @operator, Value = value };
        return query.ApplyFiltering(filterDto);
    }

    private static (string expression, object[] values) BuildFilterExpression(FilterDto filterDto)
    {
        var parsedValue = filterDto.ParseValue();
        
        return filterDto.Operator switch
        {
            FilterOperator.Equals => ($"{filterDto.Field} == @0", new[] { parsedValue! }),
            FilterOperator.NotEquals => ($"{filterDto.Field} != @0", new[] { parsedValue! }),
            FilterOperator.Contains => ($"{filterDto.Field}.Contains(@0)", new[] { parsedValue! }),
            FilterOperator.NotContains => ($"!{filterDto.Field}.Contains(@0)", new[] { parsedValue! }),
            FilterOperator.StartsWith => ($"{filterDto.Field}.StartsWith(@0)", new[] { parsedValue! }),
            FilterOperator.EndsWith => ($"{filterDto.Field}.EndsWith(@0)", new[] { parsedValue! }),
            FilterOperator.GreaterThan => ($"{filterDto.Field} > @0", new[] { parsedValue! }),
            FilterOperator.GreaterThanOrEqual => ($"{filterDto.Field} >= @0", new[] { parsedValue! }),
            FilterOperator.LessThan => ($"{filterDto.Field} < @0", new[] { parsedValue! }),
            FilterOperator.LessThanOrEqual => ($"{filterDto.Field} <= @0", new[] { parsedValue! }),
            FilterOperator.In => BuildInExpression(filterDto),
            FilterOperator.NotIn => BuildNotInExpression(filterDto),
            FilterOperator.IsNull => ($"{filterDto.Field} == null", Array.Empty<object>()),
            FilterOperator.IsNotNull => ($"{filterDto.Field} != null", Array.Empty<object>()),
            FilterOperator.IsEmpty => ($"{filterDto.Field} == \"\"", Array.Empty<object>()),
            FilterOperator.IsNotEmpty => ($"{filterDto.Field} != \"\"", Array.Empty<object>()),
            _ => ("true", Array.Empty<object>())
        };
    }

    private static (string expression, object[] values) BuildInExpression(FilterDto filterDto)
    {
        var values = filterDto.Values.ToArray();
        var placeholders = values.Select((_, index) => $"@{index}").ToArray();
        return ($"{filterDto.Field}.In({string.Join(", ", placeholders)})", values.Cast<object>().ToArray());
    }

    private static (string expression, object[] values) BuildNotInExpression(FilterDto filterDto)
    {
        var values = filterDto.Values.ToArray();
        var placeholders = values.Select((_, index) => $"@{index}").ToArray();
        return ($"!{filterDto.Field}.In({string.Join(", ", placeholders)})", values.Cast<object>().ToArray());
    }
}