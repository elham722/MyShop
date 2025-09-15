using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Identity.Enums;

namespace MyShop.Identity.Services;

/// <summary>
/// Service for managing Identity statistics and analytics
/// </summary>
public interface IIdentityStatisticsService
{
    Task<IdentityStatistics> GetOverallStatisticsAsync();
    Task<UserStatistics> GetUserStatisticsAsync();
    Task<RoleStatistics> GetRoleStatisticsAsync();
    Task<PermissionStatistics> GetPermissionStatisticsAsync();
    Task<AuditStatistics> GetAuditStatisticsAsync();
    Task<LoginStatistics> GetLoginStatisticsAsync(DateTime startDate, DateTime endDate);
    Task<SecurityStatistics> GetSecurityStatisticsAsync();
    Task<Dictionary<string, int>> GetUserCountByRoleAsync();
    Task<Dictionary<string, int>> GetUserCountByStatusAsync();
    Task<Dictionary<string, int>> GetPermissionCountByResourceAsync();
    Task<Dictionary<string, int>> GetPermissionCountByCategoryAsync();
    Task<Dictionary<string, int>> GetAuditCountBySeverityAsync();
    Task<Dictionary<string, int>> GetAuditCountByActionAsync();
    Task<List<RecentActivity>> GetRecentActivitiesAsync(int count = 50);
    Task<List<TopUsers>> GetTopUsersByActivityAsync(int count = 10);
    Task<List<FailedLoginAttempts>> GetFailedLoginAttemptsAsync(DateTime startDate, DateTime endDate);
    Task<Dictionary<string, int>> GetLoginCountByDayAsync(DateTime startDate, DateTime endDate);
    Task<Dictionary<string, int>> GetLoginCountByHourAsync(DateTime startDate, DateTime endDate);
}

/// <summary>
/// Overall Identity statistics
/// </summary>
public class IdentityStatistics
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int InactiveUsers { get; set; }
    public int LockedUsers { get; set; }
    public int TotalRoles { get; set; }
    public int ActiveRoles { get; set; }
    public int TotalPermissions { get; set; }
    public int ActivePermissions { get; set; }
    public int TotalAuditLogs { get; set; }
    public int TodayLogins { get; set; }
    public int TodayFailedLogins { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// User statistics
/// </summary>
public class UserStatistics
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int InactiveUsers { get; set; }
    public int LockedUsers { get; set; }
    public int UsersWithTwoFactor { get; set; }
    public int UsersWithSmsEnabled { get; set; }
    public int NewUsersThisWeek { get; set; }
    public int NewUsersThisMonth { get; set; }
    public int UsersRequiringPasswordChange { get; set; }
    public double AverageLoginAttempts { get; set; }
}

/// <summary>
/// Role statistics
/// </summary>
public class RoleStatistics
{
    public int TotalRoles { get; set; }
    public int ActiveRoles { get; set; }
    public int SystemRoles { get; set; }
    public int CustomRoles { get; set; }
    public int RolesWithUsers { get; set; }
    public int RolesWithoutUsers { get; set; }
    public string MostUsedRole { get; set; } = string.Empty;
    public int MostUsedRoleCount { get; set; }
    public string LeastUsedRole { get; set; } = string.Empty;
    public int LeastUsedRoleCount { get; set; }
}

/// <summary>
/// Permission statistics
/// </summary>
public class PermissionStatistics
{
    public int TotalPermissions { get; set; }
    public int ActivePermissions { get; set; }
    public int SystemPermissions { get; set; }
    public int CustomPermissions { get; set; }
    public int PermissionsWithRoles { get; set; }
    public int PermissionsWithoutRoles { get; set; }
    public string MostUsedPermission { get; set; } = string.Empty;
    public int MostUsedPermissionCount { get; set; }
    public string LeastUsedPermission { get; set; } = string.Empty;
    public int LeastUsedPermissionCount { get; set; }
}

