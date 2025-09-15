namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Data Transfer Object for creating a new Role
    /// </summary>
    public class CreateRoleDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Priority { get; set; } = 0;
        public bool IsSystemRole { get; set; } = false;
    }
}