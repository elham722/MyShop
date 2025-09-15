using MyShop.Contracts.Enums.Identity;

namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Query parameters for getting permissions
    /// </summary>
    public class GetPermissionsQueryParams
    {
        public string? Name { get; set; }
        public Resource? Resource { get; set; }
        public ActionEnum? Action { get; set; }
        public string? Category { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSystemPermission { get; set; }
        public int? MinPriority { get; set; }
        public int? MaxPriority { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? SortBy { get; set; } = "Name";
        public string? SortDirection { get; set; } = "asc";
    }
}