using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyShop.Contracts.DTOs.Identity.Report;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Contracts.Enums.Identity;
using MyShop.Contracts.Identity.Services;
using MyShop.Contracts.DTOs.Identity.Security;

namespace MyShop.Identity.Services;

/// <summary>
/// Implementation of identity report service
/// </summary>
public class IdentityReportService : IIdentityReportService
{
    private readonly MyShopIdentityDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<IdentityReportService> _logger;

    public IdentityReportService(MyShopIdentityDbContext context, UserManager<ApplicationUser> userManager,
        ILogger<IdentityReportService> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UserReport> GenerateUserReportAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new ArgumentException("User not found");

        return await GenerateUserReportAsync(userId, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);
    }

    public async Task<UserReport> GenerateUserReportAsync(string userId, DateTime startDate, DateTime endDate)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new ArgumentException("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = await GetUserPermissionsAsync(userId);

        var activitySummary = await GenerateUserActivitySummaryAsync(userId, startDate, endDate);
        var securitySummary = await GenerateUserSecuritySummaryAsync(userId);

        return new UserReport
        {
            UserId = userId,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            CreatedAt = user.Account.CreatedAt,
            LastLoginAt = user.LastLoginAt,
            LastPasswordChangeAt = user.LastPasswordChangeAt,
            IsActive = user.IsActive,
            IsLocked = user.IsLocked,
            TwoFactorEnabled = user.TwoFactorEnabled,
            Roles = roles.ToList(),
            Permissions = permissions,
            ActivitySummary = activitySummary,
            SecuritySummary = securitySummary
        };
    }

    public async Task<RoleReport> GenerateRoleReportAsync(string roleId)
    {
        var role = await _context.Roles.FindAsync(roleId);
        if (role == null) throw new ArgumentException("Role not found");

        var users = await _context.UserRoles
            .Where(ur => ur.RoleId == roleId && ur.IsActive)
            .Select(ur => ur.User.UserName)
            .ToListAsync();

        var permissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId && rp.IsActive)
            .Select(rp => rp.Permission.Name)
            .ToListAsync();

        var usageSummary = await GenerateRoleUsageSummaryAsync(roleId);

