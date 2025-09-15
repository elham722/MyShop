namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Query parameters for getting roles
    /// </summary>
    public class GetRolesQueryParams
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSystemRole { get; set; }
        public int? MinPriority { get; set; }
        public int? MaxPriority { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? SortBy { get; set; } = "Name";
        public string? SortDirection { get; set; } = "asc";
    }
}