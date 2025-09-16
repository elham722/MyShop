using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Audit summary
    /// </summary>
    public class AuditSummary
    {
        public string Action { get; set; } = string.Empty;
        public int Count { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public double SuccessRate { get; set; }
        public string Severity { get; set; } = string.Empty;
        public DateTime LastOccurrence { get; set; }
    }

}
