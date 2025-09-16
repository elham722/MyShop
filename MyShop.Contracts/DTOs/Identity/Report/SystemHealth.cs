using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// System health
    /// </summary>
    public class SystemHealth
    {
        public bool IsHealthy { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> Issues { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public DateTime LastHealthCheck { get; set; }
    }

}
