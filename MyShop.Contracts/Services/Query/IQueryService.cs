namespace MyShop.Contracts.Services.Query;
public interface IQueryService<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Basic Query Operations

    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(TId id, Expression<Func<T, object>>[] includes, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>[] includes, CancellationToken cancellationToken = default);

    #endregion

    #region Search Operations

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] includes, CancellationToken cancellationToken = default);

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion

    #region Existence and Counting

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion

    #region Paging Operations

    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    #endregion

    #region Business Queries

    Task<TResult> ExecuteBusinessQueryAsync<TResult>(Func<IQueryable<T>, Task<TResult>> query, CancellationToken cancellationToken = default);

    Task<TResult> ExecuteBusinessQueryAsync<TResult>(Func<IQueryable<T>, Task<TResult>> query, Expression<Func<T, object>>[] includes, CancellationToken cancellationToken = default);

    #endregion
}