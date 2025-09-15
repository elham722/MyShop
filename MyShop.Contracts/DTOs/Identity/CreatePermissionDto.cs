using MyShop.Contracts.Enums.Identity;

namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Data Transfer Object for creating a new Permission
    /// </summary>
    public class CreatePermissionDto
    {
        public string Name { get; set; } = string.Empty;
        public Resource Resource { get; set; }
        public ActionEnum Action { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Priority { get; set; } = 0;
        public bool IsSystemPermission { get; set; } = false;
    }
}