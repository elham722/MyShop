using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Performance recommendation
    /// </summary>
    public class PerformanceRecommendation
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Impact { get; set; } = string.Empty;
        public string Effort { get; set; } = string.Empty;
        public string ExpectedImprovement { get; set; } = string.Empty;
    }

}
