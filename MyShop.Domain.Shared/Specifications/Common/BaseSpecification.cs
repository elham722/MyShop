namespace MyShop.Domain.Shared.Specifications.Common;
   public abstract class BaseSpecification<T> : ISpecification<T>
{
    private readonly List<Expression<Func<T, bool>>> _criteria = new();
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected BaseSpecification() { }

    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        AddCriteria(criteria);
    }

    public Expression<Func<T, bool>> Criteria => CombineCriteria();

    public Expression<Func<T, bool>> ToExpression()
    {
        return Criteria;
    }

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Guard.AgainstNull(criteria, nameof(criteria));
        _criteria.Add(criteria);
    }

    protected void AddCriteria(params Expression<Func<T, bool>>[] criteria)
    {
        foreach (var criterion in criteria)
        {
            AddCriteria(criterion);
        }
    }

    protected void AddCriteria(IEnumerable<Expression<Func<T, bool>>> criteria)
    {
        foreach (var criterion in criteria)
        {
            AddCriteria(criterion);
        }
    }

    protected void ClearCriteria()
    {
        _criteria.Clear();
    }


    public int CriteriaCount => _criteria.Count;


    public bool HasCriteria => _criteria.Count > 0;

    public ISpecification<T> AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
        return this;
    }

    public ISpecification<T> AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
        return this;
    }

    public ISpecification<T> AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
        return this;
    }

    public ISpecification<T> AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
        return this;
    }

    public ISpecification<T> ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
        return this;
    }


    public ISpecification<T> And(ISpecification<T> other)
    {
        var newSpec = new CompositeSpecification<T>(this, other, "AND");
        return newSpec;
    }


    public ISpecification<T> Or(ISpecification<T> other)
    {
        var newSpec = new CompositeSpecification<T>(this, other, "OR");
        return newSpec;
    }


    public ISpecification<T> Not()
    {
        var newSpec = new CompositeSpecification<T>(this, null, "NOT");
        return newSpec;
    }


    public virtual string Description => $"Specification with {CriteriaCount} criteria(s)";


    public virtual ISpecification<T> Clone()
    {
        var clone = new GenericSpecification<T>();

        // Copy criteria
        foreach (var criteria in _criteria)
        {
            clone.AddCriteria(criteria);
        }

        // Copy includes
        clone.Includes.AddRange(Includes);
        clone.IncludeStrings.AddRange(IncludeStrings);

        // Copy ordering
        if (OrderBy != null)
            clone.AddOrderBy(OrderBy);
        if (OrderByDescending != null)
            clone.AddOrderByDescending(OrderByDescending);

        // Copy paging
        if (IsPagingEnabled)
            clone.ApplyPaging(Skip, Take);

        return clone;
    }

    private Expression<Func<T, bool>> CombineCriteria()
    {
        // If no criteria, return true (match all)
        if (!_criteria.Any())
            return x => true;

        // If only one criteria, return it directly
        if (_criteria.Count == 1)
            return _criteria.First();

        // Combine multiple criteria with AND logic
        return CombineMultipleCriteria();
    }

    private Expression<Func<T, bool>> CombineMultipleCriteria()
    {
        // Create a new parameter for the combined expression
        var combinedParameter = Expression.Parameter(typeof(T), "x");

        // Start with the first criteria
        var combinedBody = ReplaceParameter(_criteria[0].Body, _criteria[0].Parameters[0], combinedParameter);

        // Combine with remaining criteria using AND logic
        for (int i = 1; i < _criteria.Count; i++)
        {
            var currentBody = ReplaceParameter(_criteria[i].Body, _criteria[i].Parameters[0], combinedParameter);
            combinedBody = Expression.AndAlso(combinedBody, currentBody);
        }

        return Expression.Lambda<Func<T, bool>>(combinedBody, combinedParameter);
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
    }

}


