using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    /// <summary>
    /// Custom UserToken for refresh token management, multi-token scenarios, and token revocation
    /// For simple JWT with basic expiry, use the default IdentityUserToken<string>
    /// </summary>
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
        public string? TokenType { get; private set; }
        public string? TokenPurpose { get; private set; }
        public int UsageCount { get; private set; }
        public DateTime? LastUsedAt { get; private set; }
        public string? ParentTokenId { get; private set; }
        public bool IsRotated { get; private set; }
        public DateTime? RotatedAt { get; private set; }
        public string? RotatedBy { get; private set; }

        private UserToken() { } // For EF Core

        public static UserToken Create(string userId, string loginProvider, string name, string value, DateTime? expiresAt = null, string? deviceInfo = null, string? ipAddress = null, string? userAgent = null, string? tokenType = null, string? tokenPurpose = null, string? parentTokenId = null, string createdBy = "System")
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
                IsRevoked = false,
                TokenType = tokenType ?? "Bearer",
                TokenPurpose = tokenPurpose ?? "Authentication",
                UsageCount = 0,
                ParentTokenId = parentTokenId,
                IsRotated = false
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

        public void ExtendExpiration(DateTime newExpiresAt, string? reason = null, string extendedBy = "System")
        {
            Guard.AgainstNullOrEmpty(extendedBy, nameof(extendedBy));

            if (newExpiresAt <= DateTime.UtcNow)
                throw new CustomValidationException("New expiration date must be in the future");

            if (IsRevoked)
                throw new CustomValidationException("Cannot extend expiration of a revoked token");

            ExpiresAt = newExpiresAt;
            if (!string.IsNullOrEmpty(reason))
                RevocationReason = reason;
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

        public void Rotate(string newValue, string rotatedBy, string? reason = null)
        {
            Guard.AgainstNullOrEmpty(newValue, nameof(newValue));
            Guard.AgainstNullOrEmpty(rotatedBy, nameof(rotatedBy));

            if (IsRevoked)
                throw new CustomValidationException("Cannot rotate a revoked token");

            Value = newValue;
            IsRotated = true;
            RotatedAt = DateTime.UtcNow;
            RotatedBy = rotatedBy;
            if (!string.IsNullOrEmpty(reason))
                RevocationReason = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = rotatedBy;
        }

        public void RecordUsage()
        {
            UsageCount++;
            LastUsedAt = DateTime.UtcNow;
        }

        public void Deactivate(string? reason = null, string deactivatedBy = "System")
        {
            Guard.AgainstNullOrEmpty(deactivatedBy, nameof(deactivatedBy));

            IsActive = false;
            if (!string.IsNullOrEmpty(reason))
                RevocationReason = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = deactivatedBy;
        }

        public void Activate(string? reason = null, string activatedBy = "System")
        {
            Guard.AgainstNullOrEmpty(activatedBy, nameof(activatedBy));

            IsActive = true;
            if (!string.IsNullOrEmpty(reason))
                RevocationReason = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = activatedBy;
        }

        /// <summary>
        /// Checks if this token is expired
        /// </summary>
        public bool IsExpired()
        {
            return ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
        }

        /// <summary>
        /// Checks if this token is valid
        /// </summary>
        public bool IsValid()
        {
            return IsActive && !IsRevoked && !IsExpired();
        }

        /// <summary>
        /// Checks if this token is from the same device
        /// </summary>
        public bool IsFromSameDevice(string? deviceInfo, string? ipAddress, string? userAgent)
        {
            return DeviceInfo == deviceInfo && IpAddress == ipAddress && UserAgent == userAgent;
        }

        /// <summary>
        /// Checks if this token is a refresh token
        /// </summary>
        public bool IsRefreshToken()
        {
            return TokenPurpose?.Equals("Refresh", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Checks if this token is an access token
        /// </summary>
        public bool IsAccessToken()
        {
            return TokenPurpose?.Equals("Access", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Checks if this token is a bearer token
        /// </summary>
        public bool IsBearerToken()
        {
            return TokenType?.Equals("Bearer", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Checks if this token has been rotated
        /// </summary>
        public bool HasBeenRotated()
        {
            return IsRotated;
        }

        /// <summary>
        /// Checks if this token has a parent token
        /// </summary>
        public bool HasParentToken()
        {
            return !string.IsNullOrEmpty(ParentTokenId);
        }

        /// <summary>
        /// Gets the remaining time until expiration
        /// </summary>
        public TimeSpan? GetRemainingTime()
        {
            if (!ExpiresAt.HasValue || IsExpired())
                return null;

            return ExpiresAt.Value - DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the time since creation
        /// </summary>
        public TimeSpan GetTimeSinceCreation()
        {
            return DateTime.UtcNow - CreatedAt;
        }

        /// <summary>
        /// Gets the time since last usage
        /// </summary>
        public TimeSpan? GetTimeSinceLastUsage()
        {
            if (!LastUsedAt.HasValue)
                return null;

            return DateTime.UtcNow - LastUsedAt.Value;
        }

        /// <summary>
        /// Gets a human-readable description of the remaining time
        /// </summary>
        public string GetRemainingTimeDescription()
        {
            var remaining = GetRemainingTime();
            if (!remaining.HasValue)
                return "Never expires";

            if (remaining.Value.TotalDays >= 1)
                return $"Expires in {remaining.Value.Days} days";
            
            if (remaining.Value.TotalHours >= 1)
                return $"Expires in {remaining.Value.Hours} hours";
            
            if (remaining.Value.TotalMinutes >= 1)
                return $"Expires in {remaining.Value.Minutes} minutes";
            
            return "Expires soon";
        }

        /// <summary>
        /// Gets a human-readable description of the time since last usage
        /// </summary>
        public string GetLastUsageDescription()
        {
            if (!LastUsedAt.HasValue)
                return "Never used";

            var timeSince = GetTimeSinceLastUsage();
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
        /// Gets the token type
        /// </summary>
        public string GetTokenType()
        {
            return TokenType ?? "Bearer";
        }

        /// <summary>
        /// Gets the token purpose
        /// </summary>
        public string GetTokenPurpose()
        {
            return TokenPurpose ?? "Authentication";
        }

        /// <summary>
        /// Gets the usage count
        /// </summary>
        public int GetUsageCount()
        {
            return UsageCount;
        }

        /// <summary>
        /// Checks if this token requires attention
        /// </summary>
        public bool RequiresAttention()
        {
            // Check if expired
            if (IsExpired())
                return true;

            // Check if expiring soon (within 1 hour)
            var remaining = GetRemainingTime();
            if (remaining.HasValue && remaining.Value.TotalHours <= 1)
                return true;

            // Check if high usage count
            if (UsageCount > 100)
                return true;

            // Check if not used for a long time
            var timeSinceLastUsage = GetTimeSinceLastUsage();
            if (timeSinceLastUsage.HasValue && timeSinceLastUsage.Value.TotalDays > 30)
                return true;

            return false;
        }

        /// <summary>
        /// Gets the status of this token
        /// </summary>
        public string GetStatus()
        {
            if (!IsActive)
                return "Inactive";
            
            if (IsRevoked)
                return "Revoked";
            
            if (IsExpired())
                return "Expired";
            
            if (IsRotated)
                return "Rotated";
            
            return "Active";
        }

        /// <summary>
        /// Gets a summary of this token
        /// </summary>
        public string GetTokenSummary()
        {
            var status = GetStatus();
            var type = GetTokenType();
            var purpose = GetTokenPurpose();
            var remaining = GetRemainingTimeDescription();
            var usage = GetUsageCount();
            
            return $"{status} - {type} ({purpose}) - {remaining} - Used {usage} times";
        }

        /// <summary>
        /// Gets the audit information for this token
        /// </summary>
        public (string action, string details, string? reason) GetAuditInfo()
        {
            var action = IsRevoked ? "Token Revoked" : "Token Created";
            var details = $"User: {UserId}, Type: {GetTokenType()}, Purpose: {GetTokenPurpose()}, Usage: {GetUsageCount()}";
            var reason = RevocationReason;
            
            return (action, details, reason);
        }

        /// <summary>
        /// Checks if this token conflicts with another token
        /// </summary>
        public bool ConflictsWith(UserToken other)
        {
            // Same user and same purpose
            if (UserId == other.UserId && GetTokenPurpose() == other.GetTokenPurpose())
            {
                // Check if both are active and not expired
                if (IsValid() && other.IsValid())
                {
                    // Check for device conflict
                    if (IsFromSameDevice(other.DeviceInfo, other.IpAddress, other.UserAgent))
                        return true;
                }
            }

            return false;
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
    }
} 