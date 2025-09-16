using MyShop.Contracts.Enums.Identity;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.Common;

namespace MyShop.Contracts.Identity.Services;
/// <summary>
/// Contract for auditing user and system activities
/// </summary>
public interface IAuditService
{
    Task LogUserActionAsync(string userId, string action, string entityType, string entityId,
        string? oldValues = null, string? newValues = null, string? ipAddress = null,
        string? userAgent = null, string? deviceInfo = null, string? sessionId = null,
        string? requestId = null, string? additionalData = null, bool isSuccess = true,
        string? errorMessage = null, AuditSeverity severity = AuditSeverity.Info);

    Task LogLoginAsync(string userId, bool isSuccess, string? ipAddress = null,
        string? userAgent = null, string? deviceInfo = null, string? sessionId = null,
        string? errorMessage = null);

    Task LogLogoutAsync(string userId, string? ipAddress = null,
        string? userAgent = null, string? sessionId = null);


    Task LogRoleAssignmentAsync(string userId, string roleId, string action,
        string? oldValues = null, string? newValues = null, string? ipAddress = null,
        string? userAgent = null, string? additionalData = null);

    Task LogPermissionChangeAsync(string userId, string permissionId, string action,
        string? oldValues = null, string? newValues = null, string? ipAddress = null,
        string? userAgent = null, string? additionalData = null);

    Task LogTokenOperationAsync(string userId, string action, string? tokenId = null,
        string? ipAddress = null, string? userAgent = null, string? additionalData = null,
        bool isSuccess = true, string? errorMessage = null);

    Task<Result<IReadOnlyList<AuditLogDto>>> GetUserAuditLogsAsync(string userId, int pageNumber = 1,
        int pageSize = 50);

    Task<Result<IReadOnlyList<AuditLogDto>>> GetEntityAuditLogsAsync(string entityType, string entityId,
        int pageNumber = 1, int pageSize = 50);

    Task<Result<IReadOnlyList<AuditLogDto>>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate,
        int pageNumber = 1, int pageSize = 50);

    Task<Result<IReadOnlyList<AuditLogDto>>> GetAuditLogsBySeverityAsync(AuditSeverity severity,
        int pageNumber = 1, int pageSize = 50);

    Task<Result<IReadOnlyList<AuditLogDto>>> GetAuditLogsAsync(GetAuditLogsQueryParams queryParams);

    Task<Result<AuditLogDto>> GetAuditLogByIdAsync(string id);
}

