using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Contracts.Enums.Identity;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.Common;
using Mapster;
using MyShop.Contracts.Identity.Services.Audit;

namespace MyShop.Identity.Services.Audit;

public class AuditService : IAuditService
{
    private readonly MyShopIdentityDbContext _context;

    public AuditService(MyShopIdentityDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Logging Methods

    public async Task LogUserActionAsync(string userId, string action, string entityType, string entityId,
        string? oldValues = null, string? newValues = null, string? ipAddress = null,
        string? userAgent = null, string? deviceInfo = null, string? sessionId = null,
        string? requestId = null, string? additionalData = null, bool isSuccess = true,
        string? errorMessage = null, AuditSeverity severity = AuditSeverity.Info)
    {
        var log = AuditLog.Create(userId, action, entityType, entityId, oldValues, newValues,
            ipAddress, userAgent, deviceInfo, sessionId, requestId, additionalData,
            isSuccess, errorMessage, severity);

        await AddLogAsync(log);
    }

    public async Task LogLoginAsync(string userId, bool isSuccess, string? ipAddress = null,
        string? userAgent = null, string? deviceInfo = null, string? sessionId = null,
        string? errorMessage = null)
    {
        var log = AuditLog.CreateForLogin(userId, isSuccess, ipAddress, userAgent,
            deviceInfo, sessionId, errorMessage);

        await AddLogAsync(log);
    }

    public async Task LogLogoutAsync(string userId, string? ipAddress = null,
        string? userAgent = null, string? sessionId = null)
    {
        var log = AuditLog.CreateForLogout(userId, ipAddress, userAgent, sessionId);
        await AddLogAsync(log);
    }

    public async Task LogRoleAssignmentAsync(string userId, string roleId, string action,
        string? oldValues = null, string? newValues = null, string? ipAddress = null,
        string? userAgent = null, string? additionalData = null)
    {
        var log = AuditLog.CreateForRoleAssignment(userId, roleId, action,
            oldValues, newValues, ipAddress, userAgent, additionalData);

        await AddLogAsync(log);
    }

    public async Task LogPermissionChangeAsync(string userId, string permissionId, string action,
        string? oldValues = null, string? newValues = null, string? ipAddress = null,
        string? userAgent = null, string? additionalData = null)
    {
        var log = AuditLog.CreateForPermissionChange(userId, permissionId, action,
            oldValues, newValues, ipAddress, userAgent, additionalData);

        await AddLogAsync(log);
    }

    public async Task LogTokenOperationAsync(string userId, string action, string? tokenId = null,
        string? ipAddress = null, string? userAgent = null, string? additionalData = null,
        bool isSuccess = true, string? errorMessage = null)
    {
        var log = AuditLog.CreateForTokenOperation(userId, action, tokenId,
            ipAddress, userAgent, additionalData, isSuccess, errorMessage);

        await AddLogAsync(log);
    }

    #endregion

    #region Query Methods

    public async Task<Result<IReadOnlyList<AuditLogDto>>> GetUserAuditLogsAsync(string userId, int pageNumber = 1, int pageSize = 50)
    {
        try
        {
            var logs = await GetLogsAsync(al => al.UserId == userId, pageNumber, pageSize);
            return Result<IReadOnlyList<AuditLogDto>>.Success(logs);
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyList<AuditLogDto>>.Failure($"Failed to get user audit logs: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyList<AuditLogDto>>> GetEntityAuditLogsAsync(string entityType, string entityId,
        int pageNumber = 1, int pageSize = 50)
    {
        try
        {
            var logs = await GetLogsAsync(al => al.EntityType == entityType && al.EntityId == entityId, pageNumber, pageSize);
            return Result<IReadOnlyList<AuditLogDto>>.Success(logs);
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyList<AuditLogDto>>.Failure($"Failed to get entity audit logs: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyList<AuditLogDto>>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate,
        int pageNumber = 1, int pageSize = 50)
    {
        try
        {
            var logs = await GetLogsAsync(al => al.Timestamp >= startDate && al.Timestamp <= endDate, pageNumber, pageSize);
            return Result<IReadOnlyList<AuditLogDto>>.Success(logs);
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyList<AuditLogDto>>.Failure($"Failed to get audit logs by date range: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyList<AuditLogDto>>> GetAuditLogsBySeverityAsync(AuditSeverity severity,
        int pageNumber = 1, int pageSize = 50)
    {
        try
        {
            var logs = await GetLogsAsync(al => al.Severity == severity, pageNumber, pageSize);
            return Result<IReadOnlyList<AuditLogDto>>.Success(logs);
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyList<AuditLogDto>>.Failure($"Failed to get audit logs by severity: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyList<AuditLogDto>>> GetAuditLogsAsync(GetAuditLogsQueryParams queryParams)
    {
        try
        {
            var query = _context.AuditLogs.Include(al => al.User).AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(queryParams.UserId))
                query = query.Where(al => al.UserId == queryParams.UserId);

            if (!string.IsNullOrEmpty(queryParams.EntityType))
                query = query.Where(al => al.EntityType == queryParams.EntityType);

            if (!string.IsNullOrEmpty(queryParams.EntityId))
                query = query.Where(al => al.EntityId == queryParams.EntityId);

            if (!string.IsNullOrEmpty(queryParams.Action))
                query = query.Where(al => al.Action == queryParams.Action);

            if (queryParams.Severity.HasValue)
                query = query.Where(al => al.Severity == queryParams.Severity.Value);

            if (queryParams.StartDate.HasValue)
                query = query.Where(al => al.Timestamp >= queryParams.StartDate.Value);

            if (queryParams.EndDate.HasValue)
                query = query.Where(al => al.Timestamp <= queryParams.EndDate.Value);

            if (queryParams.IsSuccess.HasValue)
                query = query.Where(al => al.IsSuccess == queryParams.IsSuccess.Value);

            if (!string.IsNullOrEmpty(queryParams.IpAddress))
                query = query.Where(al => al.IpAddress == queryParams.IpAddress);

            // Apply sorting
            query = queryParams.SortBy?.ToLowerInvariant() switch
            {
                "timestamp" => queryParams.SortDirection?.ToLowerInvariant() == "asc" 
                    ? query.OrderBy(al => al.Timestamp) 
                    : query.OrderByDescending(al => al.Timestamp),
                "action" => queryParams.SortDirection?.ToLowerInvariant() == "asc" 
                    ? query.OrderBy(al => al.Action) 
                    : query.OrderByDescending(al => al.Action),
                "severity" => queryParams.SortDirection?.ToLowerInvariant() == "asc" 
                    ? query.OrderBy(al => al.Severity) 
                    : query.OrderByDescending(al => al.Severity),
                _ => query.OrderByDescending(al => al.Timestamp)
            };

            // Apply pagination
            var logs = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var auditLogDtos = logs.Adapt<List<AuditLogDto>>();
            return Result<IReadOnlyList<AuditLogDto>>.Success(auditLogDtos);
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyList<AuditLogDto>>.Failure($"Failed to get audit logs: {ex.Message}");
        }
    }

    public async Task<Result<AuditLogDto>> GetAuditLogByIdAsync(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return Result<AuditLogDto>.Failure("Audit log ID cannot be null or empty");

            var log = await _context.AuditLogs
                .Include(al => al.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(al => al.Id == id);

            if (log == null)
                return Result<AuditLogDto>.Failure($"Audit log with ID '{id}' not found");

            var auditLogDto = log.Adapt<AuditLogDto>();
            return Result<AuditLogDto>.Success(auditLogDto);
        }
        catch (Exception ex)
        {
            return Result<AuditLogDto>.Failure($"Failed to get audit log by ID: {ex.Message}");
        }
    }

    #endregion

    #region Private Helpers

    private async Task AddLogAsync(AuditLog log)
    {
        await _context.AuditLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    private async Task<IReadOnlyList<AuditLogDto>> GetLogsAsync(
        Expression<Func<AuditLog, bool>> predicate, int pageNumber, int pageSize)
    {
        var logs = await _context.AuditLogs
            .Include(al => al.User)
            .Where(predicate) // ✅ حالا EF Core می‌فهمه
            .OrderByDescending(al => al.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return logs.Adapt<List<AuditLogDto>>();
    }


    #endregion
}
