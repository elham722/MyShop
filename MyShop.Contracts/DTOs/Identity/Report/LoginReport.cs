using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Login report model
    /// </summary>
    public class LoginReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalLogins { get; set; }
        public int SuccessfulLogins { get; set; }
        public int FailedLogins { get; set; }
        public double SuccessRate { get; set; }
        public int UniqueUsers { get; set; }
        public List<LoginSummary> LoginSummaries { get; set; } = new();
        public List<LoginTrend> LoginTrends { get; set; } = new();
        public List<LoginAnomaly> LoginAnomalies { get; set; } = new();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

}
