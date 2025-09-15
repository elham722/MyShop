using MyShop.Contracts.Enums.Identity;

namespace MyShop.Contracts.DTOs.Identity
{
    public class AuditLogDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string? UserAgent { get; set; }
        public string? DeviceInfo { get; set; }
        public string? SessionId { get; set; }
        public string? RequestId { get; set; }
        public string? AdditionalData { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public AuditSeverity Severity { get; set; }
        
        // Navigation Properties (optional - for when user info is included)
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }
}
