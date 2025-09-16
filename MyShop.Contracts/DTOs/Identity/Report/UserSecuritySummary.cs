using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// User security summary
    /// </summary>
    public class UserSecuritySummary
    {
        public bool TwoFactorEnabled { get; set; }
        public bool SmsEnabled { get; set; }
        public bool TotpEnabled { get; set; }
        public int FailedLoginAttempts { get; set; }
        public int SecurityAlerts { get; set; }
        public int SuspiciousActivities { get; set; }
        public DateTime LastPasswordChange { get; set; }
        public bool RequiresPasswordChange { get; set; }
        public int DaysSinceLastPasswordChange { get; set; }
        public string SecurityLevel { get; set; } = string.Empty;
    }

}
