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

        var expression = BuildFilterExpression(filterDto);
        return query.Where(expression);
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
    /// <returns>Dynamic LINQ expression string</returns>
    private static string BuildFilterExpression(FilterDto filterDto)
    {
        return filterDto.Operator switch
        {
            FilterOperator.Equals => $"{filterDto.Field} == \"{filterDto.Value}\"",
            FilterOperator.NotEquals => $"{filterDto.Field} != \"{filterDto.Value}\"",
            FilterOperator.Contains => $"{filterDto.Field}.Contains(\"{filterDto.Value}\")",
            FilterOperator.NotContains => $"!{filterDto.Field}.Contains(\"{filterDto.Value}\")",
            FilterOperator.StartsWith => $"{filterDto.Field}.StartsWith(\"{filterDto.Value}\")",
            FilterOperator.EndsWith => $"{filterDto.Field}.EndsWith(\"{filterDto.Value}\")",
            FilterOperator.GreaterThan => $"{filterDto.Field} > \"{filterDto.Value}\"",
            FilterOperator.GreaterThanOrEqual => $"{filterDto.Field} >= \"{filterDto.Value}\"",
            FilterOperator.LessThan => $"{filterDto.Field} < \"{filterDto.Value}\"",
            FilterOperator.LessThanOrEqual => $"{filterDto.Field} <= \"{filterDto.Value}\"",
            FilterOperator.In => BuildInExpression(filterDto),
            FilterOperator.NotIn => BuildNotInExpression(filterDto),
            FilterOperator.IsNull => $"{filterDto.Field} == null",
            FilterOperator.IsNotNull => $"{filterDto.Field} != null",
            FilterOperator.IsEmpty => $"{filterDto.Field} == \"\"",
            FilterOperator.IsNotEmpty => $"{filterDto.Field} != \"\"",
            _ => "true"
        };
    }

    /// <summary>
    /// Builds an IN expression for filtering
    /// </summary>
    /// <param name="filterDto">Filter parameters</param>
    /// <returns>Dynamic LINQ expression string</returns>
    private static string BuildInExpression(FilterDto filterDto)
    {
        var values = filterDto.Values.Select(v => $"\"{v}\"").ToArray();
        return $"{filterDto.Field}.In({string.Join(", ", values)})";
    }

    /// <summary>
    /// Builds a NOT IN expression for filtering
    /// </summary>
    /// <param name="filterDto">Filter parameters</param>
    /// <returns>Dynamic LINQ expression string</returns>
    private static string BuildNotInExpression(FilterDto filterDto)
    {
        var values = filterDto.Values.Select(v => $"\"{v}\"").ToArray();
        return $"!{filterDto.Field}.In({string.Join(", ", values)})";
    }
}