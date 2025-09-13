namespace MyShop.Contracts.Repositories.Search;
public interface ISearchCriteria<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    string SearchTerm { get; set; }

    Expression<Func<T, object>>[] SearchFields { get; set; }

    Expression<Func<T, bool>>[] Filters { get; set; }

    Expression<Func<T, object>>[] Includes { get; set; }

    SearchOrderCriteria[] OrderBy { get; set; }

    SearchOptions Options { get; set; }
}