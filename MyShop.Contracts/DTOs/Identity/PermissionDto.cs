using MyShop.Contracts.Enums.Identity;

namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Data Transfer Object for Permission
    /// </summary>
    public class PermissionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Resource Resource { get; set; }
        public ActionEnum Action { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemPermission { get; set; }
        public string? Category { get; set; }
        public int Priority { get; set; }
        
        // Computed Properties
        public string FullName => $"{Resource}.{Action}";
        public string DisplayName => $"{Resource.ToStringValue()} - {Action.ToStringValue()}";
    }
}