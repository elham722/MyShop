using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Security
{
    /// <summary>
    /// Security metrics model
    /// </summary>
    public class SecurityMetrics
    {
        public int FailedLoginAttempts { get; set; }
        public int SuccessfulLogins { get; set; }
        public int PasswordChanges { get; set; }
        public int TwoFactorEnables { get; set; }
        public int TwoFactorDisables { get; set; }
        public int AccountLocks { get; set; }
        public int AccountUnlocks { get; set; }
        public int SuspiciousActivities { get; set; }
        public int SecurityAlerts { get; set; }
        public DateTime LastPasswordChange { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastSecurityAlert { get; set; }
    }
}
