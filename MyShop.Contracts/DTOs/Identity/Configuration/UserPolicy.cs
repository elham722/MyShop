using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Configuration
{
    /// <summary>
    /// User policy settings
    /// </summary>
    public class UserPolicy
    {
        public string AllowedUserNameCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        public bool RequireUniqueEmail { get; set; } = true;
    }
}
