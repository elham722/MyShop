using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// System performance
    /// </summary>
    public class SystemPerformance
    {
        public TimeSpan AverageResponseTime { get; set; }
        public int SlowQueries { get; set; }
        public int HighMemoryUsage { get; set; }
        public int HighCpuUsage { get; set; }
        public List<string> PerformanceIssues { get; set; } = new();
        public List<string> PerformanceRecommendations { get; set; } = new();
    }

}
