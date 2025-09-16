using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Audit trend
    /// </summary>
    public class AuditTrend
    {
        public DateTime Date { get; set; }
        public int TotalActions { get; set; }
        public int SuccessfulActions { get; set; }
        public int FailedActions { get; set; }
        public double SuccessRate { get; set; }
        public int UniqueUsers { get; set; }
        public int UniqueActions { get; set; }
    }

}
