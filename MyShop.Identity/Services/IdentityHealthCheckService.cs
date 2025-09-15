using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyShop.Identity.Context;
using MyShop.Identity.Enums;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services;

/// <summary>
/// Service for managing Identity health checks
/// </summary>
public interface IIdentityHealthCheckService
{
    Task<IdentityHealthStatus> CheckOverallHealthAsync();
    Task<DatabaseHealthStatus> CheckDatabaseHealthAsync();
    Task<AuthenticationHealthStatus> CheckAuthenticationHealthAsync();
    Task<AuthorizationHealthStatus> CheckAuthorizationHealthAsync();
    Task<SecurityHealthStatus> CheckSecurityHealthAsync();
    Task<PerformanceHealthStatus> CheckPerformanceHealthAsync();
    Task<List<HealthIssue>> GetHealthIssuesAsync();
    Task<bool> IsHealthyAsync();
    Task<HealthMetrics> GetHealthMetricsAsync();
}

/// <summary>
/// Overall Identity health status
/// </summary>
public class IdentityHealthStatus
{
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    public List<HealthIssue> Issues { get; set; } = new();
    public HealthMetrics Metrics { get; set; } = new();
}

/// <summary>
/// Database health status
/// </summary>
public class DatabaseHealthStatus
{
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public TimeSpan ResponseTime { get; set; }
    public int ConnectionCount { get; set; }
    public string DatabaseVersion { get; set; } = string.Empty;
    public List<HealthIssue> Issues { get; set; } = new();
}

/// <summary>
/// Authentication health status
/// </summary>
public class AuthenticationHealthStatus
{
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int LockedUsers { get; set; }
    public int UsersWithTwoFactor { get; set; }
    public double TwoFactorAdoptionRate { get; set; }
    public List<HealthIssue> Issues { get; set; } = new();
}

/// <summary>
/// Authorization health status
/// </summary>
public class AuthorizationHealthStatus
{
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalRoles { get; set; }
    public int ActiveRoles { get; set; }
    public int TotalPermissions { get; set; }
    public int ActivePermissions { get; set; }
    public int RolesWithUsers { get; set; }
    public int PermissionsWithRoles { get; set; }
    public List<HealthIssue> Issues { get; set; } = new();
}

/// <summary>
/// Security health status
/// </summary>
public class SecurityHealthStatus
{
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public int FailedLoginAttempts24h { get; set; }
    public int SuspiciousActivities24h { get; set; }
    public int SecurityAlerts24h { get; set; }
    public int UsersRequiringPasswordChange { get; set; }
    public double SecurityScore { get; set; }
    public List<HealthIssue> Issues { get; set; } = new();
}

/// <summary>
/// Performance health status
/// </summary>
public class PerformanceHealthStatus
{
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public TimeSpan AverageResponseTime { get; set; }
    public int SlowQueries { get; set; }
    public int HighMemoryUsage { get; set; }
    public int HighCpuUsage { get; set; }
    public List<HealthIssue> Issues { get; set; } = new();
}

