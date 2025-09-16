using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Permission usage summary
    /// </summary>
    public class PermissionUsageSummary
    {
        public int TotalRoles { get; set; }
        public int ActiveRoles { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int UsageCount { get; set; }
        public DateTime LastUsed { get; set; }
        public DateTime FirstUsed { get; set; }
        public double UsageFrequency { get; set; }
        public List<string> TopUsers { get; set; } = new();
        public List<string> TopRoles { get; set; } = new();
    }

}
