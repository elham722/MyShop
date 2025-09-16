using MyShop.Domain.Shared.ValueObjects.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Configuration
{
    /// <summary>
    /// Identity configuration model
    /// </summary>
    public class IdentityConfiguration
    {
        public PasswordPolicy PasswordPolicy { get; set; } = new();
        public LockoutPolicy LockoutPolicy { get; set; } = new();
        public UserPolicy UserPolicy { get; set; } = new();
        public SignInPolicy SignInPolicy { get; set; } = new();
        public JwtSettings.JwtSettings JwtSettings { get; set; } = new();
    }
}
