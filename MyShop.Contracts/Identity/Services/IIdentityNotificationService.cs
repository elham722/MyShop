using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.Identity.Services
{
    /// <summary>
    /// Service for managing Identity-related notifications
    /// </summary>
    public interface IIdentityNotificationService
    {
        Task<bool> SendWelcomeEmailAsync(string userId, string email, string userName);
        Task<bool> SendEmailConfirmationAsync(string userId, string email, string confirmationToken);
        Task<bool> SendPasswordResetEmailAsync(string userId, string email, string resetToken);
        Task<bool> SendPasswordChangedNotificationAsync(string userId, string email);
        Task<bool> SendAccountLockedNotificationAsync(string userId, string email, string reason);
        Task<bool> SendAccountUnlockedNotificationAsync(string userId, string email);
        Task<bool> SendTwoFactorEnabledNotificationAsync(string userId, string email);
        Task<bool> SendTwoFactorDisabledNotificationAsync(string userId, string email);
        Task<bool> SendRoleAssignedNotificationAsync(string userId, string email, string roleName);
        Task<bool> SendRoleRemovedNotificationAsync(string userId, string email, string roleName);
        Task<bool> SendSecurityAlertAsync(string userId, string email, string alertType, string message);
        Task<bool> SendLoginNotificationAsync(string userId, string email, string ipAddress, string userAgent);
        Task<bool> SendSuspiciousActivityAlertAsync(string userId, string email, string activity);
        Task<bool> SendAccountDeactivatedNotificationAsync(string userId, string email);
        Task<bool> SendAccountActivatedNotificationAsync(string userId, string email);
        Task<bool> SendDataExportNotificationAsync(string userId, string email, string exportType);
        Task<bool> SendDataDeletionNotificationAsync(string userId, string email);
        Task<bool> SendPrivacyPolicyUpdateNotificationAsync(string userId, string email);
        Task<bool> SendTermsOfServiceUpdateNotificationAsync(string userId, string email);
        Task<bool> SendMaintenanceNotificationAsync(string userId, string email, DateTime maintenanceStart, DateTime maintenanceEnd);
        Task<bool> SendSystemUpdateNotificationAsync(string userId, string email, string updateVersion);
    }

}
