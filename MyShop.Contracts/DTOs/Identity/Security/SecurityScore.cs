using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Security
{
    /// <summary>
    /// Security score model
    /// </summary>
    public class SecurityScore
    {
        public int OverallScore { get; set; }
        public int PasswordScore { get; set; }
        public int TwoFactorScore { get; set; }
        public int LoginScore { get; set; }
        public int DeviceScore { get; set; }
        public int BehaviorScore { get; set; }
        public string Level { get; set; } = string.Empty;
    }

}
