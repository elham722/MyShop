using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.HealthCheck
{
    /// <summary>
    /// Overall Identity health status
    /// </summary>
    public class IdentityHealthStatus
    {
        public bool IsHealthy { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
        public List<HealthIssue> Issues { get; set; } = new();
        public HealthMetrics Metrics { get; set; } = new();
    }

}
