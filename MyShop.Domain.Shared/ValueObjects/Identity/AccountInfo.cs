namespace MyShop.Domain.Shared.ValueObjects.Identity;

public class AccountInfo : BaseValueObject
{
    public DateTime CreatedAt { get; private init; }
    public DateTime? LastLoginAt { get; private init; }
    public DateTime? LastPasswordChangeAt { get; private init; }
    public int LoginAttempts { get; private init; }
    public bool IsActive { get; private init; }
    public bool IsDeleted { get; private init; }
    public DateTime? DeletedAt { get; private init; }
    public string? BranchId { get; private init; }

    private AccountInfo() { } 

    private AccountInfo(
        DateTime createdAt,
        DateTime? lastLoginAt,
        DateTime? lastPasswordChangeAt,
        int loginAttempts,
        bool isActive,
        bool isDeleted,
        DateTime? deletedAt,
        string? branchId)
    {
        CreatedAt = createdAt;
        LastLoginAt = lastLoginAt;
        LastPasswordChangeAt = lastPasswordChangeAt;
        LoginAttempts = loginAttempts;
        IsActive = isActive;
        IsDeleted = isDeleted;
        DeletedAt = deletedAt;
        BranchId = branchId;
    }

    public static AccountInfo Create(IDateTimeService dateTimeService, string? branchId = null) =>
        new(dateTimeService.UtcNow, null, null, 0, true, false, null, branchId);

    public AccountInfo WithLastLogin(IDateTimeService dateTimeService) =>
        new(CreatedAt, dateTimeService.UtcNow, LastPasswordChangeAt, 0, IsActive, IsDeleted, DeletedAt, BranchId);

    public AccountInfo WithPasswordChanged(IDateTimeService dateTimeService) =>
        new(CreatedAt, LastLoginAt, dateTimeService.UtcNow, LoginAttempts, IsActive, IsDeleted, DeletedAt, BranchId);

    public AccountInfo IncrementLoginAttempts() =>
        new(CreatedAt, LastLoginAt, LastPasswordChangeAt, LoginAttempts + 1, IsActive, IsDeleted, DeletedAt, BranchId);

    public AccountInfo Deactivate() =>
        new(CreatedAt, LastLoginAt, LastPasswordChangeAt, LoginAttempts, false, IsDeleted, DeletedAt, BranchId);

    public AccountInfo Activate() =>
        new(CreatedAt, LastLoginAt, LastPasswordChangeAt, LoginAttempts, true, IsDeleted, DeletedAt, BranchId);

    public AccountInfo MarkAsDeleted(IDateTimeService dateTimeService) =>
        new(CreatedAt, LastLoginAt, LastPasswordChangeAt, LoginAttempts, false, true, dateTimeService.UtcNow, BranchId);

    public bool IsLocked(int maxLoginAttempts = 5) => LoginAttempts >= maxLoginAttempts;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedAt;
        yield return LastLoginAt;
        yield return LastPasswordChangeAt;
        yield return LoginAttempts;
        yield return IsActive;
        yield return IsDeleted;
        yield return DeletedAt;
        yield return BranchId;
    }
}
