using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    public class UserToken : IdentityUserToken<string>
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = null!;
        public string? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public string? DeviceInfo { get; private set; }
        public string? IpAddress { get; private set; }
        public string? UserAgent { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? RevokedBy { get; private set; }
        public string? RevocationReason { get; private set; }

        private UserToken() { } // For EF Core

        public static UserToken Create(string userId, string loginProvider, string name, string value, DateTime? expiresAt = null, string? deviceInfo = null, string? ipAddress = null, string? userAgent = null, string createdBy = "System")
        {
            Guard.AgainstNullOrEmpty(userId, nameof(userId));
            Guard.AgainstNullOrEmpty(loginProvider, nameof(loginProvider));
            Guard.AgainstNullOrEmpty(name, nameof(name));
            Guard.AgainstNullOrEmpty(value, nameof(value));

            return new UserToken
            {
                UserId = userId,
                LoginProvider = loginProvider,
                Name = name,
                Value = value,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                IsActive = true,
                ExpiresAt = expiresAt,
                DeviceInfo = deviceInfo,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                IsRevoked = false
            };
        }

        public void UpdateValue(string newValue, string updatedBy)
        {
            Guard.AgainstNullOrEmpty(newValue, nameof(newValue));
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

            if (IsRevoked)
                throw new CustomValidationException("Cannot update a revoked token");

            Value = newValue;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        public void UpdateDeviceInfo(string? deviceInfo, string? ipAddress, string? userAgent, string updatedBy)
        {
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

            if (IsRevoked)
                throw new CustomValidationException("Cannot update a revoked token");

            DeviceInfo = deviceInfo;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        public void ExtendExpiration(DateTime newExpiresAt, string extendedBy)
        {
            Guard.AgainstNullOrEmpty(extendedBy, nameof(extendedBy));

            if (newExpiresAt <= DateTime.UtcNow)
                throw new CustomValidationException("New expiration date must be in the future");

            if (IsRevoked)
                throw new CustomValidationException("Cannot extend expiration of a revoked token");

            ExpiresAt = newExpiresAt;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = extendedBy;
        }

        public void Revoke(string revokedBy, string? revocationReason = null)
        {
            Guard.AgainstNullOrEmpty(revokedBy, nameof(revokedBy));

            if (IsRevoked)
                throw new CustomValidationException("Token is already revoked");

            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
            RevokedBy = revokedBy;
            RevocationReason = revocationReason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = revokedBy;
        }

        public void Deactivate(string deactivatedBy)
        {
            Guard.AgainstNullOrEmpty(deactivatedBy, nameof(deactivatedBy));

            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = deactivatedBy;
        }

        public void Activate(string activatedBy)
        {
            Guard.AgainstNullOrEmpty(activatedBy, nameof(activatedBy));

            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = activatedBy;
        }

        public bool IsExpired()
        {
            return ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
        }

        public bool IsValid()
        {
            return IsActive && !IsRevoked && !IsExpired();
        }

        public bool IsFromSameDevice(string? deviceInfo, string? ipAddress, string? userAgent)
        {
            return DeviceInfo == deviceInfo && IpAddress == ipAddress && UserAgent == userAgent;
        }

        public TimeSpan? GetRemainingTime()
        {
            if (!ExpiresAt.HasValue || IsExpired())
                return null;

            return ExpiresAt.Value - DateTime.UtcNow;
        }
    }
} 