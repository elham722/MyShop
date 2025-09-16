using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.HealthCheck
{
    /// <summary>
    /// Performance health status
    /// </summary>
    public class PerformanceHealthStatus
    {
        public bool IsHealthy { get; set; }
        public string Status { get; set; } = string.Empty;
        public TimeSpan AverageResponseTime { get; set; }
        public int SlowQueries { get; set; }
        public int HighMemoryUsage { get; set; }
        public int HighCpuUsage { get; set; }
        public List<HealthIssue> Issues { get; set; } = new();
    }

}
