namespace MyShop.Domain.Specifications.Common;
/// <summary>
/// Composite specification that combines multiple specifications
/// </summary>
public class CompositeSpecification<T> : BaseSpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T>? _right;
    private readonly string _operator;

    public CompositeSpecification(ISpecification<T> left, ISpecification<T>? right, string @operator)
    {
        Guard.AgainstNull(left, nameof(left));
        Guard.AgainstNull(@operator, nameof(@operator));
        _left = left;
        _right = right;
        _operator = @operator;

        BuildCompositeCriteria();
    }

    private void BuildCompositeCriteria()
    {
        switch (_operator.ToUpper())
        {
            case "AND":
                if (_right != null)
                {
                    AddCriteria(x => _left.IsSatisfiedBy(x) && _right.IsSatisfiedBy(x));
                }
                break;
            case "OR":
                if (_right != null)
                {
                    AddCriteria(x => _left.IsSatisfiedBy(x) || _right.IsSatisfiedBy(x));
                }
                break;
            case "NOT":
                AddCriteria(x => !_left.IsSatisfiedBy(x));
                break;
            default:
                throw new ArgumentException($"Unsupported operator: {_operator}");
        }
    }

    public override string Description => $"Composite specification ({_operator})";
}

