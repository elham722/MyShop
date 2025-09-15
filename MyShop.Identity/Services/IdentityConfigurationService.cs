using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyShop.Identity.Context;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services;

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

/// <summary>
/// Identity configuration model
/// </summary>
public class IdentityConfiguration
{
    public PasswordPolicy PasswordPolicy { get; set; } = new();
    public LockoutPolicy LockoutPolicy { get; set; } = new();
    public UserPolicy UserPolicy { get; set; } = new();
    public SignInPolicy SignInPolicy { get; set; } = new();
    public JwtSettings JwtSettings { get; set; } = new();
}

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

/// <summary>
/// Lockout policy settings
/// </summary>
public class LockoutPolicy
{
    public TimeSpan DefaultLockoutTimeSpan { get; set; } = TimeSpan.FromMinutes(5);
    public int MaxFailedAccessAttempts { get; set; } = 5;
    public bool AllowedForNewUsers { get; set; } = true;
}

/// <summary>
/// User policy settings
/// </summary>
public class UserPolicy
{
    public string AllowedUserNameCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    public bool RequireUniqueEmail { get; set; } = true;
}

/// <summary>
/// Sign-in policy settings
/// </summary>
public class SignInPolicy
{
    public bool RequireConfirmedEmail { get; set; } = false;
    public bool RequireConfirmedPhoneNumber { get; set; } = false;
}

/// <summary>
/// JWT settings
/// </summary>
public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpirationMinutes { get; set; } = 15;
    public int RefreshTokenExpirationDays { get; set; } = 7;
}

/// <summary>
/// Implementation of identity configuration service
/// </summary>
public class IdentityConfigurationService : IIdentityConfigurationService
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

    public async Task<IdentityConfiguration> GetConfigurationAsync()
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