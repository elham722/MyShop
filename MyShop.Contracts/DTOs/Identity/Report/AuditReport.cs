using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Audit report model
    /// </summary>
    public class AuditReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalAuditLogs { get; set; }
        public int SuccessfulActions { get; set; }
        public int FailedActions { get; set; }
        public double SuccessRate { get; set; }
        public List<AuditSummary> AuditSummaries { get; set; } = new();
        public List<AuditTrend> AuditTrends { get; set; } = new();
        public List<AuditAlert> AuditAlerts { get; set; } = new();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

}
