using Microsoft.AspNetCore.Identity;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    public class Permission
    {
        public string Id { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string Resource { get; private set; } = null!;
        public string Action { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = null!;
        public string? UpdatedBy { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsSystemPermission { get; private set; }
        public string? Category { get; private set; }
        public int Priority { get; private set; }

        private Permission() { } // For EF Core

        public static Permission Create(string name, string resource, string action, string description, string? category = null, int priority = 0, bool isSystemPermission = false, string createdBy = "System")
        {
            Guard.AgainstNullOrEmpty(name, nameof(name));
            Guard.AgainstNullOrEmpty(resource, nameof(resource));
            Guard.AgainstNullOrEmpty(action, nameof(action));
            Guard.AgainstNullOrEmpty(description, nameof(description));
          
            return new Permission
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Resource = resource,
                Action = action,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                IsActive = true,
                IsSystemPermission = isSystemPermission,
                Category = category,
                Priority = priority
            };
        }

        public void Update(string name, string resource, string action, string description, string? category = null, int? priority = null, string updatedBy = "System")
        {
            Guard.AgainstNullOrEmpty(name, nameof(name));
            Guard.AgainstNullOrEmpty(resource, nameof(resource));
            Guard.AgainstNullOrEmpty(action, nameof(action));
            Guard.AgainstNullOrEmpty(description, nameof(description));
            Guard.AgainstNullOrEmpty(updatedBy, nameof(updatedBy));

            if (IsSystemPermission)
                throw new CustomValidationException("Cannot update system permissions");

            Name = name;
            Resource = resource;
            Action = action;
            Description = description;
            Category = category;
            Priority = priority ?? Priority;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        public void Deactivate(string deactivatedBy)
        {
            Guard.AgainstNullOrEmpty(deactivatedBy, nameof(deactivatedBy));

            if (IsSystemPermission)
                throw new CustomValidationException("Cannot deactivate system permissions");

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

        public string GetFullName()
        {
            return $"{Resource}:{Action}";
        }

        public bool MatchesResourceAction(string resource, string action)
        {
            return Resource.Equals(resource, StringComparison.OrdinalIgnoreCase) &&
                   Action.Equals(action, StringComparison.OrdinalIgnoreCase);
        }
    }
}