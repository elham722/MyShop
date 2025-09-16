using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Compliance violation
    /// </summary>
    public class ComplianceViolation
    {
        public string Id { get; set; } = string.Empty;
        public string RequirementId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Resolution { get; set; } = string.Empty;
        public DateTime? ResolvedAt { get; set; }
    }

}
