using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    public class UserLogin : IdentityUserLogin<string>
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = null!;
        public string? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; }
        public string? DeviceInfo { get; private set; }
        public string? IpAddress { get; private set; }
        public string? UserAgent { get; private set; }

        private UserLogin() { } // For EF Core

        public static UserLogin Create(string userId, string loginProvider, string providerKey, string? deviceInfo = null, string? ipAddress = null, string? userAgent = null, string createdBy = "System")
        {
            Guard.AgainstNullOrEmpty(userId, nameof(userId));
            Guard.AgainstNullOrEmpty(loginProvider, nameof(loginProvider));
            Guard.AgainstNullOrEmpty(providerKey, nameof(providerKey));
          
            return new UserLogin
            {
                UserId = userId,
                LoginProvider = loginProvider,
                ProviderKey = providerKey,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                IsActive = true,
                DeviceInfo = deviceInfo,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };
        }

        public void UpdateDeviceInfo(string? deviceInfo, string? ipAddress, string? userAgent, string updatedBy)
        {
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

            DeviceInfo = deviceInfo;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
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

        public bool IsFromSameDevice(string? deviceInfo, string? ipAddress, string? userAgent)
        {
            return DeviceInfo == deviceInfo && IpAddress == ipAddress && UserAgent == userAgent;
        }

        public bool IsExpired(TimeSpan? expirationPeriod = null)
        {
            if (!expirationPeriod.HasValue)
                return false;

            return DateTime.UtcNow.Subtract(CreatedAt) > expirationPeriod.Value;
        }
    }
} 