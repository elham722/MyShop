using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Security recommendations
    /// </summary>
    public class SecurityRecommendations
    {
        public List<string> HighPriority { get; set; } = new();
        public List<string> MediumPriority { get; set; } = new();
        public List<string> LowPriority { get; set; } = new();
        public List<string> BestPractices { get; set; } = new();
        public List<string> ComplianceRequirements { get; set; } = new();
    }

}
