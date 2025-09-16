using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.HealthCheck
{
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

}
