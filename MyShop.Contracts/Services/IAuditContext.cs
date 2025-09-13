namespace MyShop.Contracts.Services;
public interface IAuditContext
{
    string? CurrentUserId { get; }

    string? CurrentUserName { get; }

    string? IpAddress { get; }

    string? UserAgent { get; }

    DateTime Timestamp { get; }
}