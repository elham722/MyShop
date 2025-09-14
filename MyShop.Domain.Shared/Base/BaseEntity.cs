namespace MyShop.Domain.Shared.Base;
public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>> where TId : IEquatable<TId>
{
    public TId Id { get; protected set; } = default!;

    private readonly List<BaseDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected BaseEntity()
    {
        if (typeof(TId) == typeof(Guid) && EqualityComparer<TId>.Default.Equals(Id, default!))
        {
            Id = (TId)(object)Guid.NewGuid();
        }
    }
    protected BaseEntity(TId id) => Id = ValidateId(id);

    public void AddDomainEvent(BaseDomainEvent @event)
    {
        Guard.AgainstNull(@event, nameof(@event));
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();

    public bool HasDomainEvents => _domainEvents.Count > 0;


    protected virtual void OnCreated() { }
    protected virtual void OnUpdated() { }
    protected virtual void OnDeleted() { }


    public virtual void MarkAsCreated() => OnCreated();
    public virtual void MarkAsUpdated() => OnUpdated();
    public virtual void MarkAsDeleted() => OnDeleted();


    public override bool Equals(object? obj) => Equals(obj as BaseEntity<TId>);
    public bool Equals(BaseEntity<TId>? other) =>
        other is not null && EqualityComparer<TId>.Default.Equals(Id, other.Id);

    public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id);

    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right) => Equals(left, right);
    public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right) => !Equals(left, right);

    protected virtual TId ValidateId(TId id)
    {
        if (EqualityComparer<TId>.Default.Equals(id, default!))
            throw new CustomValidationException("Id cannot be default");
        return id;
    }
}

