using Microsoft.AspNetCore.Identity;
using MyShop.Contracts.Enums.Identity;
using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.Shared;

namespace MyShop.Identity.Models
{
    public class AuditLog
    {
        public string Id { get; private set; } = null!;
        public string UserId { get; private set; } = null!;
        public string Action { get; private set; } = null!;
        public string EntityType { get; private set; } = null!;
        public string EntityId { get; private set; } = null!;
        public string? OldValues { get; private set; }
        public string? NewValues { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string IpAddress { get; private set; } = null!;
        public string? UserAgent { get; private set; }
        public string? DeviceInfo { get; private set; }
        public string? SessionId { get; private set; }
        public string? RequestId { get; private set; }
        public string? AdditionalData { get; private set; }
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public AuditSeverity Severity { get; private set; } = AuditSeverity.Info;

        // Navigation Properties
        public ApplicationUser? User { get; set; }

        private AuditLog() { } 

        public static AuditLog Create(string userId, string action, string entityType, string entityId, string? oldValues = null, string? newValues = null, string? ipAddress = null, string? userAgent = null, string? deviceInfo = null, string? sessionId = null, string? requestId = null, string? additionalData = null, bool isSuccess = true, string? errorMessage = null, AuditSeverity severity = AuditSeverity.Info)
        {
            Guard.AgainstNullOrEmpty(userId, nameof(userId));
            Guard.AgainstNullOrEmpty(action, nameof(action));
            Guard.AgainstNullOrEmpty(entityType, nameof(entityType));
            Guard.AgainstNullOrEmpty(entityId, nameof(entityId));

            return new AuditLog
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                OldValues = oldValues,
                NewValues = newValues,
                Timestamp = DateTime.UtcNow,
                IpAddress = ipAddress ?? "Unknown",
                UserAgent = userAgent,
                DeviceInfo = deviceInfo,
                SessionId = sessionId,
                RequestId = requestId,
                AdditionalData = additionalData,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                Severity = severity
            };
        }

        public static AuditLog CreateForLogin(string userId, bool isSuccess, string? ipAddress = null, string? userAgent = null, string? deviceInfo = null, string? sessionId = null, string? errorMessage = null)
        {
            return Create(
                userId: userId,
                action: isSuccess ? "Login" : "LoginFailed",
                entityType: "User",
                entityId: userId,
                ipAddress: ipAddress,
                userAgent: userAgent,
                deviceInfo: deviceInfo,
                sessionId: sessionId,
                isSuccess: isSuccess,
                errorMessage: errorMessage,
                severity: isSuccess ? AuditSeverity.Info : AuditSeverity.Warning
            );
        }

        public static AuditLog CreateForLogout(string userId, string? ipAddress = null, string? userAgent = null, string? sessionId = null)
        {
            return Create(
                userId: userId,
                action: "Logout",
                entityType: "User",
                entityId: userId,
                ipAddress: ipAddress,
                userAgent: userAgent,
                sessionId: sessionId,
                isSuccess: true,
                severity: AuditSeverity.Info
            );
        }

        public static AuditLog CreateForRoleAssignment(string userId, string roleId, string action, string? oldValues = null, string? newValues = null, string? ipAddress = null, string? userAgent = null, string? additionalData = null)
        {
            return Create(
                userId: userId,
                action: action,
                entityType: "UserRole",
                entityId: roleId,
                oldValues: oldValues,
                newValues: newValues,
                ipAddress: ipAddress,
                userAgent: userAgent,
                additionalData: additionalData,
                isSuccess: true,
                severity: AuditSeverity.Info
            );
        }

        public static AuditLog CreateForPermissionChange(string userId, string permissionId, string action, string? oldValues = null, string? newValues = null, string? ipAddress = null, string? userAgent = null, string? additionalData = null)
        {
            return Create(
                userId: userId,
                action: action,
                entityType: "Permission",
                entityId: permissionId,
                oldValues: oldValues,
                newValues: newValues,
                ipAddress: ipAddress,
                userAgent: userAgent,
                additionalData: additionalData,
                isSuccess: true,
                severity: AuditSeverity.Info
            );
        }

        public static AuditLog CreateForTokenOperation(string userId, string action, string? tokenId = null, string? ipAddress = null, string? userAgent = null, string? additionalData = null, bool isSuccess = true, string? errorMessage = null)
        {
            return Create(
                userId: userId,
                action: action,
                entityType: "Token",
                entityId: tokenId ?? "Unknown",
                ipAddress: ipAddress,
                userAgent: userAgent,
                additionalData: additionalData,
                isSuccess: isSuccess,
                errorMessage: errorMessage,
                severity: isSuccess ? AuditSeverity.Info : AuditSeverity.Warning
            );
        }

        public void UpdateSuccess(bool isSuccess, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Severity = isSuccess ? AuditSeverity.Info : AuditSeverity.Warning;
        }

        public void AddAdditionalData(string additionalData)
        {
            AdditionalData = string.IsNullOrEmpty(AdditionalData) 
                ? additionalData 
                : $"{AdditionalData}; {additionalData}";
        }
    }
}