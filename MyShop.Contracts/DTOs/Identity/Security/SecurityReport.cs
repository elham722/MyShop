using MyShop.Contracts.DTOs.Identity.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Security
{
    /// <summary>
    /// Security report model
    /// </summary>
    public class SecurityReport
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public SecurityScore SecurityScore { get; set; } = new();
        public List<SecurityRecommendation> Recommendations { get; set; } = new();
        public List<SecurityAlert> RecentAlerts { get; set; } = new();
        public SecurityMetrics Metrics { get; set; } = new();
    }
}
