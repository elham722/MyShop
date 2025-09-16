using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.HealthCheck
{
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

}
