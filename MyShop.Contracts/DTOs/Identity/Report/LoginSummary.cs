using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Login summary
    /// </summary>
    public class LoginSummary
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalLogins { get; set; }
        public int SuccessfulLogins { get; set; }
        public int FailedLogins { get; set; }
        public double SuccessRate { get; set; }
        public DateTime FirstLogin { get; set; }
        public DateTime LastLogin { get; set; }
        public int UniqueIpAddresses { get; set; }
        public int UniqueUserAgents { get; set; }
        public List<string> IpAddresses { get; set; } = new();
        public List<string> UserAgents { get; set; } = new();
    }

}
