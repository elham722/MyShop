using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Compliance requirement
    /// </summary>
    public class ComplianceRequirement
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public bool IsMet { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Evidence { get; set; } = string.Empty;
        public DateTime LastChecked { get; set; }
    }

}
