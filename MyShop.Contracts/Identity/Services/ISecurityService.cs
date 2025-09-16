using MyShop.Contracts.DTOs.Identity.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.Identity.Services
{
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

}
