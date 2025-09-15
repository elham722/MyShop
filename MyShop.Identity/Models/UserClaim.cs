using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    /// <summary>
    /// Custom UserClaim for dynamic business claims that need management and expiration
    /// For JWT token claims, use the default IdentityUserClaim<string>
    /// </summary>
    public class UserClaim : IdentityUserClaim<string>
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = null!;
        public string? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public string? Category { get; private set; }

        private UserClaim() { } // For EF Core

        public static UserClaim Create(string userId, string claimType, string claimValue, string? category = null, DateTime? expiresAt = null, string createdBy = "System")
        {
            Guard.AgainstNullOrEmpty(userId, nameof(userId));
            Guard.AgainstNullOrEmpty(claimType, nameof(claimType));
            Guard.AgainstNullOrEmpty(claimValue, nameof(claimValue));
          
            return new UserClaim
            {
                UserId = userId,
                ClaimType = claimType,
                ClaimValue = claimValue,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                IsActive = true,
                ExpiresAt = expiresAt,
                Category = category
            };
        }

        public void Update(string claimValue, string updatedBy)
        {
            Guard.AgainstNullOrEmpty(claimValue, nameof(claimValue));
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

            ClaimValue = claimValue;
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

        public void ExtendExpiration(DateTime newExpiresAt, string extendedBy)
        {
            Guard.AgainstNullOrEmpty(extendedBy, nameof(extendedBy));

            if (newExpiresAt <= DateTime.UtcNow)
                throw new ArgumentException("New expiration date must be in the future", nameof(newExpiresAt));

            ExpiresAt = newExpiresAt;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = extendedBy;
        }

        public void RemoveExpiration(string removedBy)
        {
            Guard.AgainstNullOrEmpty(removedBy, nameof(removedBy));

            ExpiresAt = null;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = removedBy;
        }

        /// <summary>
        /// Checks if this claim is expired
        /// </summary>
        public bool IsExpired()
        {
            return ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
        }

        /// <summary>
        /// Checks if this claim is valid (active and not expired)
        /// </summary>
        public bool IsValid()
        {
            return IsActive && !IsExpired();
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
        /// Gets a human-readable description of the expiration
        /// </summary>
        public string GetExpirationDescription()
        {
            if (!ExpiresAt.HasValue)
                return "Never expires";
            
            if (IsExpired())
                return "Expired";
            
            var remaining = GetRemainingTime();
            if (remaining.HasValue)
            {
                if (remaining.Value.TotalDays >= 1)
                    return $"Expires in {remaining.Value.Days} days";
                
                if (remaining.Value.TotalHours >= 1)
                    return $"Expires in {remaining.Value.Hours} hours";
                
                return $"Expires in {remaining.Value.Minutes} minutes";
            }
            
            return "Expires soon";
        }

        /// <summary>
        /// Checks if this is a business claim (not a system claim)
        /// </summary>
        public bool IsBusinessClaim()
        {
            return Category != "System";
        }

        /// <summary>
        /// Checks if this is a temporary claim
        /// </summary>
        public bool IsTemporaryClaim()
        {
            return ExpiresAt.HasValue;
        }

        /// <summary>
        /// Gets the claim category
        /// </summary>
        public string GetCategory()
        {
            return Category ?? "Default";
        }
    }
} 