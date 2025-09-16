using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Contracts.DTOs.Identity.Configuration;
using MyShop.Contracts.DTOs.Identity.JwtSettings;

namespace MyShop.Contracts.Identity.Services
{
    /// <summary>
    /// Service for managing Identity configuration and settings
    /// </summary>
    public interface IIdentityConfigurationService
    {
        Task<IdentityConfiguration> GetConfigurationAsync();
        Task<bool> UpdatePasswordPolicyAsync(PasswordPolicy policy);
        Task<bool> UpdateLockoutPolicyAsync(LockoutPolicy policy);
        Task<bool> UpdateUserPolicyAsync(UserPolicy policy);
        Task<bool> UpdateSignInPolicyAsync(SignInPolicy policy);
        Task<bool> UpdateJwtSettingsAsync(JwtSettings settings);
        Task<bool> ResetToDefaultsAsync();
        Task<Dictionary<string, object>> GetCurrentSettingsAsync();
    }

}
