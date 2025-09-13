using System.Linq.Dynamic.Core;
using MyShop.Contracts.Common.Sorting;

namespace MyShop.Application.Common.Extensions;

/// <summary>
/// Extension methods for sorting
/// </summary>
public static class SortingExtensions
{
    /// <summary>
    /// Applies sorting to an IQueryable based on SortDto
    /// </summary>
    /// <typeparam name="T">The type of entities</typeparam>
    /// <param name="query">The queryable to sort</param>
    /// <param name="sortDto">Sort parameters</param>
    /// <returns>The sorted queryable</returns>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, SortDto sortDto)
    {
        if (sortDto == null || !sortDto.IsValid())
            return query;

        var direction = sortDto.IsAscending ? "asc" : "desc";
        var orderByExpression = $"{sortDto.Field} {direction}";
        
        return query.OrderBy(orderByExpression);
    }

    /// <summary>
    /// Applies sorting to an IQueryable based on multiple SortDto
    /// </summary>
    /// <typeparam name="T">The type of entities</typeparam>
    /// <param name="query">The queryable to sort</param>
    /// <param name="sortDtos">Collection of sort parameters</param>
    /// <returns>The sorted queryable</returns>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, SortDtoCollection sortDtos)
    {
        if (sortDtos == null || !sortDtos.IsValid())
            return query;

        IQueryable<T> sortedQuery = query;
        bool isFirst = true;

        foreach (var sortDto in sortDtos)
        {
            if (!sortDto.IsValid())
                continue;

            var direction = sortDto.IsAscending ? "asc" : "desc";
            var orderByExpression = $"{sortDto.Field} {direction}";

            if (isFirst)
            {
                sortedQuery = query.OrderBy(orderByExpression);
                isFirst = false;
            }
            else
            {
                sortedQuery = ((IOrderedQueryable<T>)sortedQuery).ThenBy(orderByExpression);
            }
        }

        return sortedQuery;
    }

    /// <summary>
    /// Applies sorting to an IQueryable based on field name and direction
    /// </summary>
    /// <typeparam name="T">The type of entities</typeparam>
    /// <param name="query">The queryable to sort</param>
    /// <param name="field">Field name to sort by</param>
    /// <param name="direction">Sort direction (asc or desc)</param>
    /// <returns>The sorted queryable</returns>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string field, string direction = "asc")
    {
        if (string.IsNullOrWhiteSpace(field))
            return query;

        var sortDto = new SortDto { Field = field, Direction = direction };
        return query.ApplySorting(sortDto);
    }

    /// <summary>
    /// Applies sorting to an IQueryable based on field name and ascending flag
    /// </summary>
    /// <typeparam name="T">The type of entities</typeparam>
    /// <param name="query">The queryable to sort</param>
    /// <param name="field">Field name to sort by</param>
    /// <param name="ascending">True for ascending, false for descending</param>
    /// <returns>The sorted queryable</returns>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string field, bool ascending = true)
    {
        var direction = ascending ? "asc" : "desc";
        return query.ApplySorting(field, direction);
    }
}