/// <summary>
/// Health issue model
/// </summary>
public class HealthIssue
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Health metrics model
/// </summary>
public class HealthMetrics
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalRoles { get; set; }
    public int TotalPermissions { get; set; }
    public int TotalAuditLogs { get; set; }
    public int TodayLogins { get; set; }
    public int TodayFailedLogins { get; set; }
    public double LoginSuccessRate { get; set; }
    public int UsersWithTwoFactor { get; set; }
    public double TwoFactorAdoptionRate { get; set; }
    public int SecurityAlerts { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Implementation of identity health check service
/// </summary>
public class IdentityHealthCheckService : IIdentityHealthCheckService
{
    private readonly MyShopIdentityDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<IdentityHealthCheckService> _logger;

    public IdentityHealthCheckService(MyShopIdentityDbContext context, UserManager<ApplicationUser> userManager,
        ILogger<IdentityHealthCheckService> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IdentityHealthStatus> CheckOverallHealthAsync()
    {
        var issues = new List<HealthIssue>();
        var metrics = await GetHealthMetricsAsync();

        // Check database health
        var dbHealth = await CheckDatabaseHealthAsync();
        if (!dbHealth.IsHealthy)
        {
            issues.AddRange(dbHealth.Issues);
        }

        // Check authentication health
        var authHealth = await CheckAuthenticationHealthAsync();
        if (!authHealth.IsHealthy)
        {
            issues.AddRange(authHealth.Issues);
        }

        // Check authorization health
        var authzHealth = await CheckAuthorizationHealthAsync();
        if (!authzHealth.IsHealthy)
        {
            issues.AddRange(authzHealth.Issues);
        }

        // Check security health
        var securityHealth = await CheckSecurityHealthAsync();
        if (!securityHealth.IsHealthy)
        {
            issues.AddRange(securityHealth.Issues);
        }

        // Check performance health
        var perfHealth = await CheckPerformanceHealthAsync();
        if (!perfHealth.IsHealthy)
        {
            issues.AddRange(perfHealth.Issues);
        }

        var isHealthy = issues.Count == 0 || issues.All(i => i.Severity != "Critical");
        var status = isHealthy ? "Healthy" : "Unhealthy";

        return new IdentityHealthStatus
        {
            IsHealthy = isHealthy,
            Status = status,
            Issues = issues,
            Metrics = metrics
        };
    }

    public async Task<DatabaseHealthStatus> CheckDatabaseHealthAsync()
    {
        var issues = new List<HealthIssue>();
        var startTime = DateTime.UtcNow;

        try
        {
            // Test database connection
            await _context.Database.OpenConnectionAsync();
            await _context.Database.CloseConnectionAsync();

            var responseTime = DateTime.UtcNow - startTime;

            // Check response time
            if (responseTime.TotalSeconds > 5)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Performance",
                    Title = "Slow Database Response",
                    Description = $"Database response time is {responseTime.TotalSeconds:F2} seconds",
                    Severity = "Warning",
                    Recommendation = "Consider optimizing database queries or increasing resources"
                });
            }

            // Check connection count
            var connectionCount = await _context.Database.GetDbConnection().State == System.Data.ConnectionState.Open ? 1 : 0;

            if (connectionCount > 100)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Resource",
                    Title = "High Connection Count",
                    Description = $"Database has {connectionCount} active connections",
                    Severity = "Warning",
                    Recommendation = "Consider connection pooling optimization"
                });
            }

            var isHealthy = issues.Count == 0 || issues.All(i => i.Severity != "Critical");

            return new DatabaseHealthStatus
            {
                IsHealthy = isHealthy,
                Status = isHealthy ? "Healthy" : "Unhealthy",
                ResponseTime = responseTime,
                ConnectionCount = connectionCount,
                DatabaseVersion = "SQL Server", // Placeholder
                Issues = issues
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");

            issues.Add(new HealthIssue
            {
                Type = "Database",
                Title = "Database Connection Failed",
                Description = ex.Message,
                Severity = "Critical",
                Recommendation = "Check database connectivity and configuration"
            });

            return new DatabaseHealthStatus
            {
                IsHealthy = false,
                Status = "Unhealthy",
                ResponseTime = DateTime.UtcNow - startTime,
                ConnectionCount = 0,
                DatabaseVersion = "Unknown",
                Issues = issues
            };
        }
    }

    public async Task<AuthenticationHealthStatus> CheckAuthenticationHealthAsync()
    {
        var issues = new List<HealthIssue>();

        try
        {
            var totalUsers = await _context.Users.CountAsync();
            var activeUsers = await _context.Users.CountAsync(u => u.IsActive);
            var lockedUsers = await _context.Users.CountAsync(u => u.IsLocked);
            var usersWithTwoFactor = await _context.Users.CountAsync(u => u.TwoFactorEnabled);

            var twoFactorAdoptionRate = totalUsers > 0 ? (double)usersWithTwoFactor / totalUsers * 100 : 0;

            // Check two-factor adoption rate
            if (twoFactorAdoptionRate < 50)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Security",
                    Title = "Low Two-Factor Adoption",
                    Description = $"Only {twoFactorAdoptionRate:F1}% of users have two-factor authentication enabled",
                    Severity = "Warning",
                    Recommendation = "Promote two-factor authentication to improve security"
                });
            }

            // Check locked users percentage
            var lockedUsersPercentage = totalUsers > 0 ? (double)lockedUsers / totalUsers * 100 : 0;
            if (lockedUsersPercentage > 10)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Security",
                    Title = "High Locked Users Percentage",
                    Description = $"{lockedUsersPercentage:F1}% of users are locked",
                    Severity = "Warning",
                    Recommendation = "Review account lockout policies and user support"
                });
            }

            // Check active users percentage
            var activeUsersPercentage = totalUsers > 0 ? (double)activeUsers / totalUsers * 100 : 0;
            if (activeUsersPercentage < 80)
            {
                issues.Add(new HealthIssue
                {
                    Type = "User Management",
                    Title = "Low Active Users Percentage",
                    Description = $"Only {activeUsersPercentage:F1}% of users are active",
                    Severity = "Info",
                    Recommendation = "Consider user engagement strategies"
                });
            }

            var isHealthy = issues.Count == 0 || issues.All(i => i.Severity != "Critical");

            return new AuthenticationHealthStatus
            {
                IsHealthy = isHealthy,
                Status = isHealthy ? "Healthy" : "Unhealthy",
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                LockedUsers = lockedUsers,
                UsersWithTwoFactor = usersWithTwoFactor,
                TwoFactorAdoptionRate = twoFactorAdoptionRate,
                Issues = issues
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Authentication health check failed");

            issues.Add(new HealthIssue
            {
                Type = "Authentication",
                Title = "Authentication Health Check Failed",
                Description = ex.Message,
                Severity = "Critical",
                Recommendation = "Check authentication system configuration"
            });

            return new AuthenticationHealthStatus
            {
                IsHealthy = false,
                Status = "Unhealthy",
                Issues = issues
            };
        }
    }

    public async Task<AuthorizationHealthStatus> CheckAuthorizationHealthAsync()
    {
        var issues = new List<HealthIssue>();

        try
        {
            var totalRoles = await _context.Roles.CountAsync();
            var activeRoles = await _context.Roles.CountAsync(r => r.IsActive);
            var totalPermissions = await _context.Permissions.CountAsync();
            var activePermissions = await _context.Permissions.CountAsync(p => p.IsActive);

            var rolesWithUsers = await _context.Roles
                .CountAsync(r => _context.UserRoles.Any(ur => ur.RoleId == r.Id && ur.IsActive));

            var permissionsWithRoles = await _context.Permissions
                .CountAsync(p => _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.IsActive));

            // Check for roles without users
            var rolesWithoutUsers = totalRoles - rolesWithUsers;
            if (rolesWithoutUsers > 0)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Authorization",
                    Title = "Unused Roles",
                    Description = $"{rolesWithoutUsers} roles have no assigned users",
                    Severity = "Info",
                    Recommendation = "Consider removing unused roles or assigning users"
                });
            }

            // Check for permissions without roles
            var permissionsWithoutRoles = totalPermissions - permissionsWithRoles;
            if (permissionsWithoutRoles > 0)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Authorization",
                    Title = "Unused Permissions",
                    Description = $"{permissionsWithoutRoles} permissions are not assigned to any role",
                    Severity = "Info",
                    Recommendation = "Consider removing unused permissions or assigning to roles"
                });
            }

            // Check for inactive roles
            var inactiveRoles = totalRoles - activeRoles;
            if (inactiveRoles > 0)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Authorization",
                    Title = "Inactive Roles",
                    Description = $"{inactiveRoles} roles are inactive",
                    Severity = "Warning",
                    Recommendation = "Review inactive roles and consider reactivation or removal"
                });
            }

            var isHealthy = issues.Count == 0 || issues.All(i => i.Severity != "Critical");

            return new AuthorizationHealthStatus
            {
                IsHealthy = isHealthy,
                Status = isHealthy ? "Healthy" : "Unhealthy",
                TotalRoles = totalRoles,
                ActiveRoles = activeRoles,
                TotalPermissions = totalPermissions,
                ActivePermissions = activePermissions,
                RolesWithUsers = rolesWithUsers,
                PermissionsWithRoles = permissionsWithRoles,
                Issues = issues
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Authorization health check failed");

            issues.Add(new HealthIssue
            {
                Type = "Authorization",
                Title = "Authorization Health Check Failed",
                Description = ex.Message,
                Severity = "Critical",
                Recommendation = "Check authorization system configuration"
            });

            return new AuthorizationHealthStatus
            {
                IsHealthy = false,
                Status = "Unhealthy",
                Issues = issues
            };
        }
    }

    public async Task<SecurityHealthStatus> CheckSecurityHealthAsync()
    {
        var issues = new List<HealthIssue>();

        try
        {
            var yesterday = DateTime.UtcNow.AddDays(-1);
            var failedLoginAttempts24h = await _context.AuditLogs
                .CountAsync(al => al.Action == "LoginFailed" && al.Timestamp >= yesterday);

            var suspiciousActivities24h = await _context.AuditLogs
                .CountAsync(al => al.Action == "SuspiciousActivity" && al.Timestamp >= yesterday);

            var securityAlerts24h = await _context.AuditLogs
                .CountAsync(al => al.Severity >= AuditSeverity.Warning && al.Timestamp >= yesterday);

            var usersRequiringPasswordChange = await _context.Users
                .CountAsync(u => u.RequiresPasswordChange);

            // Check failed login attempts
            if (failedLoginAttempts24h > 100)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Security",
                    Title = "High Failed Login Attempts",
                    Description = $"{failedLoginAttempts24h} failed login attempts in the last 24 hours",
                    Severity = "Warning",
                    Recommendation = "Review login security and consider additional measures"
                });
            }

            // Check suspicious activities
            if (suspiciousActivities24h > 10)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Security",
                    Title = "High Suspicious Activities",
                    Description = $"{suspiciousActivities24h} suspicious activities detected in the last 24 hours",
                    Severity = "Warning",
                    Recommendation = "Investigate suspicious activities and strengthen security measures"
                });
            }

            // Check security alerts
            if (securityAlerts24h > 20)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Security",
                    Title = "High Security Alerts",
                    Description = $"{securityAlerts24h} security alerts in the last 24 hours",
                    Severity = "Warning",
                    Recommendation = "Review security alerts and address underlying issues"
                });
            }

            // Check users requiring password change
            if (usersRequiringPasswordChange > 0)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Security",
                    Title = "Users Requiring Password Change",
                    Description = $"{usersRequiringPasswordChange} users require password change",
                    Severity = "Info",
                    Recommendation = "Notify users to change their passwords"
                });
            }

            var securityScore = CalculateSecurityScore(failedLoginAttempts24h, suspiciousActivities24h, securityAlerts24h);
            var isHealthy = issues.Count == 0 || issues.All(i => i.Severity != "Critical");

            return new SecurityHealthStatus
            {
                IsHealthy = isHealthy,
                Status = isHealthy ? "Healthy" : "Unhealthy",
                FailedLoginAttempts24h = failedLoginAttempts24h,
                SuspiciousActivities24h = suspiciousActivities24h,
                SecurityAlerts24h = securityAlerts24h,
                UsersRequiringPasswordChange = usersRequiringPasswordChange,
                SecurityScore = securityScore,
                Issues = issues
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Security health check failed");

            issues.Add(new HealthIssue
            {
                Type = "Security",
                Title = "Security Health Check Failed",
                Description = ex.Message,
                Severity = "Critical",
                Recommendation = "Check security system configuration"
            });

            return new SecurityHealthStatus
            {
                IsHealthy = false,
                Status = "Unhealthy",
                Issues = issues
            };
        }
    }

    public async Task<PerformanceHealthStatus> CheckPerformanceHealthAsync()
    {
        var issues = new List<HealthIssue>();

        try
        {
            // Check average response time
            var startTime = DateTime.UtcNow;
            await _context.Users.CountAsync();
            var averageResponseTime = DateTime.UtcNow - startTime;

            if (averageResponseTime.TotalSeconds > 2)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Performance",
                    Title = "Slow Query Response",
                    Description = $"Average query response time is {averageResponseTime.TotalSeconds:F2} seconds",
                    Severity = "Warning",
                    Recommendation = "Optimize database queries and consider indexing"
                });
            }

            // Check for slow queries (placeholder)
            var slowQueries = 0; // In real implementation, you would check query performance

            if (slowQueries > 5)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Performance",
                    Title = "Slow Queries Detected",
                    Description = $"{slowQueries} slow queries detected",
                    Severity = "Warning",
                    Recommendation = "Review and optimize slow queries"
                });
            }

            // Check memory usage (placeholder)
            var highMemoryUsage = 0; // In real implementation, you would check memory usage

            if (highMemoryUsage > 0)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Performance",
                    Title = "High Memory Usage",
                    Description = "High memory usage detected",
                    Severity = "Warning",
                    Recommendation = "Monitor memory usage and consider optimization"
                });
            }

            // Check CPU usage (placeholder)
            var highCpuUsage = 0; // In real implementation, you would check CPU usage

            if (highCpuUsage > 0)
            {
                issues.Add(new HealthIssue
                {
                    Type = "Performance",
                    Title = "High CPU Usage",
                    Description = "High CPU usage detected",
                    Severity = "Warning",
                    Recommendation = "Monitor CPU usage and consider optimization"
                });
            }

            var isHealthy = issues.Count == 0 || issues.All(i => i.Severity != "Critical");

            return new PerformanceHealthStatus
            {
                IsHealthy = isHealthy,
                Status = isHealthy ? "Healthy" : "Unhealthy",
                AverageResponseTime = averageResponseTime,
                SlowQueries = slowQueries,
                HighMemoryUsage = highMemoryUsage,
                HighCpuUsage = highCpuUsage,
                Issues = issues
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Performance health check failed");

            issues.Add(new HealthIssue
            {
                Type = "Performance",
                Title = "Performance Health Check Failed",
                Description = ex.Message,
                Severity = "Critical",
                Recommendation = "Check performance monitoring configuration"
            });

            return new PerformanceHealthStatus
            {
                IsHealthy = false,
                Status = "Unhealthy",
                Issues = issues
            };
        }
    }

    public async Task<List<HealthIssue>> GetHealthIssuesAsync()
    {
        var overallHealth = await CheckOverallHealthAsync();
        return overallHealth.Issues;
    }

    public async Task<bool> IsHealthyAsync()
    {
        var overallHealth = await CheckOverallHealthAsync();
        return overallHealth.IsHealthy;
    }

    public async Task<HealthMetrics> GetHealthMetricsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var activeUsers = await _context.Users.CountAsync(u => u.IsActive);
        var totalRoles = await _context.Roles.CountAsync();
        var totalPermissions = await _context.Permissions.CountAsync();
        var totalAuditLogs = await _context.AuditLogs.CountAsync();

        var today = DateTime.UtcNow.Date;
        var todayLogins = await _context.AuditLogs.CountAsync(al => al.Action == "Login" && al.Timestamp.Date == today);
        var todayFailedLogins = await _context.AuditLogs.CountAsync(al => al.Action == "LoginFailed" && al.Timestamp.Date == today);

        var loginSuccessRate = (todayLogins + todayFailedLogins) > 0 ? (double)todayLogins / (todayLogins + todayFailedLogins) * 100 : 0;

        var usersWithTwoFactor = await _context.Users.CountAsync(u => u.TwoFactorEnabled);
        var twoFactorAdoptionRate = totalUsers > 0 ? (double)usersWithTwoFactor / totalUsers * 100 : 0;

        var securityAlerts = await _context.AuditLogs.CountAsync(al => al.Severity >= AuditSeverity.Warning);

        return new HealthMetrics
        {
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            TotalRoles = totalRoles,
            TotalPermissions = totalPermissions,
            TotalAuditLogs = totalAuditLogs,
            TodayLogins = todayLogins,
            TodayFailedLogins = todayFailedLogins,
            LoginSuccessRate = loginSuccessRate,
            UsersWithTwoFactor = usersWithTwoFactor,
            TwoFactorAdoptionRate = twoFactorAdoptionRate,
            SecurityAlerts = securityAlerts
        };
    }

    private double CalculateSecurityScore(int failedLoginAttempts, int suspiciousActivities, int securityAlerts)
    {
        // Simple security score calculation
        var score = 100.0;

        if (failedLoginAttempts > 50) score -= 20;
        else if (failedLoginAttempts > 20) score -= 10;

        if (suspiciousActivities > 10) score -= 30;
        else if (suspiciousActivities > 5) score -= 15;

        if (securityAlerts > 20) score -= 25;
        else if (securityAlerts > 10) score -= 10;

        return Math.Max(0, score);
    }
}