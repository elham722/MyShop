namespace MyShop.Domain.ValueObjects.Common;
public class AuditInfo : BaseValueObject
{
    public string? CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? ModifiedBy { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }

    private AuditInfo() { }

    public AuditInfo(
        string? createdBy = null,
        DateTime? createdAt = null,
        string? ipAddress = null,
        string? userAgent = null)
    {
        CreatedBy = createdBy;
        CreatedAt = createdAt ?? DateTime.UtcNow;
        ModifiedBy = null;
        ModifiedAt = null;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public AuditInfo UpdateModified(string? modifiedBy, string? ipAddress = null, string? userAgent = null)
    {
        return new AuditInfo
        {
            CreatedBy = CreatedBy,
            CreatedAt = CreatedAt,
            ModifiedBy = modifiedBy,
            ModifiedAt = DateTime.UtcNow,
            IpAddress = ipAddress ?? IpAddress,
            UserAgent = userAgent ?? UserAgent
        };
    }

    public bool HasBeenModified => ModifiedAt.HasValue;

    public bool WasCreatedBy(string userId)
    {
        return !string.IsNullOrEmpty(CreatedBy) && CreatedBy.Equals(userId, StringComparison.OrdinalIgnoreCase);
    }

    public bool WasLastModifiedBy(string userId)
    {
        return !string.IsNullOrEmpty(ModifiedBy) && ModifiedBy.Equals(userId, StringComparison.OrdinalIgnoreCase);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedBy;
        yield return CreatedAt;
        yield return ModifiedBy;
        yield return ModifiedAt; // Keep null as null, don't convert to DateTime.MinValue
        yield return IpAddress;
        yield return UserAgent;
    }
}
