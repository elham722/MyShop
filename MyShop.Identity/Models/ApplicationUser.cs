using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Exceptions;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Interfaces;
using MyShop.Domain.Shared.Shared;
using MyShop.Domain.Shared.ValueObjects.Identity;

namespace MyShop.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public AccountInfo Account { get; private set; } = null!;
    public SecurityInfo Security { get; private set; } = null!;
    public AuditInfo Audit { get; private set; } = null!;

    public string? CustomerId { get; private set; }

    // MFA
    public string? TotpSecretKey { get; private set; }
    public bool TotpEnabled { get; private set; }
    public bool SmsEnabled { get; private set; }

    // Social Logins
    public string? GoogleId { get; private set; }
    public string? MicrosoftId { get; private set; }

    // Computed Properties
    public bool IsLocked => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;
    public bool IsAccountLocked => Account.IsLocked();
    public bool IsActive => Account.IsActive && !Account.IsDeleted;
    public bool IsNewUser => DateTime.UtcNow.Subtract(Account.CreatedAt).Days <= 7;

    // Additional state
    public bool IsDeleted => Account.IsDeleted;
    public DateTime? LastLoginAt => Account.LastLoginAt;
    public DateTime? LastPasswordChangeAt => Account.LastPasswordChangeAt;
    public int LoginAttempts => Account.LoginAttempts;
    public bool RequiresPasswordChange =>
        Account.LastPasswordChangeAt == null ||
        DateTime.UtcNow.Subtract(Account.LastPasswordChangeAt.Value).Days > 90;

    // Navigation Properties
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    // EF Core constructor
    private ApplicationUser()
    {
    
    }

    public static ApplicationUser Create(
        string email,
        string userName,
        string? customerId = null,
        string? createdBy = null,
        IDateTimeService? dateTimeService = null)
    {
        Guard.AgainstNullOrEmpty(email, nameof(email));
        Guard.AgainstNullOrEmpty(userName, nameof(userName));
        
        var dt = dateTimeService;

        return new ApplicationUser
        {
            Email = email,
            UserName = userName,
            EmailConfirmed = false,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = true,
            AccessFailedCount = 0,
            CustomerId = customerId,
            Account = AccountInfo.Create(dt),
            Security = SecurityInfo.Create(),
            Audit = AuditInfo.Create(createdBy)
        };
    }

    // State update methods

    internal void UpdateAccount(AccountInfo account)
    {
        Guard.AgainstNull(account, nameof(account));
        Account = account;
    }

    internal void UpdateSecurity(SecurityInfo security) =>
        Guard.AgainstNull(security, nameof(security));

    internal void UpdateAudit(AuditInfo audit) =>
        Guard.AgainstNull(audit, nameof(audit));

    internal void SetCustomerId(string? customerId) => CustomerId = customerId;
    internal void SetTotpSecretKey(string? secretKey) => TotpSecretKey = secretKey;
    internal void SetTotpEnabled(bool enabled) => TotpEnabled = enabled;
    internal void SetSmsEnabled(bool enabled) => SmsEnabled = enabled;
    internal void SetGoogleId(string? googleId) => GoogleId = googleId;
    internal void SetMicrosoftId(string? microsoftId) => MicrosoftId = microsoftId;

    // Business rules
    public bool CanLogin() => IsActive && !IsLocked && !IsAccountLocked;
}
