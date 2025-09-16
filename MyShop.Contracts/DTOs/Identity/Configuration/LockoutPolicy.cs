using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Configuration
{
    /// <summary>
    /// Lockout policy settings
    /// </summary>
    public class LockoutPolicy
    {
        public TimeSpan DefaultLockoutTimeSpan { get; set; } = TimeSpan.FromMinutes(5);
        public int MaxFailedAccessAttempts { get; set; } = 5;
        public bool AllowedForNewUsers { get; set; } = true;
    }
}
