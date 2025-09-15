namespace MyShop.Domain.Shared.ValueObjects.Common;

public abstract class BaseValueObject : IEquatable<BaseValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj) =>
        obj is BaseValueObject other && GetType() == obj.GetType() &&
        GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public bool Equals(BaseValueObject? other) =>
        other is not null && GetType() == other.GetType() &&
        GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(17, (current, obj) => current * 23 ^ (obj?.GetHashCode() ?? 0));

    public static bool operator ==(BaseValueObject? left, BaseValueObject? right) =>
        Equals(left, right);

    public static bool operator !=(BaseValueObject? left, BaseValueObject? right) =>
        !Equals(left, right);
}