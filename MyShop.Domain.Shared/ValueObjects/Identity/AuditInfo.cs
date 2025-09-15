namespace MyShop.Domain.Shared.ValueObjects.Identity;

public class AuditInfo : BaseValueObject
{
    public string? CreatedBy { get; private init; }
    public DateTime CreatedAt { get; private init; }
    public string? ModifiedBy { get; private init; }
    public DateTime? ModifiedAt { get; private init; }
    public string? IpAddress { get; private init; }
    public string? UserAgent { get; private init; }

    private AuditInfo() { } 

    private AuditInfo(string? createdBy, DateTime createdAt, string? modifiedBy, DateTime? modifiedAt, string? ipAddress, string? userAgent)
    {
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        ModifiedBy = modifiedBy;
        ModifiedAt = modifiedAt;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public static AuditInfo Create(string? createdBy, string? ipAddress = null, string? userAgent = null, IDateTimeService? dateTimeService = null) =>
        new(createdBy, dateTimeService?.UtcNow ?? DateTime.UtcNow, null, null, ipAddress, userAgent);

    public AuditInfo WithModified(string? modifiedBy, IDateTimeService dateTimeService, string? ipAddress = null, string? userAgent = null) =>
        new(CreatedBy, CreatedAt, modifiedBy, dateTimeService.UtcNow, ipAddress ?? IpAddress, userAgent ?? UserAgent);

    public bool HasBeenModified => ModifiedAt.HasValue;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedBy;
        yield return CreatedAt;
        yield return ModifiedBy;
        yield return ModifiedAt;
        yield return IpAddress;
        yield return UserAgent;
    }
}