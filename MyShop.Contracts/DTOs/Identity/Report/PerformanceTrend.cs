using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Performance trend
    /// </summary>
    public class PerformanceTrend
    {
        public DateTime Date { get; set; }
        public TimeSpan AverageResponseTime { get; set; }
        public int TotalRequests { get; set; }
        public int SlowRequests { get; set; }
        public int FailedRequests { get; set; }
        public double SuccessRate { get; set; }
        public int PeakConcurrency { get; set; }
        public DateTime PeakTime { get; set; }
    }

}
