namespace MyShop.Domain.Shared.ValueObjects.Identity;

public class AccountInfo : BaseValueObject
{
    public DateTime CreatedAt { get; private init; }
    public DateTime? LastLoginAt { get; private init; }
    public DateTime? LastPasswordChangeAt { get; private init; }
    // حذف LoginAttempts چون AccessFailedCount در IdentityUser موجود است
    // حذف IsActive/IsDeleted چون LockoutEnabled/LockoutEnd در IdentityUser موجود است
    public DateTime? DeletedAt { get; private init; }
    public string? BranchId { get; private init; }

    public AccountInfo() 
    {
        // Default constructor for EF Core
        CreatedAt = DateTime.UtcNow;
    } 

    private AccountInfo(
        DateTime createdAt,
        DateTime? lastLoginAt,
        DateTime? lastPasswordChangeAt,
        DateTime? deletedAt,
        string? branchId)
    {
        CreatedAt = createdAt;
        LastLoginAt = lastLoginAt;
        LastPasswordChangeAt = lastPasswordChangeAt;
        DeletedAt = deletedAt;
        BranchId = branchId;
    }

    public static AccountInfo Create(IDateTimeService dateTimeService, string? branchId = null) =>
        new(dateTimeService.UtcNow, null, null, null, branchId);

    public AccountInfo WithLastLogin(IDateTimeService dateTimeService) =>
        new(CreatedAt, dateTimeService.UtcNow, LastPasswordChangeAt, DeletedAt, BranchId);

    public AccountInfo WithPasswordChanged(IDateTimeService dateTimeService) =>
        new(CreatedAt, LastLoginAt, dateTimeService.UtcNow, DeletedAt, BranchId);

    public AccountInfo MarkAsDeleted(IDateTimeService dateTimeService) =>
        new(CreatedAt, LastLoginAt, LastPasswordChangeAt, dateTimeService.UtcNow, BranchId);

    public AccountInfo WithBranchId(string? branchId) =>
        new(CreatedAt, LastLoginAt, LastPasswordChangeAt, DeletedAt, branchId);

    // Business rules - استفاده از IdentityUser properties
    public bool IsDeleted => DeletedAt.HasValue;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedAt;
        yield return LastLoginAt;
        yield return LastPasswordChangeAt;
        yield return DeletedAt;
        yield return BranchId;
    }
}
