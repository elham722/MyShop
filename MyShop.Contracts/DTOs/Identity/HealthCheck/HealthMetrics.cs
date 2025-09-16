using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.HealthCheck
{
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

}
