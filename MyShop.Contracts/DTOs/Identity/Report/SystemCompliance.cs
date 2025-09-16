using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// System compliance
    /// </summary>
    public class SystemCompliance
    {
        public bool IsCompliant { get; set; }
        public string ComplianceStatus { get; set; } = string.Empty;
        public List<string> ComplianceIssues { get; set; } = new();
        public List<string> ComplianceRecommendations { get; set; } = new();
        public DateTime LastComplianceCheck { get; set; }
    }

}
