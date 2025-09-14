namespace MyShop.Domain.Shared.Base;
  public abstract class AuditableEntity<TId> : BaseEntity<TId> where TId : IEquatable<TId>
{
    public DateTime CreatedAt { get; protected set; }
    public string CreatedBy { get; protected set; } = string.Empty;
    public DateTime? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }

    protected AuditableEntity() : base() { }
    protected AuditableEntity(TId id) : base(id) { }

    public virtual void MarkAsCreated(string createdBy)
    {
        MarkAsCreated(createdBy, DateTime.UtcNow);
    }

    public virtual void MarkAsCreated(string createdBy, DateTime createdAt)
    {
        Guard.AgainstNullOrEmpty(createdBy, nameof(createdBy));
        CreatedBy = createdBy;
        CreatedAt = EnsureUtc(createdAt);
        IsDeleted = false;
        base.MarkAsCreated();
    }

    public virtual void MarkAsUpdated(string updatedBy)
    {
        MarkAsUpdated(updatedBy, DateTime.UtcNow);
    }

    public virtual void MarkAsUpdated(string updatedBy, DateTime updatedAt)
    {
        Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));
        UpdatedBy = updatedBy;
        UpdatedAt = EnsureUtc(updatedAt);
        base.MarkAsUpdated();
    }

    public virtual void MarkAsDeleted(string deletedBy)
    {
        MarkAsDeleted(deletedBy, DateTime.UtcNow);
    }

    public virtual void MarkAsDeleted(string deletedBy, DateTime deletedAt)
    {
        Guard.AgainstNullOrEmpty(deletedBy, nameof(deletedBy));
        DeletedBy = deletedBy;
        DeletedAt = EnsureUtc(deletedAt);
        IsDeleted = true;
        base.MarkAsDeleted();
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }

    private static DateTime EnsureUtc(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc
            ? dateTime
            : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}


