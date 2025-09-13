namespace MyShop.Contracts.Repositories.Query;
public interface IQueryBuilder<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Filtering

    IQueryBuilder<T, TId> Where(Expression<Func<T, bool>> predicate);

    IQueryBuilder<T, TId> Where(params Expression<Func<T, bool>>[] predicates);

    IQueryBuilder<T, TId> Where(ISpecification<T> specification);

    #endregion

    #region Includes

    IQueryBuilder<T, TId> Include(Expression<Func<T, object>> include);

    IQueryBuilder<T, TId> Include(params Expression<Func<T, object>>[] includes);

    #endregion

    #region Ordering

    IQueryBuilder<T, TId> OrderBy(Expression<Func<T, object>> orderBy, bool ascending = true);

    IQueryBuilder<T, TId> OrderBy(params (Expression<Func<T, object>> orderBy, bool ascending)[] orderBy);

    #endregion

    #region Paging

    IQueryBuilder<T, TId> Page(int pageNumber, int pageSize);

    #endregion

    #region Search Operations

    IQueryBuilder<T, TId> Search(string searchTerm);

    IQueryBuilder<T, TId> Search(string searchTerm, Expression<Func<T, object>>[] searchFields);

    IQueryBuilder<T, TId> Search(ISearchCriteria<T, TId> searchCriteria);

    #endregion

    #region Execution

    Task<T?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> ToListAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    Task<(IEnumerable<T> Items, int TotalCount)> ToPagedListAsync(CancellationToken cancellationToken = default);

    Task<SearchResult<T, TId>> ToSearchResultAsync(CancellationToken cancellationToken = default);

    #endregion
}
