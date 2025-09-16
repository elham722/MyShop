using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Role usage summary
    /// </summary>
    public class RoleUsageSummary
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int LockedUsers { get; set; }
        public int UsersWithTwoFactor { get; set; }
        public double TwoFactorAdoptionRate { get; set; }
        public int TotalLogins { get; set; }
        public int FailedLogins { get; set; }
        public double LoginSuccessRate { get; set; }
        public DateTime LastUserAdded { get; set; }
        public DateTime LastUserRemoved { get; set; }
    }

}