/// <summary>
/// Audit statistics
/// </summary>
public class AuditStatistics
{
    public int TotalAuditLogs { get; set; }
    public int TodayAuditLogs { get; set; }
    public int ThisWeekAuditLogs { get; set; }
    public int ThisMonthAuditLogs { get; set; }
    public int SuccessfulActions { get; set; }
    public int FailedActions { get; set; }
    public int InfoSeverity { get; set; }
    public int WarningSeverity { get; set; }
    public int ErrorSeverity { get; set; }
    public int CriticalSeverity { get; set; }
    public string MostCommonAction { get; set; } = string.Empty;
    public int MostCommonActionCount { get; set; }
}

/// <summary>
/// Login statistics
/// </summary>
public class LoginStatistics
{
    public int TotalLogins { get; set; }
    public int SuccessfulLogins { get; set; }
    public int FailedLogins { get; set; }
    public double SuccessRate { get; set; }
    public int UniqueUsers { get; set; }
    public string MostActiveUser { get; set; } = string.Empty;
    public int MostActiveUserCount { get; set; }
    public DateTime PeakLoginTime { get; set; }
    public int PeakLoginCount { get; set; }
}

/// <summary>
/// Security statistics
/// </summary>
public class SecurityStatistics
{
    public int UsersWithTwoFactor { get; set; }
    public int UsersWithSmsEnabled { get; set; }
    public int UsersWithTotpEnabled { get; set; }
    public int LockedUsers { get; set; }
    public int SuspendedUsers { get; set; }
    public int UsersRequiringPasswordChange { get; set; }
    public int FailedLoginAttemptsToday { get; set; }
    public int FailedLoginAttemptsThisWeek { get; set; }
    public int FailedLoginAttemptsThisMonth { get; set; }
    public double AverageFailedAttemptsPerUser { get; set; }
}

/// <summary>
/// Recent activity model
/// </summary>
public class RecentActivity
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool IsSuccess { get; set; }
    public AuditSeverity Severity { get; set; }
}

/// <summary>
/// Top users model
/// </summary>
public class TopUsers
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int ActivityCount { get; set; }
    public DateTime LastActivity { get; set; }
}

/// <summary>
/// Failed login attempts model
/// </summary>
public class FailedLoginAttempts
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int AttemptCount { get; set; }
    public DateTime LastAttempt { get; set; }
    public string IpAddress { get; set; } = string.Empty;
}

/// <summary>
/// Implementation of identity statistics service
/// </summary>
public class IdentityStatisticsService : IIdentityStatisticsService
{
    private readonly MyShopIdentityDbContext _context;

    public IdentityStatisticsService(MyShopIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<IdentityStatistics> GetOverallStatisticsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var activeUsers = await _context.Users.CountAsync(u => u.IsActive);
        var inactiveUsers = await _context.Users.CountAsync(u => !u.IsActive);
        var lockedUsers = await _context.Users.CountAsync(u => u.IsLocked);
        var totalRoles = await _context.Roles.CountAsync();
        var activeRoles = await _context.Roles.CountAsync(r => r.IsActive);
        var totalPermissions = await _context.Permissions.CountAsync();
        var activePermissions = await _context.Permissions.CountAsync(p => p.IsActive);
        var totalAuditLogs = await _context.AuditLogs.CountAsync();
        
        var today = DateTime.UtcNow.Date;
        var todayLogins = await _context.AuditLogs.CountAsync(al => al.Action == "Login" && al.Timestamp.Date == today);
        var todayFailedLogins = await _context.AuditLogs.CountAsync(al => al.Action == "LoginFailed" && al.Timestamp.Date == today);

        return new IdentityStatistics
        {
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            InactiveUsers = inactiveUsers,
            LockedUsers = lockedUsers,
            TotalRoles = totalRoles,
            ActiveRoles = activeRoles,
            TotalPermissions = totalPermissions,
            ActivePermissions = activePermissions,
            TotalAuditLogs = totalAuditLogs,
            TodayLogins = todayLogins,
            TodayFailedLogins = todayFailedLogins
        };
    }

