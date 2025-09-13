namespace MyShop.Contracts.DTOs.Options;

public class AuditInfo
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    public AuditInfo() { }
    
    public AuditInfo(string? userId, string? userName = null, string? ipAddress = null, string? userAgent = null)
    {
        UserId = userId;
        UserName = userName;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Timestamp = DateTime.UtcNow;
    }
}