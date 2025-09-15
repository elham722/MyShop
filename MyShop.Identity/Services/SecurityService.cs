using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using System.Security.Cryptography;
using System.Text;

namespace MyShop.Identity.Services;

/// <summary>
/// Service for managing security-related operations
/// </summary>
public interface ISecurityService
{
    Task<bool> ValidatePasswordStrengthAsync(string password);
    Task<string> GenerateSecurePasswordAsync(int length = 12);
    Task<string> GenerateTotpSecretAsync();
    Task<bool> ValidateTotpTokenAsync(string secret, string token);
    Task<string> GenerateQrCodeAsync(string secret, string email);
    Task<bool> CheckPasswordHistoryAsync(string userId, string newPassword, int historyCount = 5);
    Task<bool> IsPasswordExpiredAsync(string userId);
    Task<bool> IsAccountCompromisedAsync(string userId);
    Task<bool> CheckSuspiciousActivityAsync(string userId, string ipAddress, string userAgent);
    Task<bool> LockAccountForSecurityAsync(string userId, string reason);
    Task<bool> UnlockAccountForSecurityAsync(string userId);
    Task<bool> RequirePasswordChangeAsync(string userId, string reason);
    Task<bool> CheckBruteForceAttemptsAsync(string userId, string ipAddress);
    Task<bool> CheckGeolocationAnomalyAsync(string userId, string ipAddress);
    Task<bool> CheckDeviceAnomalyAsync(string userId, string userAgent);
    Task<bool> CheckTimeAnomalyAsync(string userId);
    Task<bool> CheckBehaviorAnomalyAsync(string userId);
    Task<SecurityReport> GenerateSecurityReportAsync(string userId);
    Task<SecurityReport> GenerateOverallSecurityReportAsync();
    Task<bool> EnableSecurityMonitoringAsync(string userId);
    Task<bool> DisableSecurityMonitoringAsync(string userId);
    Task<bool> IsSecurityMonitoringEnabledAsync(string userId);
    Task<List<SecurityAlert>> GetSecurityAlertsAsync(string userId, int count = 50);
    Task<bool> AcknowledgeSecurityAlertAsync(string alertId, string userId);
    Task<bool> ResolveSecurityAlertAsync(string alertId, string userId, string resolution);
}

/// <summary>
/// Security report model
/// </summary>
public class SecurityReport
{
    public string UserId { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public SecurityScore SecurityScore { get; set; } = new();
    public List<SecurityRecommendation> Recommendations { get; set; } = new();
    public List<SecurityAlert> RecentAlerts { get; set; } = new();
    public SecurityMetrics Metrics { get; set; } = new();
}

/// <summary>
/// Security score model
/// </summary>
public class SecurityScore
{
    public int OverallScore { get; set; }
    public int PasswordScore { get; set; }
    public int TwoFactorScore { get; set; }
    public int LoginScore { get; set; }
    public int DeviceScore { get; set; }
    public int BehaviorScore { get; set; }
    public string Level { get; set; } = string.Empty;
}

/// <summary>
/// Security recommendation model
/// </summary>
public class SecurityRecommendation
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
}

