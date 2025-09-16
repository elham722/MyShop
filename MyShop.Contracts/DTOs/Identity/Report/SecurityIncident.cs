using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Security incident
    /// </summary>
    public class SecurityIncident
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime FirstOccurrence { get; set; }
        public DateTime LastOccurrence { get; set; }
        public List<string> AffectedUsers { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public string Resolution { get; set; } = string.Empty;
    }

}
