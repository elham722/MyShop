using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = null!;
        public string? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; }

        private UserClaim() { } // For EF Core

        public static UserClaim Create(string userId, string claimType, string claimValue, string createdBy = "System")
        {
            Guard.AgainstNullOrEmpty(userId, nameof(userId));
            Guard.AgainstNullOrEmpty(claimType, nameof(claimType));
            Guard.AgainstNullOrEmpty(claimValue, nameof(claimValue));
          
            return new UserClaim
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                IsActive = true
            };
        }

        public void Update(string claimValue, string updatedBy)
        {
            Guard.AgainstNullOrEmpty(claimValue, nameof(claimValue));
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

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

        public bool IsExpired(TimeSpan? expirationPeriod = null)
        {
            if (!expirationPeriod.HasValue)
                return false;

            return DateTime.UtcNow.Subtract(CreatedAt) > expirationPeriod.Value;
        }
    }
} 