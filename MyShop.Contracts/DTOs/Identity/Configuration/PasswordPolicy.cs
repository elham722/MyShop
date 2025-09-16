using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Configuration
{
    /// <summary>
    /// Password policy settings
    /// </summary>
    public class PasswordPolicy
    {
        public bool RequireDigit { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireNonAlphanumeric { get; set; } = true;
        public bool RequireUppercase { get; set; } = true;
        public int RequiredLength { get; set; } = 8;
        public int RequiredUniqueChars { get; set; } = 1;
    }

}
