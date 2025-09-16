using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.HealthCheck
{
    /// <summary>
    /// Database health status
    /// </summary>
    public class DatabaseHealthStatus
    {
        public bool IsHealthy { get; set; }
        public string Status { get; set; } = string.Empty;
        public TimeSpan ResponseTime { get; set; }
        public int ConnectionCount { get; set; }
        public string DatabaseVersion { get; set; } = string.Empty;
        public List<HealthIssue> Issues { get; set; } = new();
    }

}
