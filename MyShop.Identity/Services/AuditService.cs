using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Identity.Enums;

namespace MyShop.Identity.Services;

/// <summary>
/// Service for managing audit logs and tracking user activities
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

    Task LogLogoutAsync(string userId, string? ipAddress = null, string? userAgent = null, 
        string? sessionId = null);

    Task LogRoleAssignmentAsync(string userId, string roleId, string action, 
        string? oldValues = null, string? newValues = null, string? ipAddress = null, 
        string? userAgent = null, string? additionalData = null);

    Task LogPermissionChangeAsync(string userId, string permissionId, string action, 
        string? oldValues = null, string? newValues = null, string? ipAddress = null, 
        string? userAgent = null, string? additionalData = null);

    Task LogTokenOperationAsync(string userId, string action, string? tokenId = null, 
        string? ipAddress = null, string? userAgent = null, string? additionalData = null, 
        bool isSuccess = true, string? errorMessage = null);

    Task<IEnumerable<AuditLog>> GetUserAuditLogsAsync(string userId, int pageNumber = 1, 
        int pageSize = 50);

    Task<IEnumerable<AuditLog>> GetEntityAuditLogsAsync(string entityType, string entityId, 
        int pageNumber = 1, int pageSize = 50);

    Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate, 
        int pageNumber = 1, int pageSize = 50);

    Task<IEnumerable<AuditLog>> GetAuditLogsBySeverityAsync(AuditSeverity severity, 
        int pageNumber = 1, int pageSize = 50);
}

/// <summary>
/// Implementation of audit service
/// </summary>
public class AuditService : IAuditService
{
    private readonly MyShopIdentityDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuditService(MyShopIdentityDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task LogUserActionAsync(string userId, string action, string entityType, string entityId, 
        string? oldValues = null, string? newValues = null, string? ipAddress = null, 
        string? userAgent = null, string? deviceInfo = null, string? sessionId = null, 
        string? requestId = null, string? additionalData = null, bool isSuccess = true, 
        string? errorMessage = null, AuditSeverity severity = AuditSeverity.Info)
    {
        var auditLog = AuditLog.Create(
            userId: userId,
            action: action,
            entityType: entityType,
            entityId: entityId,
            oldValues: oldValues,
            newValues: newValues,
            ipAddress: ipAddress,
            userAgent: userAgent,
            deviceInfo: deviceInfo,
            sessionId: sessionId,
            requestId: requestId,
            additionalData: additionalData,
            isSuccess: isSuccess,
            errorMessage: errorMessage,
            severity: severity
        );

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task LogLoginAsync(string userId, bool isSuccess, string? ipAddress = null, 
        string? userAgent = null, string? deviceInfo = null, string? sessionId = null, 
        string? errorMessage = null)
    {
        var auditLog = AuditLog.CreateForLogin(
            userId: userId,
            isSuccess: isSuccess,
            ipAddress: ipAddress,
            userAgent: userAgent,
            deviceInfo: deviceInfo,
            sessionId: sessionId,
            errorMessage: errorMessage
        );

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task LogLogoutAsync(string userId, string? ipAddress = null, string? userAgent = null, 
        string? sessionId = null)
    {
        var auditLog = AuditLog.CreateForLogout(
            userId: userId,
            ipAddress: ipAddress,
            userAgent: userAgent,
            sessionId: sessionId
        );

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task LogRoleAssignmentAsync(string userId, string roleId, string action, 
        string? oldValues = null, string? newValues = null, string? ipAddress = null, 
        string? userAgent = null, string? additionalData = null)
    {
        var auditLog = AuditLog.CreateForRoleAssignment(
            userId: userId,
            roleId: roleId,
            action: action,
            oldValues: oldValues,
            newValues: newValues,
            ipAddress: ipAddress,
            userAgent: userAgent,
            additionalData: additionalData
        );

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task LogPermissionChangeAsync(string userId, string permissionId, string action, 
        string? oldValues = null, string? newValues = null, string? ipAddress = null, 
        string? userAgent = null, string? additionalData = null)
    {
        var auditLog = AuditLog.CreateForPermissionChange(
            userId: userId,
            permissionId: permissionId,
            action: action,
            oldValues: oldValues,
            newValues: newValues,
            ipAddress: ipAddress,
            userAgent: userAgent,
            additionalData: additionalData
        );

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task LogTokenOperationAsync(string userId, string action, string? tokenId = null, 
        string? ipAddress = null, string? userAgent = null, string? additionalData = null, 
        bool isSuccess = true, string? errorMessage = null)
    {
        var auditLog = AuditLog.CreateForTokenOperation(
            userId: userId,
            action: action,
            tokenId: tokenId,
            ipAddress: ipAddress,
            userAgent: userAgent,
            additionalData: additionalData,
            isSuccess: isSuccess,
            errorMessage: errorMessage
        );

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetUserAuditLogsAsync(string userId, int pageNumber = 1, 
        int pageSize = 50)
    {
        return await _context.AuditLogs
            .Where(al => al.UserId == userId)
            .OrderByDescending(al => al.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetEntityAuditLogsAsync(string entityType, string entityId, 
        int pageNumber = 1, int pageSize = 50)
    {
        return await _context.AuditLogs
            .Where(al => al.EntityType == entityType && al.EntityId == entityId)
            .OrderByDescending(al => al.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate, 
        int pageNumber = 1, int pageSize = 50)
    {
        return await _context.AuditLogs
            .Where(al => al.Timestamp >= startDate && al.Timestamp <= endDate)
            .OrderByDescending(al => al.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetAuditLogsBySeverityAsync(AuditSeverity severity, 
        int pageNumber = 1, int pageSize = 50)
    {
        return await _context.AuditLogs
            .Where(al => al.Severity == severity)
            .OrderByDescending(al => al.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}