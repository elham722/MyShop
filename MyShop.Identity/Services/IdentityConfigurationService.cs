using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyShop.Contracts.DTOs.Identity.Configuration;
using MyShop.Contracts.DTOs.Identity.JwtSettings;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Contracts.Identity.Services;

namespace MyShop.Identity.Services;

/// <summary>
/// Implementation of identity configuration service
/// </summary>
public class IdentityConfigurationService : Contracts.Identity.Services.IIdentityConfigurationService
{
    private readonly IConfiguration _configuration;
    private readonly MyShopIdentityDbContext _context;
    private readonly IAuditService _auditService;

    public IdentityConfigurationService(IConfiguration configuration, MyShopIdentityDbContext context,
        IAuditService auditService)
    {
        _configuration = configuration;
        _context = context;
        _auditService = auditService;
    }

    public async Task<Contracts.DTOs.Identity.Configuration.IdentityConfiguration> GetConfigurationAsync()
    {
        return new IdentityConfiguration
        {
            PasswordPolicy = new PasswordPolicy
            {
                RequireDigit = _configuration.GetValue<bool>("Identity:Password:RequireDigit", true),
                RequireLowercase = _configuration.GetValue<bool>("Identity:Password:RequireLowercase", true),
                RequireNonAlphanumeric = _configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric", true),
                RequireUppercase = _configuration.GetValue<bool>("Identity:Password:RequireUppercase", true),
                RequiredLength = _configuration.GetValue<int>("Identity:Password:RequiredLength", 8),
                RequiredUniqueChars = _configuration.GetValue<int>("Identity:Password:RequiredUniqueChars", 1)
            },
            LockoutPolicy = new LockoutPolicy
            {
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(_configuration.GetValue<int>("Identity:Lockout:DefaultLockoutTimeSpanMinutes", 5)),
                MaxFailedAccessAttempts = _configuration.GetValue<int>("Identity:Lockout:MaxFailedAccessAttempts", 5),
                AllowedForNewUsers = _configuration.GetValue<bool>("Identity:Lockout:AllowedForNewUsers", true)
            },
            UserPolicy = new UserPolicy
            {
                AllowedUserNameCharacters = _configuration.GetValue<string>("Identity:User:AllowedUserNameCharacters", 
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"),
                RequireUniqueEmail = _configuration.GetValue<bool>("Identity:User:RequireUniqueEmail", true)
            },
            SignInPolicy = new SignInPolicy
            {
                RequireConfirmedEmail = _configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail", false),
                RequireConfirmedPhoneNumber = _configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber", false)
            },
            JwtSettings = new JwtSettings
            {
                Key = _configuration.GetValue<string>("Jwt:Key", ""),
                Issuer = _configuration.GetValue<string>("Jwt:Issuer", ""),
                Audience = _configuration.GetValue<string>("Jwt:Audience", ""),
                AccessTokenExpirationMinutes = _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15),
                RefreshTokenExpirationDays = _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7)
            }
        };
    }

    public async Task<bool> UpdatePasswordPolicyAsync(PasswordPolicy policy)
    {
        try
        {
            // In a real implementation, you would update the configuration source
            // For now, we'll just log the change
            await _auditService.LogUserActionAsync("System", "PasswordPolicyUpdated", "Configuration", "PasswordPolicy", 
                newValues: $"RequireDigit: {policy.RequireDigit}, RequireLowercase: {policy.RequireLowercase}, " +
                          $"RequireNonAlphanumeric: {policy.RequireNonAlphanumeric}, RequireUppercase: {policy.RequireUppercase}, " +
                          $"RequiredLength: {policy.RequiredLength}, RequiredUniqueChars: {policy.RequiredUniqueChars}",
                isSuccess: true);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateLockoutPolicyAsync(LockoutPolicy policy)
    {
        try
        {
            await _auditService.LogUserActionAsync("System", "LockoutPolicyUpdated", "Configuration", "LockoutPolicy", 
                newValues: $"DefaultLockoutTimeSpan: {policy.DefaultLockoutTimeSpan}, " +
                          $"MaxFailedAccessAttempts: {policy.MaxFailedAccessAttempts}, " +
                          $"AllowedForNewUsers: {policy.AllowedForNewUsers}",
                isSuccess: true);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateUserPolicyAsync(UserPolicy policy)
    {
        try
        {
            await _auditService.LogUserActionAsync("System", "UserPolicyUpdated", "Configuration", "UserPolicy", 
                newValues: $"AllowedUserNameCharacters: {policy.AllowedUserNameCharacters}, " +
                          $"RequireUniqueEmail: {policy.RequireUniqueEmail}",
                isSuccess: true);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateSignInPolicyAsync(SignInPolicy policy)
    {
        try
        {
            await _auditService.LogUserActionAsync("System", "SignInPolicyUpdated", "Configuration", "SignInPolicy", 
                newValues: $"RequireConfirmedEmail: {policy.RequireConfirmedEmail}, " +
                          $"RequireConfirmedPhoneNumber: {policy.RequireConfirmedPhoneNumber}",
                isSuccess: true);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateJwtSettingsAsync(JwtSettings settings)
    {
        try
        {
            await _auditService.LogUserActionAsync("System", "JwtSettingsUpdated", "Configuration", "JwtSettings", 
                newValues: $"Issuer: {settings.Issuer}, Audience: {settings.Audience}, " +
                          $"AccessTokenExpirationMinutes: {settings.AccessTokenExpirationMinutes}, " +
                          $"RefreshTokenExpirationDays: {settings.RefreshTokenExpirationDays}",
                isSuccess: true);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ResetToDefaultsAsync()
    {
        try
        {
            await _auditService.LogUserActionAsync("System", "ConfigurationResetToDefaults", "Configuration", "All", 
                isSuccess: true);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Dictionary<string, object>> GetCurrentSettingsAsync()
    {
        var config = await GetConfigurationAsync();
        
        return new Dictionary<string, object>
        {
            { "PasswordPolicy", config.PasswordPolicy },
            { "LockoutPolicy", config.LockoutPolicy },
            { "UserPolicy", config.UserPolicy },
            { "SignInPolicy", config.SignInPolicy },
            { "JwtSettings", new { 
                Issuer = config.JwtSettings.Issuer, 
                Audience = config.JwtSettings.Audience,
                AccessTokenExpirationMinutes = config.JwtSettings.AccessTokenExpirationMinutes,
                RefreshTokenExpirationDays = config.JwtSettings.RefreshTokenExpirationDays
            }}
        };
    }

}