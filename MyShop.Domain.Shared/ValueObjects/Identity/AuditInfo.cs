namespace MyShop.Domain.Shared.ValueObjects.Identity;

public class AuditInfo : BaseValueObject
{
    public string? CreatedBy { get; private init; }
    public DateTime CreatedAt { get; private init; }
    public string? ModifiedBy { get; private init; }
    public DateTime? ModifiedAt { get; private init; }
    // حذف IpAddress و UserAgent چون برای Identity معمولاً نیاز نیست
    // اگر نیاز باشد می‌توان در AuditLog جداگانه نگه داشت

    public AuditInfo() 
    {
        // Default constructor for EF Core
        CreatedAt = DateTime.UtcNow;
    } 

    private AuditInfo(string? createdBy, DateTime createdAt, string? modifiedBy, DateTime? modifiedAt)
    {
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        ModifiedBy = modifiedBy;
        ModifiedAt = modifiedAt;
    }

    public static AuditInfo Create(string? createdBy, IDateTimeService? dateTimeService = null) =>
        new(createdBy, dateTimeService?.UtcNow ?? DateTime.UtcNow, null, null);

    public AuditInfo WithModified(string? modifiedBy, IDateTimeService dateTimeService) =>
        new(CreatedBy, CreatedAt, modifiedBy, dateTimeService.UtcNow);

    public bool HasBeenModified => ModifiedAt.HasValue;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedBy;
        yield return CreatedAt;
        yield return ModifiedBy;
        yield return ModifiedAt;
    }
}