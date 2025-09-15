using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    public class Role : IdentityRole<string>
    {
        public string Description { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = null!;
        public string? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsSystemRole { get; private set; }
        public int Priority { get; private set; }
        public string? Category { get; private set; }

        private Role() { } // For EF Core

        public static Role Create(string name, string description, string? category = null, int priority = 0, bool isSystemRole = false, string createdBy = "System")
        {
            Guard.AgainstNullOrEmpty(name, nameof(name));
            Guard.AgainstNullOrEmpty(description, nameof(description));

            return new Role
            {
                Name = name,
                NormalizedName = name.ToUpperInvariant(),
                Description = description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                IsActive = true,
                IsSystemRole = isSystemRole,
                Priority = priority,
                Category = category
            };
        }

        public void Update(string name, string description, string? category = null, int? priority = null, string updatedBy = "System")
        {
            Guard.AgainstNullOrEmpty(name, nameof(name));
            Guard.AgainstNullOrEmpty(description, nameof(description));
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

            if (IsSystemRole)
                throw new CustomValidationException("Cannot update system roles");

            Name = name;
            NormalizedName = name.ToUpperInvariant();
            Description = description;
            Category = category;
            Priority = priority ?? Priority;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        public void Deactivate(string deactivatedBy)
        {
            Guard.AgainstNullOrEmpty(deactivatedBy, nameof(deactivatedBy));
            if (IsSystemRole)
                throw new CustomValidationException("Cannot deactivate system roles");

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

        public void ChangePriority(int newPriority, string changedBy)
        {
            Guard.AgainstNullOrEmpty(changedBy, nameof(changedBy));

            Priority = newPriority;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = changedBy;
        }
    }
}