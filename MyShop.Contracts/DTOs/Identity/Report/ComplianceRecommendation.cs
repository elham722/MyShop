using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Compliance recommendation
    /// </summary>
    public class ComplianceRecommendation
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Impact { get; set; } = string.Empty;
        public string Effort { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

}
