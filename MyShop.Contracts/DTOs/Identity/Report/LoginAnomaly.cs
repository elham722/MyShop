using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Login anomaly
    /// </summary>
    public class LoginAnomaly
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime FirstOccurrence { get; set; }
        public DateTime LastOccurrence { get; set; }
        public List<string> AffectedUsers { get; set; } = new();
        public List<string> IpAddresses { get; set; } = new();
        public string Recommendation { get; set; } = string.Empty;
    }

}
