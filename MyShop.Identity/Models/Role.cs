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

        // Navigation Properties
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

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

        /// <summary>
        /// Checks if this role is a system role
        /// </summary>
        public bool IsSystemRoleType()
        {
            return IsSystemRole;
        }

        /// <summary>
        /// Checks if this role requires special privileges to assign
        /// </summary>
        public bool RequiresSpecialPrivileges()
        {
            return Name switch
            {
                "SuperAdmin" or "SystemAdmin" or "Admin" or "Auditor" => true,
                _ => false
            };
        }

        /// <summary>
        /// Gets the category of this role
        /// </summary>
        public string GetRoleCategory()
        {
            return Category ?? Name switch
            {
                "SuperAdmin" or "SystemAdmin" => "System",
                "Admin" or "Manager" => "Administrative",
                "CustomerService" or "SalesRep" or "SupportAgent" => "Business",
                "Customer" or "Guest" => "User",
                "Auditor" or "ReportViewer" => "Specialized",
                _ => "Custom"
            };
        }

        /// <summary>
        /// Checks if this role can be assigned to users
        /// </summary>
        public bool IsAssignable()
        {
            return !IsSystemRole && IsActive;
        }

        /// <summary>
        /// Checks if this role can be modified
        /// </summary>
        public bool CanBeModified()
        {
            return !IsSystemRole;
        }

        /// <summary>
        /// Checks if this role can be deleted
        /// </summary>
        public bool CanBeDeleted()
        {
            return !IsSystemRole;
        }

        /// <summary>
        /// Gets a display-friendly name for the role
        /// </summary>
        public string GetDisplayName()
        {
            return Name switch
            {
                "SuperAdmin" => "Super Administrator",
                "SystemAdmin" => "System Administrator",
                "CustomerService" => "Customer Service",
                "SalesRep" => "Sales Representative",
                "SupportAgent" => "Support Agent",
                "ReportViewer" => "Report Viewer",
                _ => Name
            };
        }

        /// <summary>
        /// Gets the priority level for sorting
        /// </summary>
        public int GetSortPriority()
        {
            return Priority == 0 ? GetDefaultPriority() : Priority;
        }

        private int GetDefaultPriority()
        {
            return Name switch
            {
                "SuperAdmin" => 1,
                "SystemAdmin" => 2,
                "Admin" => 3,
                "Manager" => 4,
                "CustomerService" => 5,
                "Auditor" => 5,
                "SalesRep" => 6,
                "ReportViewer" => 6,
                "SupportAgent" => 7,
                "Customer" => 8,
                "Guest" => 9,
                _ => 10
            };
        }
    }
}