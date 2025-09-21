using MyShop.Domain.Shared.Base;

namespace MyShop.Identity.Models;

public class RefreshToken : BaseEntity
{
    public string UserId { get; private set; } = null!;
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? DeviceInfo { get; private set; }
    public string? IpAddress { get; private set; }

    // Navigation Properties
    public ApplicationUser User { get; set; } = null!;

    private RefreshToken() { }

    public static RefreshToken Create(
        string userId,
        string token,
        DateTime expiresAt,
        string? deviceInfo = null,
        string? ipAddress = null)
    {
        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            DeviceInfo = deviceInfo,
            IpAddress = ipAddress,
            IsRevoked = false
        };
    }

    public void Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    public bool IsValid => !IsRevoked && !IsExpired;
}