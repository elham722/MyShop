namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Data Transfer Object for RolePermission relationship
    /// </summary>
    public class RolePermissionDto
    {
        public string RoleId { get; set; } = string.Empty;
        public string PermissionId { get; set; } = string.Empty;
        public bool IsGranted { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        
        // Navigation Properties
        public RoleDto? Role { get; set; }
        public PermissionDto? Permission { get; set; }
        
        // Computed Properties
        public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
        public bool IsValid => IsActive && IsGranted && !IsExpired;
    }
}