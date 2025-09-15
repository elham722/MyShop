namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Data Transfer Object for UserToken
    /// </summary>
    public class UserTokenDto
    {
        public string UserId { get; set; } = string.Empty;
        public string LoginProvider { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? DeviceInfo { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokedBy { get; set; }
        public string? RevocationReason { get; set; }
        public string? TokenType { get; set; }
        public string? TokenPurpose { get; set; }
        public int UsageCount { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public string? ParentTokenId { get; set; }
        public bool IsRotated { get; set; }
        public DateTime? RotatedAt { get; set; }
        public string? RotatedBy { get; set; }
        
        // Computed Properties
        public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
        public bool IsValid => IsActive && !IsRevoked && !IsExpired;
        public TimeSpan? TimeUntilExpiry => ExpiresAt?.Subtract(DateTime.UtcNow);
    }
}