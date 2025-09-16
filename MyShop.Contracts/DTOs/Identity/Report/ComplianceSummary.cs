using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Compliance summary
    /// </summary>
    public class ComplianceSummary
    {
        public bool IsCompliant { get; set; }
        public string ComplianceStatus { get; set; } = string.Empty;
        public int TotalRequirements { get; set; }
        public int MetRequirements { get; set; }
        public int Violations { get; set; }
        public double CompliancePercentage { get; set; }
        public DateTime LastAssessment { get; set; }
    }

}
