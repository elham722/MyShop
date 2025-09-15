using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = null!;
        public string? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? AssignedAt { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public string? AssignedBy { get; private set; }
        public string? AssignmentReason { get; private set; }

        private UserRole() { } // For EF Core

        public static UserRole Create(string userId, string roleId, string? assignedBy = null, string? assignmentReason = null, DateTime? expiresAt = null, string createdBy = "System")
        {
            Guard.AgainstNullOrEmpty(userId, nameof(userId));
            Guard.AgainstNullOrEmpty(roleId, nameof(roleId));
            
            return new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                IsActive = true,
                AssignedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                AssignedBy = assignedBy ?? createdBy,
                AssignmentReason = assignmentReason
            };
        }

        public void UpdateAssignmentReason(string assignmentReason, string updatedBy)
        {
            Guard.AgainstNullOrEmpty(assignmentReason, nameof(assignmentReason));
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

            AssignmentReason = assignmentReason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        public void ExtendExpiration(DateTime newExpiresAt, string extendedBy)
        {
            Guard.AgainstNullOrEmpty(extendedBy, nameof(extendedBy));

            if (newExpiresAt <= DateTime.UtcNow)
                throw new CustomValidationException("New expiration date must be in the future");

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

        public bool IsActiveAndNotExpired()
        {
            return IsActive && !IsExpired();
        }

        public TimeSpan? GetRemainingTime()
        {
            if (!ExpiresAt.HasValue || IsExpired())
                return null;

            return ExpiresAt.Value - DateTime.UtcNow;
        }
    }
} 