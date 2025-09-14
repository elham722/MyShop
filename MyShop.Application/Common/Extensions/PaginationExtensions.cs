namespace MyShop.Application.Common.Extensions;

public static class PaginationExtensions
{
    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take)
            .ToListAsync(cancellationToken);

        return MyShop.Contracts.Common.Pagination.PagedResult<T>.Create(items, totalCount, paginationParams);
    }

    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
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

        return MyShop.Contracts.Common.Pagination.PagedResult<TResult>.Create(items, totalCount, paginationParams);
    }

    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        IQueryable<T> countQuery,
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await countQuery.CountAsync(cancellationToken);
        
        var items = await query
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take)
            .ToListAsync(cancellationToken);

        return MyShop.Contracts.Common.Pagination.PagedResult<T>.Create(items, totalCount, paginationParams);
    }

    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
        this IQueryable<TSource> query,
        IQueryable<TSource> countQuery,
        PaginationParams paginationParams,
        Expression<Func<TSource, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await countQuery.CountAsync(cancellationToken);
        
        var items = await query
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return MyShop.Contracts.Common.Pagination.PagedResult<TResult>.Create(items, totalCount, paginationParams);
    }

    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<T>> ToPagedResultParallelAsync<T>(
        this IQueryable<T> query,
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        var countTask = query.CountAsync(cancellationToken);
        var itemsTask = query
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take)
            .ToListAsync(cancellationToken);

        await Task.WhenAll(countTask, itemsTask);

        return MyShop.Contracts.Common.Pagination.PagedResult<T>.Create(await itemsTask, await countTask, paginationParams);
    }

    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<TResult>> ToPagedResultParallelAsync<TSource, TResult>(
        this IQueryable<TSource> query,
        PaginationParams paginationParams,
        Expression<Func<TSource, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        var countTask = query.CountAsync(cancellationToken);
        var itemsTask = query
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take)
            .Select(selector)
            .ToListAsync(cancellationToken);

        await Task.WhenAll(countTask, itemsTask);

        return MyShop.Contracts.Common.Pagination.PagedResult<TResult>.Create(await itemsTask, await countTask, paginationParams);
    }

    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var pagination = PaginationParams.Create(pageNumber, pageSize);
        return await query.ToPagedResultAsync(pagination, cancellationToken);
    }

    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
        this IQueryable<TSource> query,
        int pageNumber,
        int pageSize,
        Expression<Func<TSource, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        var pagination = PaginationParams.Create(pageNumber, pageSize);
        return await query.ToPagedResultAsync(pagination, selector, cancellationToken);
    }

    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        CancellationToken cancellationToken = default)
    {
        return await query.ToPagedResultAsync(PaginationParams.Default, cancellationToken);
    }

    public static async Task<MyShop.Contracts.Common.Pagination.PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        return await query.ToPagedResultAsync(PaginationParams.Default, selector, cancellationToken);
    }
}