        return new RoleReport
        {
            RoleId = roleId,
            RoleName = role.Name,
            Description = role.Description,
            IsActive = role.IsActive,
            IsSystemRole = role.IsSystemRole,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt,
            UserCount = users.Count,
            PermissionCount = permissions.Count,
            Users = users,
            Permissions = permissions,
            UsageSummary = usageSummary
        };
    }

    public async Task<PermissionReport> GeneratePermissionReportAsync(string permissionId)
    {
        var permission = await _context.Permissions.FindAsync(permissionId);
        if (permission == null) throw new ArgumentException("Permission not found");

        var roles = await _context.RolePermissions
            .Where(rp => rp.PermissionId == permissionId && rp.IsActive)
            .Select(rp => rp.Role.Name)
            .ToListAsync();

        var users = await _context.RolePermissions
            .Where(rp => rp.PermissionId == permissionId && rp.IsActive)
            .SelectMany(rp => rp.Role.UserRoles.Where(ur => ur.IsActive))
            .Select(ur => ur.User.UserName)
            .Distinct()
            .ToListAsync();

        var usageSummary = await GeneratePermissionUsageSummaryAsync(permissionId);

        return new PermissionReport
        {
            PermissionId = permissionId,
            PermissionName = permission.Name,
            Resource = permission.Resource.ToString(),
            Action = permission.Action.ToString(),
            Description = permission.Description,
            IsActive = permission.IsActive,
            IsSystemPermission = permission.IsSystemPermission,
            CreatedAt = permission.CreatedAt,
            UpdatedAt = permission.UpdatedAt,
            RoleCount = roles.Count,
            UserCount = users.Count,
            Roles = roles,
            Users = users,
            UsageSummary = usageSummary
        };
    }

    public async Task<AuditReport> GenerateAuditReportAsync(DateTime startDate, DateTime endDate)
    {
        var auditLogs = await _context.AuditLogs
            .Where(al => al.Timestamp >= startDate && al.Timestamp <= endDate)
            .ToListAsync();

        var totalAuditLogs = auditLogs.Count;
        var successfulActions = auditLogs.Count(al => al.IsSuccess);
        var failedActions = auditLogs.Count(al => !al.IsSuccess);
        var successRate = totalAuditLogs > 0 ? (double)successfulActions / totalAuditLogs * 100 : 0;

        var auditSummaries = await GenerateAuditSummariesAsync(startDate, endDate);
        var auditTrends = await GenerateAuditTrendsAsync(startDate, endDate);
        var auditAlerts = await GenerateAuditAlertsAsync(startDate, endDate);

        return new AuditReport
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalAuditLogs = totalAuditLogs,
            SuccessfulActions = successfulActions,
            FailedActions = failedActions,
            SuccessRate = successRate,
            AuditSummaries = auditSummaries,
            AuditTrends = auditTrends,
            AuditAlerts = auditAlerts
        };
    }

    public async Task<SecurityReport> GenerateSecurityReportAsync(DateTime startDate, DateTime endDate)
    {
        var totalUsers = await _context.Users.CountAsync();
        var activeUsers = await _context.Users.CountAsync(u => u.IsActive);
        var lockedUsers = await _context.Users.CountAsync(u => u.IsLocked);
        var usersWithTwoFactor = await _context.Users.CountAsync(u => u.TwoFactorEnabled);
        var twoFactorAdoptionRate = totalUsers > 0 ? (double)usersWithTwoFactor / totalUsers * 100 : 0;

        var failedLoginAttempts = await _context.AuditLogs
            .CountAsync(al => al.Action == "LoginFailed" && al.Timestamp >= startDate && al.Timestamp <= endDate);

        var suspiciousActivities = await _context.AuditLogs
            .CountAsync(al => al.Action == "SuspiciousActivity" && al.Timestamp >= startDate && al.Timestamp <= endDate);

        var securityAlerts = await _context.AuditLogs
            .CountAsync(al => al.Severity >= AuditSeverity.Warning && al.Timestamp >= startDate && al.Timestamp <= endDate);

        var securityIncidents = await GenerateSecurityIncidentsAsync(startDate, endDate);
        var securityTrends = await GenerateSecurityTrendsAsync(startDate, endDate);
        var recommendations = await GenerateSecurityRecommendationsAsync();

        return new SecurityReport
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            LockedUsers = lockedUsers,
            UsersWithTwoFactor = usersWithTwoFactor,
            TwoFactorAdoptionRate = twoFactorAdoptionRate,
            FailedLoginAttempts = failedLoginAttempts,
            SuspiciousActivities = suspiciousActivities,
            SecurityAlerts = securityAlerts,
            SecurityIncidents = securityIncidents,
            SecurityTrends = securityTrends,
            Recommendations = recommendations
        };
    }

    public async Task<LoginReport> GenerateLoginReportAsync(DateTime startDate, DateTime endDate)
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

        var loginSummaries = await GenerateLoginSummariesAsync(startDate, endDate);
        var loginTrends = await GenerateLoginTrendsAsync(startDate, endDate);
        var loginAnomalies = await GenerateLoginAnomaliesAsync(startDate, endDate);

        return new LoginReport
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalLogins = totalLogins,
            SuccessfulLogins = successfulLogins,
            FailedLogins = failedLogins,
            SuccessRate = successRate,
            UniqueUsers = uniqueUsers,
            LoginSummaries = loginSummaries,
            LoginTrends = loginTrends,
            LoginAnomalies = loginAnomalies
        };
    }

    public async Task<SystemReport> GenerateSystemReportAsync()
    {
        var summary = await GenerateSystemSummaryAsync();
        var health = await GenerateSystemHealthAsync();
        var performance = await GenerateSystemPerformanceAsync();
        var security = await GenerateSystemSecurityAsync();
        var compliance = await GenerateSystemComplianceAsync();

        return new SystemReport
        {
            Summary = summary,
            Health = health,
            Performance = performance,
            Security = security,
            Compliance = compliance
        };
    }

    public async Task<ComplianceReport> GenerateComplianceReportAsync()
    {
        var summary = await GenerateComplianceSummaryAsync();
        var requirements = await GenerateComplianceRequirementsAsync();
        var violations = await GenerateComplianceViolationsAsync();
        var recommendations = await GenerateComplianceRecommendationsAsync();

        return new ComplianceReport
        {
            Summary = summary,
            Requirements = requirements,
            Violations = violations,
            Recommendations = recommendations
        };
    }

    public async Task<PerformanceReport> GeneratePerformanceReportAsync(DateTime startDate, DateTime endDate)
    {
        var summary = await GeneratePerformanceSummaryAsync(startDate, endDate);
        var trends = await GeneratePerformanceTrendsAsync(startDate, endDate);
        var issues = await GeneratePerformanceIssuesAsync(startDate, endDate);
        var recommendations = await GeneratePerformanceRecommendationsAsync();

        return new PerformanceReport
        {
            StartDate = startDate,
            EndDate = endDate,
            Summary = summary,
            Trends = trends,
            Issues = issues,
            Recommendations = recommendations
        };
    }

    public async Task<ExportReport> ExportReportAsync(string reportType, DateTime startDate, DateTime endDate, string format = "PDF")
    {
        try
        {
            var fileName = $"{reportType}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.{format.ToLower()}";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            // In a real implementation, you would generate the actual report file
            // For now, we'll just create a placeholder

            var report = new ExportReport
            {
                ReportType = reportType,
                StartDate = startDate,
                EndDate = endDate,
                Format = format,
                FileName = fileName,
                FilePath = filePath,
                FileSize = 0, // Placeholder
                Status = "Success"
            };

            return report;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export report: {ReportType}", reportType);

            return new ExportReport
            {
                ReportType = reportType,
                StartDate = startDate,
                EndDate = endDate,
                Format = format,
                Status = "Failed",
                ErrorMessage = ex.Message
            };
        }
    }

    // Helper methods for generating report components
    private async Task<List<string>> GetUserPermissionsAsync(string userId)
    {
        var userRoles = await _context.UserRoles
            .Where(ur => ur.UserId == userId && ur.IsActive)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        var permissions = await _context.RolePermissions
            .Where(rp => userRoles.Contains(rp.RoleId) && rp.IsActive)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToListAsync();

        return permissions;
    }

    private async Task<UserActivitySummary> GenerateUserActivitySummaryAsync(string userId, DateTime startDate, DateTime endDate)
    {
        var auditLogs = await _context.AuditLogs
            .Where(al => al.UserId == userId && al.Timestamp >= startDate && al.Timestamp <= endDate)
            .ToListAsync();

        var totalLogins = auditLogs.Count(al => al.Action == "Login");
        var failedLogins = auditLogs.Count(al => al.Action == "LoginFailed");
        var successfulLogins = totalLogins - failedLogins;
        var loginSuccessRate = totalLogins > 0 ? (double)successfulLogins / totalLogins * 100 : 0;

        var passwordChanges = auditLogs.Count(al => al.Action == "PasswordChanged");
        var roleChanges = auditLogs.Count(al => al.Action == "RoleAssigned" || al.Action == "RoleRemoved");
        var permissionChanges = auditLogs.Count(al => al.Action == "PermissionAssigned" || al.Action == "PermissionRemoved");

        var firstLogin = auditLogs.Where(al => al.Action == "Login" && al.IsSuccess).MinBy(al => al.Timestamp)?.Timestamp ?? DateTime.MinValue;
        var lastLogin = auditLogs.Where(al => al.Action == "Login" && al.IsSuccess).MaxBy(al => al.Timestamp)?.Timestamp ?? DateTime.MinValue;

        var uniqueIpAddresses = auditLogs.Select(al => al.IpAddress).Distinct().Count();
        var uniqueUserAgents = auditLogs.Select(al => al.UserAgent).Distinct().Count();

        return new UserActivitySummary
        {
            TotalLogins = totalLogins,
            FailedLogins = failedLogins,
            SuccessfulLogins = successfulLogins,
            LoginSuccessRate = loginSuccessRate,
            PasswordChanges = passwordChanges,
            RoleChanges = roleChanges,
            PermissionChanges = permissionChanges,
            FirstLogin = firstLogin,
            LastLogin = lastLogin,
            TotalSessionTime = TimeSpan.Zero, // Placeholder
            UniqueIpAddresses = uniqueIpAddresses,
            UniqueUserAgents = uniqueUserAgents
        };
    }

    private async Task<UserSecuritySummary> GenerateUserSecuritySummaryAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new UserSecuritySummary();

        var failedLoginAttempts = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "LoginFailed");

        var securityAlerts = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Severity >= AuditSeverity.Warning);

        var suspiciousActivities = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "SuspiciousActivity");

        var daysSinceLastPasswordChange = user.LastPasswordChangeAt.HasValue
            ? (DateTime.UtcNow - user.LastPasswordChangeAt.Value).Days
            : 0;

        var securityLevel = CalculateSecurityLevel(user, failedLoginAttempts, securityAlerts);

        return new UserSecuritySummary
        {
            TwoFactorEnabled = user.TwoFactorEnabled,
            SmsEnabled = user.SmsEnabled,
            TotpEnabled = user.TotpEnabled,
            FailedLoginAttempts = failedLoginAttempts,
            SecurityAlerts = securityAlerts,
            SuspiciousActivities = suspiciousActivities,
            LastPasswordChange = user.LastPasswordChangeAt ?? DateTime.MinValue,
            RequiresPasswordChange = user.RequiresPasswordChange,
            DaysSinceLastPasswordChange = daysSinceLastPasswordChange,
            SecurityLevel = securityLevel
        };
    }

    private string CalculateSecurityLevel(ApplicationUser user, int failedLoginAttempts, int securityAlerts)
    {
        var score = 100;

        if (!user.TwoFactorEnabled) score -= 30;
        if (user.RequiresPasswordChange) score -= 20;
        if (failedLoginAttempts > 5) score -= 20;
        if (securityAlerts > 3) score -= 15;
        if (user.IsLocked) score -= 25;

        return score switch
        {
            >= 80 => "High",
            >= 60 => "Medium",
            >= 40 => "Low",
            _ => "Critical"
        };
    }

    // Placeholder implementations for other helper methods
    private async Task<RoleUsageSummary> GenerateRoleUsageSummaryAsync(string roleId)
    {
        return new RoleUsageSummary();
    }

    private async Task<PermissionUsageSummary> GeneratePermissionUsageSummaryAsync(string permissionId)
    {
        return new PermissionUsageSummary();
    }

    private async Task<List<AuditSummary>> GenerateAuditSummariesAsync(DateTime startDate, DateTime endDate)
    {
        return new List<AuditSummary>();
    }

    private async Task<List<AuditTrend>> GenerateAuditTrendsAsync(DateTime startDate, DateTime endDate)
    {
        return new List<AuditTrend>();
    }

    private async Task<List<AuditAlert>> GenerateAuditAlertsAsync(DateTime startDate, DateTime endDate)
    {
        return new List<AuditAlert>();
    }

    private async Task<List<SecurityIncident>> GenerateSecurityIncidentsAsync(DateTime startDate, DateTime endDate)
    {
        return new List<SecurityIncident>();
    }

    private async Task<List<SecurityTrend>> GenerateSecurityTrendsAsync(DateTime startDate, DateTime endDate)
    {
        return new List<SecurityTrend>();
    }

    private async Task<SecurityRecommendations> GenerateSecurityRecommendationsAsync()
    {
        return new SecurityRecommendations();
    }

    private async Task<List<LoginSummary>> GenerateLoginSummariesAsync(DateTime startDate, DateTime endDate)
    {
        return new List<LoginSummary>();
    }

    private async Task<List<LoginTrend>> GenerateLoginTrendsAsync(DateTime startDate, DateTime endDate)
    {
        return new List<LoginTrend>();
    }

    private async Task<List<LoginAnomaly>> GenerateLoginAnomaliesAsync(DateTime startDate, DateTime endDate)
    {
        return new List<LoginAnomaly>();
    }

    private async Task<SystemSummary> GenerateSystemSummaryAsync()
    {
        return new SystemSummary();
    }

    private async Task<SystemHealth> GenerateSystemHealthAsync()
    {
        return new SystemHealth();
    }

    private async Task<SystemPerformance> GenerateSystemPerformanceAsync()
    {
        return new SystemPerformance();
    }

    private async Task<SystemSecurity> GenerateSystemSecurityAsync()
    {
        return new SystemSecurity();
    }

    private async Task<SystemCompliance> GenerateSystemComplianceAsync()
    {
        return new SystemCompliance();
    }

    private async Task<ComplianceSummary> GenerateComplianceSummaryAsync()
    {
        return new ComplianceSummary();
    }

    private async Task<List<ComplianceRequirement>> GenerateComplianceRequirementsAsync()
    {
        return new List<ComplianceRequirement>();
    }

    private async Task<List<ComplianceViolation>> GenerateComplianceViolationsAsync()
    {
        return new List<ComplianceViolation>();
    }

    private async Task<List<ComplianceRecommendation>> GenerateComplianceRecommendationsAsync()
    {
        return new List<ComplianceRecommendation>();
    }

    private async Task<PerformanceSummary> GeneratePerformanceSummaryAsync(DateTime startDate, DateTime endDate)
    {
        return new PerformanceSummary();
    }

    private async Task<List<PerformanceTrend>> GeneratePerformanceTrendsAsync(DateTime startDate, DateTime endDate)
    {
        return new List<PerformanceTrend>();
    }

    private async Task<List<PerformanceIssue>> GeneratePerformanceIssuesAsync(DateTime startDate, DateTime endDate)
    {
        return new List<PerformanceIssue>();
    }

    private async Task<List<PerformanceRecommendation>> GeneratePerformanceRecommendationsAsync()
    {
        return new List<PerformanceRecommendation>();
    }
}