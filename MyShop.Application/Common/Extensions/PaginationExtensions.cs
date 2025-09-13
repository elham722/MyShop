using Microsoft.EntityFrameworkCore;
using MyShop.Contracts.Common.Pagination;

namespace MyShop.Application.Common.Extensions;

/// <summary>
/// Extension methods for pagination
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Applies pagination to an IQueryable and returns a PagedResult
    /// </summary>
    /// <typeparam name="T">The type of entities</typeparam>
    /// <param name="query">The queryable to paginate</param>
    /// <param name="paginationParams">Pagination parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paged result</returns>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    /// <summary>
    /// Applies pagination to an IQueryable and returns a PagedResult with a selector
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="query">The queryable to paginate</param>
    /// <param name="paginationParams">Pagination parameters</param>
    /// <param name="selector">Selector function to transform items</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paged result</returns>
    public static async Task<PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
        this IQueryable<TSource> query,
        PaginationParams paginationParams,
        Expression<Func<TSource, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return new PagedResult<TResult>(items, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }
}