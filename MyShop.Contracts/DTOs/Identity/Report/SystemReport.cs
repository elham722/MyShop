using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// System report model
    /// </summary>
    public class SystemReport
    {
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public SystemSummary Summary { get; set; } = new();
        public SystemHealth Health { get; set; } = new();
        public SystemPerformance Performance { get; set; } = new();
        public SystemSecurity Security { get; set; } = new();
        public SystemCompliance Compliance { get; set; } = new();
    }

}
