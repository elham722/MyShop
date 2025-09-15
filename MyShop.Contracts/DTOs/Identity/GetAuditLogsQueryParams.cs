using MyShop.Contracts.Enums.Identity;

namespace MyShop.Contracts.DTOs.Identity
{
    public class GetAuditLogsQueryParams
    {
        public string? UserId { get; set; }
        public string? EntityType { get; set; }
        public string? EntityId { get; set; }
        public string? Action { get; set; }
        public AuditSeverity? Severity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsSuccess { get; set; }
        public string? IpAddress { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? SortBy { get; set; } = "Timestamp";
        public string? SortDirection { get; set; } = "desc";
    }
}