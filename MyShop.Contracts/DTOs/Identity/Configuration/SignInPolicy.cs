using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Configuration
{
    /// <summary>
    /// Sign-in policy settings
    /// </summary>
    public class SignInPolicy
    {
        public bool RequireConfirmedEmail { get; set; } = false;
        public bool RequireConfirmedPhoneNumber { get; set; } = false;
    }

}
