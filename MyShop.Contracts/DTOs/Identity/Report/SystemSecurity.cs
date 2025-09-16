using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// System security
    /// </summary>
    public class SystemSecurity
    {
        public double SecurityScore { get; set; }
        public int FailedLoginAttempts { get; set; }
        public int SuspiciousActivities { get; set; }
        public int SecurityAlerts { get; set; }
        public int UsersWithTwoFactor { get; set; }
        public double TwoFactorAdoptionRate { get; set; }
        public List<string> SecurityIssues { get; set; } = new();
        public List<string> SecurityRecommendations { get; set; } = new();
    }

}
