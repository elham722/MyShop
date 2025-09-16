using MyShop.Contracts.DTOs.Identity.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Contracts.DTOs.Identity.Security;

namespace MyShop.Contracts.Identity.Services
{
    /// <summary>
    /// Service for generating Identity-related reports
    /// </summary>
    public interface IIdentityReportService
    {
        Task<UserReport> GenerateUserReportAsync(string userId);
        Task<UserReport> GenerateUserReportAsync(string userId, DateTime startDate, DateTime endDate);
        Task<RoleReport> GenerateRoleReportAsync(string roleId);
        Task<PermissionReport> GeneratePermissionReportAsync(string permissionId);
        Task<AuditReport> GenerateAuditReportAsync(DateTime startDate, DateTime endDate);
        Task<SecurityReport> GenerateSecurityReportAsync(DateTime startDate, DateTime endDate);
        Task<LoginReport> GenerateLoginReportAsync(DateTime startDate, DateTime endDate);
        Task<SystemReport> GenerateSystemReportAsync();
        Task<ComplianceReport> GenerateComplianceReportAsync();
        Task<PerformanceReport> GeneratePerformanceReportAsync(DateTime startDate, DateTime endDate);
        Task<ExportReport> ExportReportAsync(string reportType, DateTime startDate, DateTime endDate, string format = "PDF");
    }

}
