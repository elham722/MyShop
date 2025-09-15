namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Data Transfer Object for updating a Role
    /// </summary>
    public class UpdateRoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int? Priority { get; set; }
        public bool? IsActive { get; set; }
    }
}