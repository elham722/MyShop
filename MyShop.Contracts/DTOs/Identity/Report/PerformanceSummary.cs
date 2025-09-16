using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Performance summary
    /// </summary>
    public class PerformanceSummary
    {
        public TimeSpan AverageResponseTime { get; set; }
        public TimeSpan MedianResponseTime { get; set; }
        public TimeSpan P95ResponseTime { get; set; }
        public TimeSpan P99ResponseTime { get; set; }
        public int TotalRequests { get; set; }
        public int SlowRequests { get; set; }
        public int FailedRequests { get; set; }
        public double SuccessRate { get; set; }
        public int PeakConcurrency { get; set; }
        public DateTime PeakTime { get; set; }
    }

}
