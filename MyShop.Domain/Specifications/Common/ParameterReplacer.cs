namespace MyShop.Domain.Specifications.Common;
/// <summary>
/// Expression visitor that replaces parameter expressions in an expression tree.
/// This is used to combine multiple criteria expressions by replacing their parameters
/// with a single unified parameter.
/// </summary>
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

    /// <summary>
    /// Replaces the old parameter with the new parameter.
    /// This method does NOT call base.VisitParameter because we want to replace
    /// the parameter directly, not visit it.
    /// </summary>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        // Replace the old parameter with the new one
        // Do NOT call base.VisitParameter(node) as we want direct replacement
        return node == _oldParameter ? _newParameter : node;
    }
}