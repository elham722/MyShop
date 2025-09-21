using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Exceptions;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Interfaces;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models;

public class ApplicationUser : IdentityUser<string>
{
    // Core Identity properties (inherited from IdentityUser)
    // Email, UserName, PasswordHash, TwoFactorEnabled, LockoutEnd, etc.

    // Business-specific properties
    public string? CustomerId { get; private set; }

    // Social Logins
    public string? GoogleId { get; private set; }
    public string? MicrosoftId { get; private set; }

    // MFA
    public string? TotpSecretKey { get; private set; }
    public bool TotpEnabled { get; private set; }

    // Extra states - inline instead of Value Objects
    public bool IsDeleted { get; private set; }
    public DateTime? LastPasswordChangeAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? BranchId { get; private set; }

    // Navigation Properties
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    // EF Core constructor
    private ApplicationUser()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public static ApplicationUser Create(
        string email,
        string userName,
        string? customerId = null)
    {
        Guard.AgainstNullOrEmpty(email, nameof(email));
        Guard.AgainstNullOrEmpty(userName, nameof(userName));

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
            CreatedAt = DateTime.UtcNow
        };
    }

    // State update methods
    internal void SetCustomerId(string? customerId) => CustomerId = customerId;
    internal void SetTotpSecretKey(string? secretKey) => TotpSecretKey = secretKey;
    internal void SetTotpEnabled(bool enabled) => TotpEnabled = enabled;
    internal void SetGoogleId(string? googleId) => GoogleId = googleId;
    internal void SetMicrosoftId(string? microsoftId) => MicrosoftId = microsoftId;
    internal void SetIsDeleted(bool isDeleted) => IsDeleted = isDeleted;
    internal void SetLastPasswordChangeAt(DateTime? date) => LastPasswordChangeAt = date;
    internal void SetLastLoginAt(DateTime? date) => LastLoginAt = date;
    internal void SetBranchId(string? branchId) => BranchId = branchId;

    // Computed Properties
    public bool IsLocked => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;
    public bool IsActive => !IsLocked && EmailConfirmed && !IsDeleted;
    public bool IsNewUser => CreatedAt != null && DateTime.UtcNow.Subtract(CreatedAt).Days <= 7;
    public bool RequiresPasswordChange =>
        LastPasswordChangeAt == null ||
        (LastPasswordChangeAt != null && DateTime.UtcNow.Subtract(LastPasswordChangeAt.Value).Days > 90);

    // Business rules
    public bool CanLogin() => IsActive && !IsLocked;
}