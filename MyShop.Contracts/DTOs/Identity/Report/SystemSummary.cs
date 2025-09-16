using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// System summary
    /// </summary>
    public class SystemSummary
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalRoles { get; set; }
        public int TotalPermissions { get; set; }
        public int TotalAuditLogs { get; set; }
        public DateTime SystemStartTime { get; set; }
        public TimeSpan Uptime { get; set; }
        public string Version { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
    }

}
