using System.Linq.Dynamic.Core;
using MyShop.Contracts.Common.Filtering;

namespace MyShop.Application.Common.Extensions;

/// <summary>
/// Extension methods for filtering
/// </summary>
public static class FilteringExtensions
{
    /// <summary>
    /// Applies filtering to an IQueryable based on FilterDto
    /// </summary>
    /// <typeparam name="T">The type of entities</typeparam>
    /// <param name="query">The queryable to filter</param>
    /// <param name="filterDto">Filter parameters</param>
    /// <returns>The filtered queryable</returns>
    public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, FilterDto filterDto)
    {
        if (filterDto == null || !filterDto.IsValid())
            return query;

        var (expression, values) = BuildFilterExpression(filterDto);
        return query.Where(expression, values);
    }

    /// <summary>
    /// Applies filtering to an IQueryable based on multiple FilterDto
    /// </summary>
    /// <typeparam name="T">The type of entities</typeparam>
    /// <param name="query">The queryable to filter</param>
    /// <param name="filterDtos">Collection of filter parameters</param>
    /// <returns>The filtered queryable</returns>
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

    /// <summary>
    /// Applies filtering to an IQueryable based on field, operator and value
    /// </summary>
    /// <typeparam name="T">The type of entities</typeparam>
    /// <param name="query">The queryable to filter</param>
    /// <param name="field">Field name to filter by</param>
    /// <param name="operator">Filter operator</param>
    /// <param name="value">Filter value</param>
    /// <returns>The filtered queryable</returns>
    public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, string field, FilterOperator @operator, string value = "")
    {
        var filterDto = new FilterDto { Field = field, Operator = @operator, Value = value };
        return query.ApplyFiltering(filterDto);
    }

    /// <summary>
    /// Builds a dynamic LINQ expression for filtering
    /// </summary>
    /// <param name="filterDto">Filter parameters</param>
    /// <returns>Tuple containing expression string and parameter values</returns>
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

    /// <summary>
    /// Builds an IN expression for filtering
    /// </summary>
    /// <param name="filterDto">Filter parameters</param>
    /// <returns>Tuple containing expression string and parameter values</returns>
    private static (string expression, object[] values) BuildInExpression(FilterDto filterDto)
    {
        var values = filterDto.Values.ToArray();
        var placeholders = values.Select((_, index) => $"@{index}").ToArray();
        return ($"{filterDto.Field}.In({string.Join(", ", placeholders)})", values.Cast<object>().ToArray());
    }

    /// <summary>
    /// Builds a NOT IN expression for filtering
    /// </summary>
    /// <param name="filterDto">Filter parameters</param>
    /// <returns>Tuple containing expression string and parameter values</returns>
    private static (string expression, object[] values) BuildNotInExpression(FilterDto filterDto)
    {
        var values = filterDto.Values.ToArray();
        var placeholders = values.Select((_, index) => $"@{index}").ToArray();
        return ($"!{filterDto.Field}.In({string.Join(", ", placeholders)})", values.Cast<object>().ToArray());
    }
}