/// <summary>
/// Security alert model
/// </summary>
public class SecurityAlert
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? Resolution { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Security metrics model
/// </summary>
public class SecurityMetrics
{
    public int FailedLoginAttempts { get; set; }
    public int SuccessfulLogins { get; set; }
    public int PasswordChanges { get; set; }
    public int TwoFactorEnables { get; set; }
    public int TwoFactorDisables { get; set; }
    public int AccountLocks { get; set; }
    public int AccountUnlocks { get; set; }
    public int SuspiciousActivities { get; set; }
    public int SecurityAlerts { get; set; }
    public DateTime LastPasswordChange { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime LastSecurityAlert { get; set; }
}

/// <summary>
/// Implementation of security service
/// </summary>
public class SecurityService : ISecurityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MyShopIdentityDbContext _context;
    private readonly IAuditService _auditService;
    private readonly ILogger<SecurityService> _logger;

    public SecurityService(UserManager<ApplicationUser> userManager, MyShopIdentityDbContext context,
        IAuditService auditService, ILogger<SecurityService> logger)
    {
        _userManager = userManager;
        _context = context;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<bool> ValidatePasswordStrengthAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        // Check minimum length
        if (password.Length < 8)
            return false;

        // Check for uppercase letter
        if (!password.Any(char.IsUpper))
            return false;

        // Check for lowercase letter
        if (!password.Any(char.IsLower))
            return false;

        // Check for digit
        if (!password.Any(char.IsDigit))
            return false;

        // Check for special character
        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            return false;

        // Check for common passwords
        var commonPasswords = new[] { "password", "123456", "qwerty", "abc123", "password123" };
        if (commonPasswords.Any(cp => password.ToLower().Contains(cp)))
            return false;

        return true;
    }

    public async Task<string> GenerateSecurePasswordAsync(int length = 12)
    {
        const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string special = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        var allChars = uppercase + lowercase + digits + special;
        var password = new StringBuilder();

        // Ensure at least one character from each category
        password.Append(uppercase[RandomNumberGenerator.GetInt32(uppercase.Length)]);
        password.Append(lowercase[RandomNumberGenerator.GetInt32(lowercase.Length)]);
        password.Append(digits[RandomNumberGenerator.GetInt32(digits.Length)]);
        password.Append(special[RandomNumberGenerator.GetInt32(special.Length)]);

        // Fill the rest randomly
        for (int i = 4; i < length; i++)
        {
            password.Append(allChars[RandomNumberGenerator.GetInt32(allChars.Length)]);
        }

        // Shuffle the password
        var passwordArray = password.ToString().ToCharArray();
        for (int i = passwordArray.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (passwordArray[i], passwordArray[j]) = (passwordArray[j], passwordArray[i]);
        }

        return new string(passwordArray);
    }

    public async Task<string> GenerateTotpSecretAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        var secret = new StringBuilder();
        
        for (int i = 0; i < 32; i++)
        {
            secret.Append(chars[RandomNumberGenerator.GetInt32(chars.Length)]);
        }

        return secret.ToString();
    }

    public async Task<bool> ValidateTotpTokenAsync(string secret, string token)
    {
        // This is a simplified implementation
        // In a real scenario, you would use a proper TOTP library
        try
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var timeStep = 30; // 30 seconds
            var counter = (long)(timestamp / timeStep);

            // Generate expected token (simplified)
            var expectedToken = GenerateTotpToken(secret, counter);
            
            return expectedToken == token;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateTotpToken(string secret, long counter)
    {
        // Simplified TOTP token generation
        // In a real implementation, you would use HMAC-SHA1
        var data = BitConverter.GetBytes(counter);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(data);

        var hash = SHA1.HashData(data);
        var offset = hash[hash.Length - 1] & 0x0F;
        var code = ((hash[offset] & 0x7F) << 24) |
                   ((hash[offset + 1] & 0xFF) << 16) |
                   ((hash[offset + 2] & 0xFF) << 8) |
                   (hash[offset + 3] & 0xFF);

        return (code % 1000000).ToString("D6");
    }

    public async Task<string> GenerateQrCodeAsync(string secret, string email)
    {
        var issuer = "MyShop";
        var accountName = email;
        var qrCodeString = $"otpauth://totp/{issuer}:{accountName}?secret={secret}&issuer={issuer}";
        
        // In a real implementation, you would generate an actual QR code image
        return qrCodeString;
    }

    public async Task<bool> CheckPasswordHistoryAsync(string userId, string newPassword, int historyCount = 5)
    {
        // In a real implementation, you would check against password history
        // For now, we'll just return true (no history check)
        return true;
    }

    public async Task<bool> IsPasswordExpiredAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        return user.RequiresPasswordChange;
    }

    public async Task<bool> IsAccountCompromisedAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        // Check for signs of compromise
        var recentFailedAttempts = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "LoginFailed" && 
                             al.Timestamp >= DateTime.UtcNow.AddHours(-24));

        return recentFailedAttempts > 10; // More than 10 failed attempts in 24 hours
    }

    public async Task<bool> CheckSuspiciousActivityAsync(string userId, string ipAddress, string userAgent)
    {
        // Check for suspicious patterns
        var recentLogins = await _context.AuditLogs
            .Where(al => al.UserId == userId && al.Action == "Login" && al.IsSuccess)
            .OrderByDescending(al => al.Timestamp)
            .Take(5)
            .ToListAsync();

        if (recentLogins.Count < 2) return false;

        // Check for IP address changes
        var uniqueIps = recentLogins.Select(al => al.IpAddress).Distinct().Count();
        if (uniqueIps > 3) return true;

        // Check for user agent changes
        var uniqueUserAgents = recentLogins.Select(al => al.UserAgent).Distinct().Count();
        if (uniqueUserAgents > 2) return true;

        return false;
    }

    public async Task<bool> LockAccountForSecurityAsync(string userId, string reason)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var lockoutEnd = DateTime.UtcNow.AddHours(24);
        var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);

        if (result.Succeeded)
        {
            await _auditService.LogUserActionAsync(userId, "AccountLockedForSecurity", "User", userId, 
                additionalData: $"Reason: {reason}", isSuccess: true);
        }

        return result.Succeeded;
    }

    public async Task<bool> UnlockAccountForSecurityAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.SetLockoutEndDateAsync(user, null);

        if (result.Succeeded)
        {
            await _auditService.LogUserActionAsync(userId, "AccountUnlockedForSecurity", "User", userId, 
                isSuccess: true);
        }

        return result.Succeeded;
    }

    public async Task<bool> RequirePasswordChangeAsync(string userId, string reason)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        // In a real implementation, you would set a flag requiring password change
        await _auditService.LogUserActionAsync(userId, "PasswordChangeRequired", "User", userId, 
            additionalData: $"Reason: {reason}", isSuccess: true);

        return true;
    }

    public async Task<bool> CheckBruteForceAttemptsAsync(string userId, string ipAddress)
    {
        var recentAttempts = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "LoginFailed" && 
                             al.IpAddress == ipAddress && al.Timestamp >= DateTime.UtcNow.AddMinutes(15));

        return recentAttempts > 5; // More than 5 failed attempts in 15 minutes
    }

    public async Task<bool> CheckGeolocationAnomalyAsync(string userId, string ipAddress)
    {
        // In a real implementation, you would check against known locations
        // For now, we'll just return false
        return false;
    }

    public async Task<bool> CheckDeviceAnomalyAsync(string userId, string userAgent)
    {
        var recentLogins = await _context.AuditLogs
            .Where(al => al.UserId == userId && al.Action == "Login" && al.IsSuccess)
            .OrderByDescending(al => al.Timestamp)
            .Take(10)
            .ToListAsync();

        if (recentLogins.Count < 3) return false;

        var commonUserAgent = recentLogins.GroupBy(al => al.UserAgent)
            .OrderByDescending(g => g.Count())
            .First().Key;

        return userAgent != commonUserAgent;
    }

    public async Task<bool> CheckTimeAnomalyAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var lastLogin = user.LastLoginAt;
        if (lastLogin == null) return false;

        var timeSinceLastLogin = DateTime.UtcNow - lastLogin.Value;
        return timeSinceLastLogin.TotalHours > 24; // Login after more than 24 hours
    }

    public async Task<bool> CheckBehaviorAnomalyAsync(string userId)
    {
        // In a real implementation, you would analyze user behavior patterns
        // For now, we'll just return false
        return false;
    }

    public async Task<SecurityReport> GenerateSecurityReportAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new SecurityReport { UserId = userId };

        var report = new SecurityReport
        {
            UserId = userId,
            SecurityScore = await CalculateSecurityScoreAsync(userId),
            Recommendations = await GenerateSecurityRecommendationsAsync(userId),
            RecentAlerts = await GetSecurityAlertsAsync(userId, 10),
            Metrics = await CalculateSecurityMetricsAsync(userId)
        };

        return report;
    }

    public async Task<SecurityReport> GenerateOverallSecurityReportAsync()
    {
        var report = new SecurityReport
        {
            UserId = "System",
            SecurityScore = await CalculateOverallSecurityScoreAsync(),
            Recommendations = await GenerateOverallSecurityRecommendationsAsync(),
            RecentAlerts = await GetOverallSecurityAlertsAsync(10),
            Metrics = await CalculateOverallSecurityMetricsAsync()
        };

        return report;
    }

    private async Task<SecurityScore> CalculateSecurityScoreAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new SecurityScore();

        var passwordScore = user.RequiresPasswordChange ? 0 : 100;
        var twoFactorScore = user.TwoFactorEnabled ? 100 : 0;
        var loginScore = user.LoginAttempts > 5 ? 0 : 100;
        var deviceScore = 100; // Placeholder
        var behaviorScore = 100; // Placeholder

        var overallScore = (passwordScore + twoFactorScore + loginScore + deviceScore + behaviorScore) / 5;

        return new SecurityScore
        {
            OverallScore = overallScore,
            PasswordScore = passwordScore,
            TwoFactorScore = twoFactorScore,
            LoginScore = loginScore,
            DeviceScore = deviceScore,
            BehaviorScore = behaviorScore,
            Level = overallScore switch
            {
                >= 80 => "Excellent",
                >= 60 => "Good",
                >= 40 => "Fair",
                >= 20 => "Poor",
                _ => "Critical"
            }
        };
    }

    private async Task<SecurityScore> CalculateOverallSecurityScoreAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var usersWithTwoFactor = await _context.Users.CountAsync(u => u.TwoFactorEnabled);
        var usersWithExpiredPasswords = await _context.Users.CountAsync(u => u.RequiresPasswordChange);
        var lockedUsers = await _context.Users.CountAsync(u => u.IsLocked);

        var twoFactorScore = totalUsers > 0 ? (usersWithTwoFactor * 100) / totalUsers : 0;
        var passwordScore = totalUsers > 0 ? ((totalUsers - usersWithExpiredPasswords) * 100) / totalUsers : 0;
        var lockoutScore = totalUsers > 0 ? ((totalUsers - lockedUsers) * 100) / totalUsers : 0;

        var overallScore = (twoFactorScore + passwordScore + lockoutScore) / 3;

        return new SecurityScore
        {
            OverallScore = overallScore,
            TwoFactorScore = twoFactorScore,
            PasswordScore = passwordScore,
            LoginScore = lockoutScore,
            DeviceScore = 100,
            BehaviorScore = 100,
            Level = overallScore switch
            {
                >= 80 => "Excellent",
                >= 60 => "Good",
                >= 40 => "Fair",
                >= 20 => "Poor",
                _ => "Critical"
            }
        };
    }

    private async Task<List<SecurityRecommendation>> GenerateSecurityRecommendationsAsync(string userId)
    {
        var recommendations = new List<SecurityRecommendation>();
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null) return recommendations;

        if (!user.TwoFactorEnabled)
        {
            recommendations.Add(new SecurityRecommendation
            {
                Type = "TwoFactor",
                Title = "Enable Two-Factor Authentication",
                Description = "Enable two-factor authentication to add an extra layer of security to your account.",
                Priority = "High",
                Action = "Enable 2FA"
            });
        }

        if (user.RequiresPasswordChange)
        {
            recommendations.Add(new SecurityRecommendation
            {
                Type = "Password",
                Title = "Change Password",
                Description = "Your password has expired and needs to be changed.",
                Priority = "High",
                Action = "Change Password"
            });
        }

        if (user.LoginAttempts > 3)
        {
            recommendations.Add(new SecurityRecommendation
            {
                Type = "Login",
                Title = "Review Login Attempts",
                Description = "You have multiple failed login attempts. Review your account security.",
                Priority = "Medium",
                Action = "Review Security"
            });
        }

        return recommendations;
    }

    private async Task<List<SecurityRecommendation>> GenerateOverallSecurityRecommendationsAsync()
    {
        var recommendations = new List<SecurityRecommendation>();

        var totalUsers = await _context.Users.CountAsync();
        var usersWithTwoFactor = await _context.Users.CountAsync(u => u.TwoFactorEnabled);
        var twoFactorPercentage = totalUsers > 0 ? (usersWithTwoFactor * 100) / totalUsers : 0;

        if (twoFactorPercentage < 50)
        {
            recommendations.Add(new SecurityRecommendation
            {
                Type = "TwoFactor",
                Title = "Increase Two-Factor Authentication Adoption",
                Description = $"Only {twoFactorPercentage}% of users have two-factor authentication enabled.",
                Priority = "High",
                Action = "Promote 2FA"
            });
        }

        var usersWithExpiredPasswords = await _context.Users.CountAsync(u => u.RequiresPasswordChange);
        var passwordPercentage = totalUsers > 0 ? ((totalUsers - usersWithExpiredPasswords) * 100) / totalUsers : 0;

        if (passwordPercentage < 80)
        {
            recommendations.Add(new SecurityRecommendation
            {
                Type = "Password",
                Title = "Improve Password Policy",
                Description = $"{100 - passwordPercentage}% of users have expired passwords.",
                Priority = "Medium",
                Action = "Review Password Policy"
            });
        }

        return recommendations;
    }

    private async Task<List<SecurityAlert>> GetSecurityAlertsAsync(string userId, int count)
    {
        return await _context.AuditLogs
            .Where(al => al.UserId == userId && al.Severity >= AuditSeverity.Warning)
            .OrderByDescending(al => al.Timestamp)
            .Take(count)
            .Select(al => new SecurityAlert
            {
                Id = al.Id,
                UserId = al.UserId,
                Type = al.Action,
                Title = al.Action,
                Description = al.ErrorMessage ?? al.Action,
                Severity = al.Severity.ToString(),
                CreatedAt = al.Timestamp,
                IsActive = true
            })
            .ToListAsync();
    }

    private async Task<List<SecurityAlert>> GetOverallSecurityAlertsAsync(int count)
    {
        return await _context.AuditLogs
            .Where(al => al.Severity >= AuditSeverity.Warning)
            .OrderByDescending(al => al.Timestamp)
            .Take(count)
            .Select(al => new SecurityAlert
            {
                Id = al.Id,
                UserId = al.UserId,
                Type = al.Action,
                Title = al.Action,
                Description = al.ErrorMessage ?? al.Action,
                Severity = al.Severity.ToString(),
                CreatedAt = al.Timestamp,
                IsActive = true
            })
            .ToListAsync();
    }

    private async Task<SecurityMetrics> CalculateSecurityMetricsAsync(string userId)
    {
        var failedLoginAttempts = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "LoginFailed");
        
        var successfulLogins = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "Login" && al.IsSuccess);
        
        var passwordChanges = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "PasswordChanged");
        
        var twoFactorEnables = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "TwoFactorEnabled");
        
        var twoFactorDisables = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "TwoFactorDisabled");
        
        var accountLocks = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "AccountLocked");
        
        var accountUnlocks = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "AccountUnlocked");
        
        var suspiciousActivities = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Action == "SuspiciousActivity");
        
        var securityAlerts = await _context.AuditLogs
            .CountAsync(al => al.UserId == userId && al.Severity >= AuditSeverity.Warning);

        var lastPasswordChange = await _context.AuditLogs
            .Where(al => al.UserId == userId && al.Action == "PasswordChanged")
            .OrderByDescending(al => al.Timestamp)
            .Select(al => al.Timestamp)
            .FirstOrDefaultAsync();

        var lastLogin = await _context.AuditLogs
            .Where(al => al.UserId == userId && al.Action == "Login" && al.IsSuccess)
            .OrderByDescending(al => al.Timestamp)
            .Select(al => al.Timestamp)
            .FirstOrDefaultAsync();

        var lastSecurityAlert = await _context.AuditLogs
            .Where(al => al.UserId == userId && al.Severity >= AuditSeverity.Warning)
            .OrderByDescending(al => al.Timestamp)
            .Select(al => al.Timestamp)
            .FirstOrDefaultAsync();

        return new SecurityMetrics
        {
            FailedLoginAttempts = failedLoginAttempts,
            SuccessfulLogins = successfulLogins,
            PasswordChanges = passwordChanges,
            TwoFactorEnables = twoFactorEnables,
            TwoFactorDisables = twoFactorDisables,
            AccountLocks = accountLocks,
            AccountUnlocks = accountUnlocks,
            SuspiciousActivities = suspiciousActivities,
            SecurityAlerts = securityAlerts,
            LastPasswordChange = lastPasswordChange,
            LastLogin = lastLogin,
            LastSecurityAlert = lastSecurityAlert
        };
    }

    private async Task<SecurityMetrics> CalculateOverallSecurityMetricsAsync()
    {
        var failedLoginAttempts = await _context.AuditLogs
            .CountAsync(al => al.Action == "LoginFailed");
        
        var successfulLogins = await _context.AuditLogs
            .CountAsync(al => al.Action == "Login" && al.IsSuccess);
        
        var passwordChanges = await _context.AuditLogs
            .CountAsync(al => al.Action == "PasswordChanged");
        
        var twoFactorEnables = await _context.AuditLogs
            .CountAsync(al => al.Action == "TwoFactorEnabled");
        
        var twoFactorDisables = await _context.AuditLogs
            .CountAsync(al => al.Action == "TwoFactorDisabled");
        
        var accountLocks = await _context.AuditLogs
            .CountAsync(al => al.Action == "AccountLocked");
        
        var accountUnlocks = await _context.AuditLogs
            .CountAsync(al => al.Action == "AccountUnlocked");
        
        var suspiciousActivities = await _context.AuditLogs
            .CountAsync(al => al.Action == "SuspiciousActivity");
        
        var securityAlerts = await _context.AuditLogs
            .CountAsync(al => al.Severity >= AuditSeverity.Warning);

        var lastPasswordChange = await _context.AuditLogs
            .Where(al => al.Action == "PasswordChanged")
            .OrderByDescending(al => al.Timestamp)
            .Select(al => al.Timestamp)
            .FirstOrDefaultAsync();

        var lastLogin = await _context.AuditLogs
            .Where(al => al.Action == "Login" && al.IsSuccess)
            .OrderByDescending(al => al.Timestamp)
            .Select(al => al.Timestamp)
            .FirstOrDefaultAsync();

        var lastSecurityAlert = await _context.AuditLogs
            .Where(al => al.Severity >= AuditSeverity.Warning)
            .OrderByDescending(al => al.Timestamp)
            .Select(al => al.Timestamp)
            .FirstOrDefaultAsync();

        return new SecurityMetrics
        {
            FailedLoginAttempts = failedLoginAttempts,
            SuccessfulLogins = successfulLogins,
            PasswordChanges = passwordChanges,
            TwoFactorEnables = twoFactorEnables,
            TwoFactorDisables = twoFactorDisables,
            AccountLocks = accountLocks,
            AccountUnlocks = accountUnlocks,
            SuspiciousActivities = suspiciousActivities,
            SecurityAlerts = securityAlerts,
            LastPasswordChange = lastPasswordChange,
            LastLogin = lastLogin,
            LastSecurityAlert = lastSecurityAlert
        };
    }

    public async Task<bool> EnableSecurityMonitoringAsync(string userId)
    {
        await _auditService.LogUserActionAsync(userId, "SecurityMonitoringEnabled", "User", userId, 
            isSuccess: true);
        return true;
    }

    public async Task<bool> DisableSecurityMonitoringAsync(string userId)
    {
        await _auditService.LogUserActionAsync(userId, "SecurityMonitoringDisabled", "User", userId, 
            isSuccess: true);
        return true;
    }

    public async Task<bool> IsSecurityMonitoringEnabledAsync(string userId)
    {
        // In a real implementation, you would check a flag in the user's profile
        return true;
    }

    public async Task<List<SecurityAlert>> GetSecurityAlertsAsync(string userId, int count = 50)
    {
        return await GetSecurityAlertsAsync(userId, count);
    }

    public async Task<bool> AcknowledgeSecurityAlertAsync(string alertId, string userId)
    {
        await _auditService.LogUserActionAsync(userId, "SecurityAlertAcknowledged", "SecurityAlert", alertId, 
            isSuccess: true);
        return true;
    }

    public async Task<bool> ResolveSecurityAlertAsync(string alertId, string userId, string resolution)
    {
        await _auditService.LogUserActionAsync(userId, "SecurityAlertResolved", "SecurityAlert", alertId, 
            additionalData: $"Resolution: {resolution}", isSuccess: true);
        return true;
    }
}