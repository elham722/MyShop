using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Compliance report model
    /// </summary>
    public class ComplianceReport
    {
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public ComplianceSummary Summary { get; set; } = new();
        public List<ComplianceRequirement> Requirements { get; set; } = new();
        public List<ComplianceViolation> Violations { get; set; } = new();
        public List<ComplianceRecommendation> Recommendations { get; set; } = new();
    }

}
