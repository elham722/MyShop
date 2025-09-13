namespace MyShop.Domain.Specifications.Common;
/// <summary>
/// Generic specification implementation for dynamic criteria
/// </summary>
public class GenericSpecification<T> : BaseSpecification<T>
{
    public GenericSpecification() { }

    public GenericSpecification(Expression<Func<T, bool>> criteria) : base(criteria) { }
}

