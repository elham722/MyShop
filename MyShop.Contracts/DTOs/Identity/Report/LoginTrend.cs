using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Login trend
    /// </summary>
    public class LoginTrend
    {
        public DateTime Date { get; set; }
        public int TotalLogins { get; set; }
        public int SuccessfulLogins { get; set; }
        public int FailedLogins { get; set; }
        public double SuccessRate { get; set; }
        public int UniqueUsers { get; set; }
        public int PeakHour { get; set; }
        public int PeakCount { get; set; }
    }

}
