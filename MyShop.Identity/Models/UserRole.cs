using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    /// <summary>
    /// Custom UserRole for role assignment tracking with expiration and audit integration
    /// </summary>
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
        public string? AssignmentCategory { get; private set; }
        public int Priority { get; private set; }
        public bool IsTemporary { get; private set; }
        public string? Notes { get; private set; }

        private UserRole() { } // For EF Core

        public static UserRole Create(string userId, string roleId, string? assignedBy = null, string? assignmentReason = null, DateTime? expiresAt = null, string? assignmentCategory = null, int priority = 0, bool isTemporary = false, string? notes = null, string createdBy = "System")
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
                AssignmentReason = assignmentReason,
                AssignmentCategory = assignmentCategory ?? "Standard",
                Priority = priority,
                IsTemporary = isTemporary,
                Notes = notes
            };
        }

        public void UpdateAssignmentReason(string assignmentReason, string? assignmentCategory = null, string? notes = null, string updatedBy = "System")
        {
            Guard.AgainstNullOrEmpty(assignmentReason, nameof(assignmentReason));
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

            AssignmentReason = assignmentReason;
            AssignmentCategory = assignmentCategory ?? AssignmentCategory;
            Notes = notes ?? Notes;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        public void ExtendExpiration(DateTime newExpiresAt, string? reason = null, string extendedBy = "System")
        {
            Guard.AgainstNullOrEmpty(extendedBy, nameof(extendedBy));

            if (newExpiresAt <= DateTime.UtcNow)
                throw new CustomValidationException("New expiration date must be in the future");

            ExpiresAt = newExpiresAt;
            if (!string.IsNullOrEmpty(reason))
                AssignmentReason = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = extendedBy;
        }

        public void RemoveExpiration(string? reason = null, string removedBy = "System")
        {
            Guard.AgainstNullOrEmpty(removedBy, nameof(removedBy));

            ExpiresAt = null;
            if (!string.IsNullOrEmpty(reason))
                AssignmentReason = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = removedBy;
        }

        public void ChangePriority(int newPriority, string? reason = null, string changedBy = "System")
        {
            Guard.AgainstNullOrEmpty(changedBy, nameof(changedBy));

            Priority = newPriority;
            if (!string.IsNullOrEmpty(reason))
                AssignmentReason = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = changedBy;
        }

        public void MarkAsTemporary(bool isTemporary, string? reason = null, string changedBy = "System")
        {
            Guard.AgainstNullOrEmpty(changedBy, nameof(changedBy));

            IsTemporary = isTemporary;
            if (!string.IsNullOrEmpty(reason))
                AssignmentReason = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = changedBy;
        }

        public void Deactivate(string? reason = null, string deactivatedBy = "System")
        {
            Guard.AgainstNullOrEmpty(deactivatedBy, nameof(deactivatedBy));

            IsActive = false;
            if (!string.IsNullOrEmpty(reason))
                AssignmentReason = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = deactivatedBy;
        }

        public void Activate(string? reason = null, string activatedBy = "System")
        {
            Guard.AgainstNullOrEmpty(activatedBy, nameof(activatedBy));

            IsActive = true;
            if (!string.IsNullOrEmpty(reason))
                AssignmentReason = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = activatedBy;
        }

        /// <summary>
        /// Checks if this role assignment is expired
        /// </summary>
        public bool IsExpired()
        {
            return ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
        }

        /// <summary>
        /// Checks if this role assignment is active and not expired
        /// </summary>
        public bool IsActiveAndNotExpired()
        {
            return IsActive && !IsExpired();
        }

        /// <summary>
        /// Checks if this role assignment is temporary
        /// </summary>
        public bool IsTemporaryAssignment()
        {
            return IsTemporary || ExpiresAt.HasValue;
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
        /// Gets the time since assignment
        /// </summary>
        public TimeSpan GetTimeSinceAssignment()
        {
            return DateTime.UtcNow - AssignedAt.Value;
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
        /// Gets a human-readable description of the time since assignment
        /// </summary>
        public string GetAssignmentTimeDescription()
        {
            var timeSince = GetTimeSinceAssignment();
            
            if (timeSince.TotalDays >= 1)
                return $"Assigned {timeSince.Days} days ago";
            
            if (timeSince.TotalHours >= 1)
                return $"Assigned {timeSince.Hours} hours ago";
            
            if (timeSince.TotalMinutes >= 1)
                return $"Assigned {timeSince.Minutes} minutes ago";
            
            return "Just assigned";
        }

        /// <summary>
        /// Gets the assignment category
        /// </summary>
        public string GetAssignmentCategory()
        {
            return AssignmentCategory ?? "Standard";
        }

        /// <summary>
        /// Gets the priority level
        /// </summary>
        public int GetPriority()
        {
            return Priority == 0 ? 5 : Priority; // Default priority is 5
        }

        /// <summary>
        /// Checks if this assignment requires special attention
        /// </summary>
        public bool RequiresAttention()
        {
            // Check if expired
            if (IsExpired())
                return true;

            // Check if expiring soon (within 7 days)
            var remaining = GetRemainingTime();
            if (remaining.HasValue && remaining.Value.TotalDays <= 7)
                return true;

            // Check if temporary and high priority
            if (IsTemporary && Priority <= 3)
                return true;

            return false;
        }

        /// <summary>
        /// Gets the status of this role assignment
        /// </summary>
        public string GetStatus()
        {
            if (!IsActive)
                return "Inactive";
            
            if (IsExpired())
                return "Expired";
            
            if (IsTemporary)
                return "Temporary";
            
            return "Active";
        }

        /// <summary>
        /// Gets a summary of this role assignment
        /// </summary>
        public string GetAssignmentSummary()
        {
            var status = GetStatus();
            var remaining = GetRemainingTimeDescription();
            var category = GetAssignmentCategory();
            var priority = GetPriority();
            
            return $"{status} - {category} (Priority: {priority}) - {remaining}";
        }

        /// <summary>
        /// Gets the audit information for this assignment
        /// </summary>
        public (string action, string details, string? reason) GetAuditInfo()
        {
            var action = IsActive ? "Role Assigned" : "Role Deactivated";
            var details = $"User: {UserId}, Role: {RoleId}, Category: {GetAssignmentCategory()}, Priority: {GetPriority()}";
            var reason = AssignmentReason;
            
            return (action, details, reason);
        }

        /// <summary>
        /// Checks if this assignment conflicts with another assignment
        /// </summary>
        public bool ConflictsWith(UserRole other)
        {
            // Same user and role
            if (UserId == other.UserId && RoleId == other.RoleId)
                return true;

            // Both active and overlapping time periods
            if (IsActiveAndNotExpired() && other.IsActiveAndNotExpired())
            {
                // Check for time overlap if both have expiration dates
                if (ExpiresAt.HasValue && other.ExpiresAt.HasValue)
                {
                    var thisStart = AssignedAt;
                    var thisEnd = ExpiresAt.Value;
                    var otherStart = other.AssignedAt;
                    var otherEnd = other.ExpiresAt.Value;

                    return thisStart < otherEnd && otherStart < thisEnd;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the effective priority (considering temporary status)
        /// </summary>
        public int GetEffectivePriority()
        {
            var basePriority = GetPriority();
            
            // Temporary assignments get higher priority (lower number)
            if (IsTemporary)
                return Math.Max(1, basePriority - 1);
            
            return basePriority;
        }
    }
} 