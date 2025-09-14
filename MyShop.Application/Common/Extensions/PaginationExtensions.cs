namespace MyShop.Application.Common.Extensions;

public static class PaginationExtensions
{
    public static async Task<Contracts.Common.Pagination.PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take)
            .ToListAsync(cancellationToken);

        return new Contracts.Common.Pagination.PagedResult<T>(items, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public static async Task<Contracts.Common.Pagination.PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
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

        return new Contracts.Common.Pagination.PagedResult<TResult>(items, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }
}