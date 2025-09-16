using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// User activity summary
    /// </summary>
    public class UserActivitySummary
    {
        public int TotalLogins { get; set; }
        public int FailedLogins { get; set; }
        public int SuccessfulLogins { get; set; }
        public double LoginSuccessRate { get; set; }
        public int PasswordChanges { get; set; }
        public int RoleChanges { get; set; }
        public int PermissionChanges { get; set; }
        public DateTime FirstLogin { get; set; }
        public DateTime LastLogin { get; set; }
        public TimeSpan TotalSessionTime { get; set; }
        public int UniqueIpAddresses { get; set; }
        public int UniqueUserAgents { get; set; }
    }

}
