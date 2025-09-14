namespace MyShop.Domain.Shared.Specifications.Common;

public class GenericSpecification<T> : BaseSpecification<T>
{
    public GenericSpecification() { }

    public GenericSpecification(Expression<Func<T, bool>> criteria) : base(criteria) { }
}