    public async Task<UserStatistics> GetUserStatisticsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var activeUsers = await _context.Users.CountAsync(u => u.IsActive);
        var inactiveUsers = await _context.Users.CountAsync(u => !u.IsActive);
        var lockedUsers = await _context.Users.CountAsync(u => u.IsLocked);
        var usersWithTwoFactor = await _context.Users.CountAsync(u => u.TwoFactorEnabled);
        var usersWithSmsEnabled = await _context.Users.CountAsync(u => u.SmsEnabled);
        
        var weekAgo = DateTime.UtcNow.AddDays(-7);
        var monthAgo = DateTime.UtcNow.AddDays(-30);
        var newUsersThisWeek = await _context.Users.CountAsync(u => u.Account.CreatedAt >= weekAgo);
        var newUsersThisMonth = await _context.Users.CountAsync(u => u.Account.CreatedAt >= monthAgo);
        
        var usersRequiringPasswordChange = await _context.Users.CountAsync(u => u.RequiresPasswordChange);
        var averageLoginAttempts = await _context.Users.AverageAsync(u => u.LoginAttempts);

        return new UserStatistics
        {
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            InactiveUsers = inactiveUsers,
            LockedUsers = lockedUsers,
            UsersWithTwoFactor = usersWithTwoFactor,
            UsersWithSmsEnabled = usersWithSmsEnabled,
            NewUsersThisWeek = newUsersThisWeek,
            NewUsersThisMonth = newUsersThisMonth,
            UsersRequiringPasswordChange = usersRequiringPasswordChange,
            AverageLoginAttempts = averageLoginAttempts
        };
    }

    public async Task<RoleStatistics> GetRoleStatisticsAsync()
    {
        var totalRoles = await _context.Roles.CountAsync();
        var activeRoles = await _context.Roles.CountAsync(r => r.IsActive);
        var systemRoles = await _context.Roles.CountAsync(r => r.IsSystemRole);
        var customRoles = await _context.Roles.CountAsync(r => !r.IsSystemRole);
        
        var rolesWithUsers = await _context.Roles
            .CountAsync(r => _context.UserRoles.Any(ur => ur.RoleId == r.Id && ur.IsActive));
        var rolesWithoutUsers = totalRoles - rolesWithUsers;

        var mostUsedRole = await _context.UserRoles
            .Where(ur => ur.IsActive)
            .GroupBy(ur => ur.Role.Name)
            .OrderByDescending(g => g.Count())
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .FirstOrDefaultAsync();

        var leastUsedRole = await _context.UserRoles
            .Where(ur => ur.IsActive)
            .GroupBy(ur => ur.Role.Name)
            .OrderBy(g => g.Count())
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .FirstOrDefaultAsync();

        return new RoleStatistics
        {
            TotalRoles = totalRoles,
            ActiveRoles = activeRoles,
            SystemRoles = systemRoles,
            CustomRoles = customRoles,
            RolesWithUsers = rolesWithUsers,
            RolesWithoutUsers = rolesWithoutUsers,
            MostUsedRole = mostUsedRole?.Name ?? string.Empty,
            MostUsedRoleCount = mostUsedRole?.Count ?? 0,
            LeastUsedRole = leastUsedRole?.Name ?? string.Empty,
            LeastUsedRoleCount = leastUsedRole?.Count ?? 0
        };
    }

    public async Task<PermissionStatistics> GetPermissionStatisticsAsync()
    {
        var totalPermissions = await _context.Permissions.CountAsync();
        var activePermissions = await _context.Permissions.CountAsync(p => p.IsActive);
        var systemPermissions = await _context.Permissions.CountAsync(p => p.IsSystemPermission);
        var customPermissions = await _context.Permissions.CountAsync(p => !p.IsSystemPermission);
        
        var permissionsWithRoles = await _context.Permissions
            .CountAsync(p => _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.IsActive));
        var permissionsWithoutRoles = totalPermissions - permissionsWithRoles;

        var mostUsedPermission = await _context.RolePermissions
            .Where(rp => rp.IsActive)
            .GroupBy(rp => rp.Permission.Name)
            .OrderByDescending(g => g.Count())
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .FirstOrDefaultAsync();

        var leastUsedPermission = await _context.RolePermissions
            .Where(rp => rp.IsActive)
            .GroupBy(rp => rp.Permission.Name)
            .OrderBy(g => g.Count())
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .FirstOrDefaultAsync();

        return new PermissionStatistics
        {
            TotalPermissions = totalPermissions,
            ActivePermissions = activePermissions,
            SystemPermissions = systemPermissions,
            CustomPermissions = customPermissions,
            PermissionsWithRoles = permissionsWithRoles,
            PermissionsWithoutRoles = permissionsWithoutRoles,
            MostUsedPermission = mostUsedPermission?.Name ?? string.Empty,
            MostUsedPermissionCount = mostUsedPermission?.Count ?? 0,
            LeastUsedPermission = leastUsedPermission?.Name ?? string.Empty,
            LeastUsedPermissionCount = leastUsedPermission?.Count ?? 0
        };
    }

    public async Task<AuditStatistics> GetAuditStatisticsAsync()
    {
        var totalAuditLogs = await _context.AuditLogs.CountAsync();
        
        var today = DateTime.UtcNow.Date;
        var weekAgo = DateTime.UtcNow.AddDays(-7);
        var monthAgo = DateTime.UtcNow.AddDays(-30);
        
        var todayAuditLogs = await _context.AuditLogs.CountAsync(al => al.Timestamp.Date == today);
        var thisWeekAuditLogs = await _context.AuditLogs.CountAsync(al => al.Timestamp >= weekAgo);
        var thisMonthAuditLogs = await _context.AuditLogs.CountAsync(al => al.Timestamp >= monthAgo);
        
        var successfulActions = await _context.AuditLogs.CountAsync(al => al.IsSuccess);
        var failedActions = await _context.AuditLogs.CountAsync(al => !al.IsSuccess);
        
        var infoSeverity = await _context.AuditLogs.CountAsync(al => al.Severity == AuditSeverity.Info);
        var warningSeverity = await _context.AuditLogs.CountAsync(al => al.Severity == AuditSeverity.Warning);
        var errorSeverity = await _context.AuditLogs.CountAsync(al => al.Severity == AuditSeverity.Error);
        var criticalSeverity = await _context.AuditLogs.CountAsync(al => al.Severity == AuditSeverity.Critical);

        var mostCommonAction = await _context.AuditLogs
            .GroupBy(al => al.Action)
            .OrderByDescending(g => g.Count())
            .Select(g => new { Action = g.Key, Count = g.Count() })
            .FirstOrDefaultAsync();

        return new AuditStatistics
        {
            TotalAuditLogs = totalAuditLogs,
            TodayAuditLogs = todayAuditLogs,
            ThisWeekAuditLogs = thisWeekAuditLogs,
            ThisMonthAuditLogs = thisMonthAuditLogs,
            SuccessfulActions = successfulActions,
            FailedActions = failedActions,
            InfoSeverity = infoSeverity,
            WarningSeverity = warningSeverity,
            ErrorSeverity = errorSeverity,
            CriticalSeverity = criticalSeverity,
            MostCommonAction = mostCommonAction?.Action ?? string.Empty,
            MostCommonActionCount = mostCommonAction?.Count ?? 0
        };
    }

    public async Task<LoginStatistics> GetLoginStatisticsAsync(DateTime startDate, DateTime endDate)
    {
        var totalLogins = await _context.AuditLogs
            .CountAsync(al => al.Action == "Login" && al.Timestamp >= startDate && al.Timestamp <= endDate);
        
        var successfulLogins = await _context.AuditLogs
            .CountAsync(al => al.Action == "Login" && al.IsSuccess && al.Timestamp >= startDate && al.Timestamp <= endDate);
        
        var failedLogins = await _context.AuditLogs
            .CountAsync(al => al.Action == "LoginFailed" && al.Timestamp >= startDate && al.Timestamp <= endDate);
        
        var successRate = totalLogins > 0 ? (double)successfulLogins / totalLogins * 100 : 0;
        
        var uniqueUsers = await _context.AuditLogs
            .Where(al => al.Action == "Login" && al.Timestamp >= startDate && al.Timestamp <= endDate)
            .Select(al => al.UserId)
            .Distinct()
            .CountAsync();

        var mostActiveUser = await _context.AuditLogs
            .Where(al => al.Action == "Login" && al.IsSuccess && al.Timestamp >= startDate && al.Timestamp <= endDate)
            .GroupBy(al => al.UserId)
            .OrderByDescending(g => g.Count())
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .FirstOrDefaultAsync();

        var peakLoginTime = await _context.AuditLogs
            .Where(al => al.Action == "Login" && al.IsSuccess && al.Timestamp >= startDate && al.Timestamp <= endDate)
            .GroupBy(al => al.Timestamp.Hour)
            .OrderByDescending(g => g.Count())
            .Select(g => new { Hour = g.Key, Count = g.Count() })
            .FirstOrDefaultAsync();

        return new LoginStatistics
        {
            TotalLogins = totalLogins,
            SuccessfulLogins = successfulLogins,
            FailedLogins = failedLogins,
            SuccessRate = successRate,
            UniqueUsers = uniqueUsers,
            MostActiveUser = mostActiveUser?.UserId ?? string.Empty,
            MostActiveUserCount = mostActiveUser?.Count ?? 0,
            PeakLoginTime = peakLoginTime != null ? DateTime.UtcNow.Date.AddHours(peakLoginTime.Hour) : DateTime.UtcNow,
            PeakLoginCount = peakLoginTime?.Count ?? 0
        };
    }

    public async Task<SecurityStatistics> GetSecurityStatisticsAsync()
    {
        var usersWithTwoFactor = await _context.Users.CountAsync(u => u.TwoFactorEnabled);
        var usersWithSmsEnabled = await _context.Users.CountAsync(u => u.SmsEnabled);
        var usersWithTotpEnabled = await _context.Users.CountAsync(u => u.TotpEnabled);
        var lockedUsers = await _context.Users.CountAsync(u => u.IsLocked);
        var suspendedUsers = await _context.Users.CountAsync(u => u.IsLocked);
        var usersRequiringPasswordChange = await _context.Users.CountAsync(u => u.RequiresPasswordChange);
        
        var today = DateTime.UtcNow.Date;
        var weekAgo = DateTime.UtcNow.AddDays(-7);
        var monthAgo = DateTime.UtcNow.AddDays(-30);
        
        var failedLoginAttemptsToday = await _context.AuditLogs.CountAsync(al => al.Action == "LoginFailed" && al.Timestamp.Date == today);
        var failedLoginAttemptsThisWeek = await _context.AuditLogs.CountAsync(al => al.Action == "LoginFailed" && al.Timestamp >= weekAgo);
        var failedLoginAttemptsThisMonth = await _context.AuditLogs.CountAsync(al => al.Action == "LoginFailed" && al.Timestamp >= monthAgo);
        
        var averageFailedAttemptsPerUser = await _context.Users.AverageAsync(u => u.LoginAttempts);

        return new SecurityStatistics
        {
            UsersWithTwoFactor = usersWithTwoFactor,
            UsersWithSmsEnabled = usersWithSmsEnabled,
            UsersWithTotpEnabled = usersWithTotpEnabled,
            LockedUsers = lockedUsers,
            SuspendedUsers = suspendedUsers,
            UsersRequiringPasswordChange = usersRequiringPasswordChange,
            FailedLoginAttemptsToday = failedLoginAttemptsToday,
            FailedLoginAttemptsThisWeek = failedLoginAttemptsThisWeek,
            FailedLoginAttemptsThisMonth = failedLoginAttemptsThisMonth,
            AverageFailedAttemptsPerUser = averageFailedAttemptsPerUser
        };
    }

    public async Task<Dictionary<string, int>> GetUserCountByRoleAsync()
    {
        return await _context.UserRoles
            .Where(ur => ur.IsActive)
            .GroupBy(ur => ur.Role.Name)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetUserCountByStatusAsync()
    {
        var activeCount = await _context.Users.CountAsync(u => u.IsActive);
        var inactiveCount = await _context.Users.CountAsync(u => !u.IsActive);
        var lockedCount = await _context.Users.CountAsync(u => u.IsLocked);

        return new Dictionary<string, int>
        {
            { "Active", activeCount },
            { "Inactive", inactiveCount },
            { "Locked", lockedCount }
        };
    }

    public async Task<Dictionary<string, int>> GetPermissionCountByResourceAsync()
    {
        return await _context.Permissions
            .Where(p => p.IsActive)
            .GroupBy(p => p.Resource.ToString())
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetPermissionCountByCategoryAsync()
    {
        return await _context.Permissions
            .Where(p => p.IsActive)
            .GroupBy(p => p.Category)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetAuditCountBySeverityAsync()
    {
        return await _context.AuditLogs
            .GroupBy(al => al.Severity.ToString())
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetAuditCountByActionAsync()
    {
        return await _context.AuditLogs
            .GroupBy(al => al.Action)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<List<RecentActivity>> GetRecentActivitiesAsync(int count = 50)
    {
        return await _context.AuditLogs
            .OrderByDescending(al => al.Timestamp)
            .Take(count)
            .Select(al => new RecentActivity
            {
                UserId = al.UserId,
                Action = al.Action,
                EntityType = al.EntityType,
                EntityId = al.EntityId,
                Timestamp = al.Timestamp,
                IsSuccess = al.IsSuccess,
                Severity = al.Severity
            })
            .ToListAsync();
    }

    public async Task<List<TopUsers>> GetTopUsersByActivityAsync(int count = 10)
    {
        return await _context.AuditLogs
            .GroupBy(al => al.UserId)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => new TopUsers
            {
                UserId = g.Key,
                ActivityCount = g.Count(),
                LastActivity = g.Max(al => al.Timestamp)
            })
            .ToListAsync();
    }

    public async Task<List<FailedLoginAttempts>> GetFailedLoginAttemptsAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.AuditLogs
            .Where(al => al.Action == "LoginFailed" && al.Timestamp >= startDate && al.Timestamp <= endDate)
            .GroupBy(al => al.UserId)
            .Select(g => new FailedLoginAttempts
            {
                UserId = g.Key,
                AttemptCount = g.Count(),
                LastAttempt = g.Max(al => al.Timestamp),
                IpAddress = g.OrderByDescending(al => al.Timestamp).First().IpAddress
            })
            .OrderByDescending(f => f.AttemptCount)
            .ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetLoginCountByDayAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.AuditLogs
            .Where(al => al.Action == "Login" && al.IsSuccess && al.Timestamp >= startDate && al.Timestamp <= endDate)
            .GroupBy(al => al.Timestamp.Date)
            .ToDictionaryAsync(g => g.Key.ToString("yyyy-MM-dd"), g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetLoginCountByHourAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.AuditLogs
            .Where(al => al.Action == "Login" && al.IsSuccess && al.Timestamp >= startDate && al.Timestamp <= endDate)
            .GroupBy(al => al.Timestamp.Hour)
            .ToDictionaryAsync(g => g.Key.ToString("00:00"), g => g.Count());
    }
}