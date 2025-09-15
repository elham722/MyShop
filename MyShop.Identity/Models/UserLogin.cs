using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    /// <summary>
    /// Custom UserLogin for multi-device login tracking and security monitoring
    /// For simple external login providers, use the default IdentityUserLogin<string>
    /// </summary>
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
        public DateTime? LastUsedAt { get; private set; }
        public string? Location { get; private set; }
        public bool IsTrusted { get; private set; }

        private UserLogin() { } // For EF Core

        public static UserLogin Create(string userId, string loginProvider, string providerKey, string? deviceInfo = null, string? ipAddress = null, string? userAgent = null, string? location = null, bool isTrusted = false, string createdBy = "System")
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
                UserAgent = userAgent,
                LastUsedAt = DateTime.UtcNow,
                Location = location,
                IsTrusted = isTrusted
            };
        }

        public void UpdateDeviceInfo(string? deviceInfo, string? ipAddress, string? userAgent, string? location, string updatedBy)
        {
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

            DeviceInfo = deviceInfo;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            Location = location;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        public void UpdateLastUsed()
        {
            LastUsedAt = DateTime.UtcNow;
        }

        public void MarkAsTrusted(string trustedBy)
        {
            Guard.AgainstNullOrEmpty(trustedBy, nameof(trustedBy));

            IsTrusted = true;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = trustedBy;
        }

        public void MarkAsUntrusted(string untrustedBy)
        {
            Guard.AgainstNullOrEmpty(untrustedBy, nameof(untrustedBy));

            IsTrusted = false;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = untrustedBy;
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

        /// <summary>
        /// Checks if this login is from the same device
        /// </summary>
        public bool IsFromSameDevice(string? deviceInfo, string? ipAddress, string? userAgent)
        {
            return DeviceInfo == deviceInfo && IpAddress == ipAddress && UserAgent == userAgent;
        }

        /// <summary>
        /// Checks if this login is from the same location
        /// </summary>
        public bool IsFromSameLocation(string? location)
        {
            return Location == location;
        }

        /// <summary>
        /// Checks if this login is suspicious
        /// </summary>
        public bool IsSuspicious(string? currentIpAddress, string? currentLocation)
        {
            // Check for IP address change
            if (!string.IsNullOrEmpty(IpAddress) && IpAddress != currentIpAddress)
                return true;

            // Check for location change
            if (!string.IsNullOrEmpty(Location) && Location != currentLocation)
                return true;

            // Check if not trusted
            if (!IsTrusted)
                return true;

            return false;
        }

        /// <summary>
        /// Checks if this login is expired based on last usage
        /// </summary>
        public bool IsExpired(TimeSpan? expirationPeriod = null)
        {
            if (!expirationPeriod.HasValue || !LastUsedAt.HasValue)
                return false;

            return DateTime.UtcNow.Subtract(LastUsedAt.Value) > expirationPeriod.Value;
        }

        /// <summary>
        /// Gets the time since last usage
        /// </summary>
        public TimeSpan? GetTimeSinceLastUsed()
        {
            if (!LastUsedAt.HasValue)
                return null;

            return DateTime.UtcNow - LastUsedAt.Value;
        }

        /// <summary>
        /// Gets a human-readable description of the last usage
        /// </summary>
        public string GetLastUsageDescription()
        {
            if (!LastUsedAt.HasValue)
                return "Never used";

            var timeSince = GetTimeSinceLastUsed();
            if (!timeSince.HasValue)
                return "Just now";

            if (timeSince.Value.TotalDays >= 1)
                return $"Last used {timeSince.Value.Days} days ago";
            
            if (timeSince.Value.TotalHours >= 1)
                return $"Last used {timeSince.Value.Hours} hours ago";
            
            if (timeSince.Value.TotalMinutes >= 1)
                return $"Last used {timeSince.Value.Minutes} minutes ago";
            
            return "Just now";
        }

        /// <summary>
        /// Gets the device type from user agent
        /// </summary>
        public string GetDeviceType()
        {
            if (string.IsNullOrEmpty(UserAgent))
                return "Unknown";

            var userAgent = UserAgent.ToLowerInvariant();
            
            if (userAgent.Contains("mobile") || userAgent.Contains("android") || userAgent.Contains("iphone"))
                return "Mobile";
            
            if (userAgent.Contains("tablet") || userAgent.Contains("ipad"))
                return "Tablet";
            
            if (userAgent.Contains("desktop") || userAgent.Contains("windows") || userAgent.Contains("mac"))
                return "Desktop";
            
            return "Unknown";
        }

        /// <summary>
        /// Gets the browser from user agent
        /// </summary>
        public string GetBrowser()
        {
            if (string.IsNullOrEmpty(UserAgent))
                return "Unknown";

            var userAgent = UserAgent.ToLowerInvariant();
            
            if (userAgent.Contains("chrome"))
                return "Chrome";
            
            if (userAgent.Contains("firefox"))
                return "Firefox";
            
            if (userAgent.Contains("safari"))
                return "Safari";
            
            if (userAgent.Contains("edge"))
                return "Edge";
            
            if (userAgent.Contains("opera"))
                return "Opera";
            
            return "Unknown";
        }

        /// <summary>
        /// Gets a summary of this login
        /// </summary>
        public string GetLoginSummary()
        {
            var deviceType = GetDeviceType();
            var browser = GetBrowser();
            var lastUsage = GetLastUsageDescription();
            var trusted = IsTrusted ? "Trusted" : "Untrusted";
            
            return $"{LoginProvider} - {deviceType} ({browser}) - {lastUsage} - {trusted}";
        }
    }
} 