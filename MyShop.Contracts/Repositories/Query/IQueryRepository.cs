namespace MyShop.Contracts.Repositories.Query;
public interface IQueryRepository<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
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

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] includes, CancellationToken cancellationToken = default);

    #endregion

    #region Specification Operations

    Task<IEnumerable<T>> FindAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    #endregion

    #region Existence Checks

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion

    #region Counting Operations

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


    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, object>>[] includes,
        CancellationToken cancellationToken = default);


    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>[] includes,
        CancellationToken cancellationToken = default);

    #endregion

    #region Ordering Operations

    Task<IEnumerable<T>> GetOrderedAsync(
        Expression<Func<T, object>> orderBy,
        bool ascending = true,
        CancellationToken cancellationToken = default);


    Task<IEnumerable<T>> GetOrderedAsync(
        Expression<Func<T, object>> orderBy,
        Expression<Func<T, object>>[] includes,
        bool ascending = true,
        CancellationToken cancellationToken = default);


    Task<IEnumerable<T>> GetOrderedAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> orderBy,
        bool ascending = true,
        CancellationToken cancellationToken = default);


    Task<IEnumerable<T>> GetOrderedAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> orderBy,
        Expression<Func<T, object>>[] includes,
        bool ascending = true,
        CancellationToken cancellationToken = default);

    #endregion

    #region Search Operations

    Task<IEnumerable<T>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> SearchAsync(string searchTerm, Expression<Func<T, object>>[] searchFields, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> SearchAsync(string searchTerm, Expression<Func<T, object>>[] searchFields, Expression<Func<T, object>>[] includes, CancellationToken cancellationToken = default);

    Task<(IEnumerable<T> Items, int TotalCount)> SearchPagedAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    Task<(IEnumerable<T> Items, int TotalCount)> SearchPagedAsync(string searchTerm, int pageNumber, int pageSize, Expression<Func<T, object>>[] searchFields, CancellationToken cancellationToken = default);

    Task<(IEnumerable<T> Items, int TotalCount)> SearchPagedAsync(string searchTerm, int pageNumber, int pageSize, Expression<Func<T, object>>[] searchFields, Expression<Func<T, object>>[] includes, CancellationToken cancellationToken = default);

    Task<int> SearchCountAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task<int> SearchCountAsync(string searchTerm, Expression<Func<T, object>>[] searchFields, CancellationToken cancellationToken = default);

    #endregion

    #region Advanced Search Operations

    Task<IEnumerable<T>> SearchAsync(ISearchCriteria<T, TId> searchCriteria, CancellationToken cancellationToken = default);

    Task<(IEnumerable<T> Items, int TotalCount)> SearchPagedAsync(ISearchCriteria<T, TId> searchCriteria, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    Task<int> SearchCountAsync(ISearchCriteria<T, TId> searchCriteria, CancellationToken cancellationToken = default);

    #endregion

    #region Query Builder

    IQueryBuilder<T, TId> CreateQuery();

    #endregion
}
