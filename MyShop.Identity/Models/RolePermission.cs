using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    public class RolePermission
    {
        public string Id { get; private set; } = null!;
        public string RoleId { get; private set; } = null!;
        public string PermissionId { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = null!;
        public string? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? AssignedAt { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public string? AssignedBy { get; private set; }
        // AssignmentReason removed - use AuditLog for tracking assignment reasons
        public bool IsGranted { get; private set; } = true; // For future deny permissions

        // Navigation properties
        public Role Role { get; private set; } = null!;
        public Permission Permission { get; private set; } = null!;

        private RolePermission() { } // For EF Core

        public static RolePermission Create(string roleId, string permissionId, string? assignedBy = null, DateTime? expiresAt = null, bool isGranted = true, string createdBy = "System")
        {
            Guard.AgainstNullOrEmpty(roleId, nameof(roleId));
            Guard.AgainstNullOrEmpty(permissionId, nameof(permissionId));
           
            return new RolePermission
            {
                Id = Guid.NewGuid().ToString(),
                RoleId = roleId,
                PermissionId = permissionId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                IsActive = true,
                AssignedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                AssignedBy = assignedBy ?? createdBy,
                IsGranted = isGranted
            };
        }

        // UpdateAssignmentReason removed - use AuditLog for tracking assignment reasons

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

        public void ToggleGrant(string toggledBy)
        {
            Guard.AgainstNullOrEmpty(toggledBy, nameof(toggledBy));

            IsGranted = !IsGranted;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = toggledBy;
        }

        public bool IsExpired()
        {
            return ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
        }

        public bool IsActiveAndNotExpired()
        {
            return IsActive && !IsExpired();
        }

        public bool IsValid()
        {
            return IsActiveAndNotExpired() && IsGranted;
        }

        public TimeSpan? GetRemainingTime()
        {
            if (!ExpiresAt.HasValue || IsExpired())
                return null;

            return ExpiresAt.Value - DateTime.UtcNow;
        }

        /// <summary>
        /// Checks if this assignment can be modified
        /// </summary>
        public bool CanBeModified()
        {
            return IsActive && !IsExpired();
        }

        /// <summary>
        /// Checks if this assignment can be revoked
        /// </summary>
        public bool CanBeRevoked()
        {
            return IsActive && IsGranted;
        }

        /// <summary>
        /// Checks if this assignment can be granted
        /// </summary>
        public bool CanBeGranted()
        {
            return IsActive && !IsGranted;
        }

        /// <summary>
        /// Gets the status of this assignment
        /// </summary>
        public string GetStatus()
        {
            if (!IsActive)
                return "Inactive";
            
            if (IsExpired())
                return "Expired";
            
            if (!IsGranted)
                return "Denied";
            
            return "Active";
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
        /// Checks if this assignment is for a system role
        /// </summary>
        public bool IsSystemRoleAssignment()
        {
            return Role?.IsSystemRole == true;
        }

        /// <summary>
        /// Checks if this assignment is for a system permission
        /// </summary>
        public bool IsSystemPermissionAssignment()
        {
            return Permission?.IsSystemPermission == true;
        }

        /// <summary>
        /// Checks if this assignment requires special privileges to modify
        /// </summary>
        public bool RequiresSpecialPrivilegesToModify()
        {
            return IsSystemRoleAssignment() || IsSystemPermissionAssignment();
        }

        /// <summary>
        /// Gets the assignment duration
        /// </summary>
        public TimeSpan GetAssignmentDuration()
        {
            if (!AssignedAt.HasValue)
                throw new CustomValidationException("AssignedAt must have a value to calculate the duration.");
            return DateTime.UtcNow - AssignedAt.Value;
        }

        /// <summary>
        /// Gets a summary of this assignment
        /// </summary>
        public string GetAssignmentSummary()
        {
            var status = GetStatus();
            var expiration = GetExpirationDescription();
            var duration = GetAssignmentDuration();
            
            return $"Role: {Role?.Name}, Permission: {Permission?.Name}, Status: {status}, {expiration}, Duration: {duration.Days} days";
        }
    }
}