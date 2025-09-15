using MyShop.Contracts.Enums.Identity;

namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Data Transfer Object for updating a Permission
    /// </summary>
    public class UpdatePermissionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Resource Resource { get; set; }
        public ActionEnum Action { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int? Priority { get; set; }
        public bool? IsActive { get; set; }
    }
}