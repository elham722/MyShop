namespace MyShop.Domain.Shared.Specifications.Common;

internal class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly ParameterExpression _newParameter;

    public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        Guard.AgainstNull(oldParameter, nameof(oldParameter));
        Guard.AgainstNull(newParameter, nameof(newParameter));
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        // Replace the old parameter with the new one
        // Do NOT call base.VisitParameter(node) as we want direct replacement
        return node == _oldParameter ? _newParameter : node;
    }
}