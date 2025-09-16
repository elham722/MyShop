using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Performance report model
    /// </summary>
    public class PerformanceReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PerformanceSummary Summary { get; set; } = new();
        public List<PerformanceTrend> Trends { get; set; } = new();
        public List<PerformanceIssue> Issues { get; set; } = new();
        public List<PerformanceRecommendation> Recommendations { get; set; } = new();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

}
