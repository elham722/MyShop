using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Security trend
    /// </summary>
    public class SecurityTrend
    {
        public DateTime Date { get; set; }
        public int FailedLogins { get; set; }
        public int SuspiciousActivities { get; set; }
        public int SecurityAlerts { get; set; }
        public int AccountLocks { get; set; }
        public int PasswordChanges { get; set; }
        public int TwoFactorEnables { get; set; }
        public int TwoFactorDisables { get; set; }
    }

}
