namespace MyShop.Domain.ValueObjects.Common;
public abstract class BaseValueObject : IEquatable<BaseValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        return Equals((BaseValueObject)obj);
    }

    public bool Equals(BaseValueObject? other)
    {
        if (other is null || other.GetType() != GetType())
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(17, (current, obj) => current * 23 ^ (obj?.GetHashCode() ?? 0));
    }

    public static bool operator ==(BaseValueObject? left, BaseValueObject? right) =>
        Equals(left, right);

    public static bool operator !=(BaseValueObject? right, BaseValueObject? left) =>
        !Equals(left, right);
}
