namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Data Transfer Object for Role
    /// </summary>
    public class RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemRole { get; set; }
        public int Priority { get; set; }
        public string? Category { get; set; }
        
        // Navigation Properties (optional - for when permissions are included)
        public ICollection<PermissionDto>? Permissions { get; set; }
    }
}