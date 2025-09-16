using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyShop.Contracts.Identity.Services;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Identity.Constants;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Identity.Mappings;
using MyShop.Domain.Shared.Interfaces;

namespace MyShop.Identity.Services;


/// <summary>
/// Implementation of authentication service
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly MyShopIdentityDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAuditService _auditService;
    private readonly IConfiguration _configuration;
    private readonly IDateTimeService _dateTimeService;

    public AuthenticationService(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, MyShopIdentityDbContext context,
        IJwtTokenService jwtTokenService, IAuditService auditService
        , IConfiguration configuration, IDateTimeService dateTimeService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _jwtTokenService = jwtTokenService;
        _auditService = auditService;
        _configuration = configuration;
        _dateTimeService = dateTimeService;

    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password, string? ipAddress = null, 
        string? userAgent = null, string? deviceInfo = null)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null)
        {
            await _auditService.LogLoginAsync("", false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "User not found");
            return new AuthenticationResult { IsSuccess = false, ErrorMessage = "Invalid credentials" };
        }

        // Check if user is locked
        if (user.IsLocked || user.IsAccountLocked)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Account is locked");
            return new AuthenticationResult 
            { 
                IsSuccess = false, 
                ErrorMessage = "Account is locked", 
                IsAccountLocked = true,
                LockoutEnd = user.LockoutEnd?.DateTime
            };
        }

        // Check if user is active
        if (!user.IsActive)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Account is inactive");
            return new AuthenticationResult { IsSuccess = false, ErrorMessage = "Account is inactive" };
        }

        // Check if email is confirmed
        if (!user.EmailConfirmed)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Email not confirmed");
            return new AuthenticationResult 
            { 
                IsSuccess = false, 
                ErrorMessage = "Please confirm your email address", 
                RequiresEmailConfirmation = true 
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        
        if (!result.Succeeded)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Invalid password");
            return new AuthenticationResult { IsSuccess = false, ErrorMessage = "Invalid credentials" };
        }

        if (result.RequiresTwoFactor)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Two-factor authentication required");
            return new AuthenticationResult 
            { 
                IsSuccess = false, 
                ErrorMessage = "Two-factor authentication required", 
                RequiresTwoFactor = true 
            };
        }

        // Generate tokens
        var userDto = user.Adapt<ApplicationUserDto>();
        var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(userDto);
        var refreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(userDto);

        // Update last login
      
        user.UpdateAccount(user.Account.WithLastLogin(_dateTimeService));
        await _userManager.UpdateAsync(user);

        // Log successful login
        await _auditService.LogLoginAsync(user.Id, true, ipAddress, userAgent, deviceInfo);

        return CreateSuccessResult(user, accessToken, refreshToken);
    }

    public async Task<AuthenticationResult> LoginWithRefreshTokenAsync(string refreshToken, string? ipAddress = null, 
        string? userAgent = null)
    {
        var userToken = await _context.UserTokens
            .FirstOrDefaultAsync(ut => ut.Value == refreshToken && ut.IsActive && !ut.IsRevoked);

        if (userToken == null || userToken.ExpiresAt < DateTime.UtcNow)
        {
            await _auditService.LogTokenOperationAsync("", "RefreshTokenValidation", 
                ipAddress: ipAddress, userAgent: userAgent, isSuccess: false, 
                errorMessage: "Invalid or expired refresh token");
            return new AuthenticationResult { IsSuccess = false, ErrorMessage = "Invalid refresh token" };
        }

        var user = await _userManager.FindByIdAsync(userToken.UserId);
        if (user == null || !user.IsActive)
        {
            await _auditService.LogTokenOperationAsync(userToken.UserId, "RefreshTokenValidation", 
                ipAddress: ipAddress, userAgent: userAgent, isSuccess: false, 
                errorMessage: "User not found or inactive");
            return new AuthenticationResult { IsSuccess = false, ErrorMessage = "User not found" };
        }

        // Generate new tokens
        var userDto = user.Adapt<ApplicationUserDto>();
        var newAccessToken = await _jwtTokenService.GenerateAccessTokenAsync(userDto);
        var newRefreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(userDto);

        // Revoke old refresh token
        await _jwtTokenService.RevokeTokenAsync(refreshToken);

        await _auditService.LogTokenOperationAsync(user.Id, "RefreshTokenUsed", 
            ipAddress: ipAddress, userAgent: userAgent, isSuccess: true);

        return new AuthenticationResult
        {
            IsSuccess = true,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            User = user.Adapt<ApplicationUserDto>()
        };
    }

    public async Task<bool> LogoutAsync(string userId, string? ipAddress = null, string? userAgent = null)
    {
        await _auditService.LogLogoutAsync(userId, ipAddress, userAgent);
        return true;
    }

    public async Task<bool> LogoutAllDevicesAsync(string userId, string? ipAddress = null, string? userAgent = null)
    {
        await _jwtTokenService.RevokeAllUserTokensAsync(userId);
        await _auditService.LogLogoutAsync(userId, ipAddress, userAgent);
        return true;
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string userName, string password, 
        string? customerId = null, string? ipAddress = null, string? userAgent = null)
    {
        var user = ApplicationUser.Create(email, userName, customerId, "System");
        
        var result = await _userManager.CreateAsync(user, password);
        
        if (!result.Succeeded)
        {
            await _auditService.LogUserActionAsync("", "UserRegistration", "User", "", 
                ipAddress: ipAddress, userAgent: userAgent, isSuccess: false, 
                errorMessage: string.Join(", ", result.Errors.Select(e => e.Description)));
            
            return new AuthenticationResult 
            { 
                IsSuccess = false, 
                ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)) 
            };
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, RoleConstants.User.Customer);

        // Generate email confirmation token
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        await _auditService.LogUserActionAsync(user.Id, "UserRegistration", "User", user.Id, 
            ipAddress: ipAddress, userAgent: userAgent, isSuccess: true);

        return new AuthenticationResult
        {
            IsSuccess = true,
            User = user.Adapt<ApplicationUserDto>(),
            RequiresEmailConfirmation = true
        };
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        
        await _auditService.LogUserActionAsync(userId, "EmailConfirmation", "User", userId, 
            isSuccess: result.Succeeded, errorMessage: result.Succeeded ? null : "Invalid token");
        
        return result.Succeeded;
    }

    public async Task<bool> ResendEmailConfirmationAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.EmailConfirmed) return false;

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        await _auditService.LogUserActionAsync(user.Id, "EmailConfirmationResent", "User", user.Id, 
            isSuccess: true);
        
        return true;
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        await _auditService.LogUserActionAsync(user.Id, "PasswordResetRequested", "User", user.Id, 
            isSuccess: true);
        
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        
        if (result.Succeeded)
        {
            user.UpdateAccount(user.Account.WithPasswordChanged(_dateTimeService));
            await _userManager.UpdateAsync(user);
        }
        
        await _auditService.LogUserActionAsync(userId, "PasswordReset", "User", userId, 
            isSuccess: result.Succeeded, errorMessage: result.Succeeded ? null : "Invalid token");
        
        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        
        if (result.Succeeded)
        {
            user.UpdateAccount(user.Account.WithPasswordChanged(_dateTimeService));
            await _userManager.UpdateAsync(user);
        }
        
        await _auditService.LogUserActionAsync(userId, "PasswordChanged", "User", userId, 
            isSuccess: result.Succeeded, errorMessage: result.Succeeded ? null : "Invalid current password");
        
        return result.Succeeded;
    }

    public async Task<bool> EnableTwoFactorAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.SetTwoFactorEnabledAsync(user, true);
        
        await _auditService.LogUserActionAsync(userId, "TwoFactorEnabled", "User", userId, 
            isSuccess: result.Succeeded);
        
        return result.Succeeded;
    }

    public async Task<bool> DisableTwoFactorAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
        
        await _auditService.LogUserActionAsync(userId, "TwoFactorDisabled", "User", userId, 
            isSuccess: result.Succeeded);
        
        return result.Succeeded;
    }

    public async Task<bool> VerifyTwoFactorTokenAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", token);
        
        await _auditService.LogUserActionAsync(userId, "TwoFactorVerification", "User", userId, 
            isSuccess: result, errorMessage: result ? null : "Invalid token");
        
        return result;
    }

    public async Task<string> GenerateTwoFactorTokenAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return string.Empty;

        return await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
    }

    public async Task<bool> LockUserAsync(string userId, TimeSpan? lockoutDuration = null)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var lockoutEnd = DateTime.UtcNow.Add(lockoutDuration ?? TimeSpan.FromMinutes(15));
        var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        
        await _auditService.LogUserActionAsync(userId, "UserLocked", "User", userId, 
            isSuccess: result.Succeeded);
        
        return result.Succeeded;
    }

    public async Task<bool> UnlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.SetLockoutEndDateAsync(user, null);
        
        await _auditService.LogUserActionAsync(userId, "UserUnlocked", "User", userId, 
            isSuccess: result.Succeeded);
        
        return result.Succeeded;
    }

    public async Task<bool> IsUserLockedAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.IsLocked ?? false;
    }

    public async Task<TimeSpan?> GetLockoutEndTimeAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user?.LockoutEnd == null) return null;

        var remaining = user.LockoutEnd.Value - DateTime.UtcNow;
        return remaining > TimeSpan.Zero ? remaining : null;
    }

    #region Helper Methods

    /// <summary>
    /// Helper method to create successful authentication result with user DTO
    /// </summary>
    private AuthenticationResult CreateSuccessResult(ApplicationUser user, string? accessToken = null, string? refreshToken = null)
    {
        return new AuthenticationResult
        {
            IsSuccess = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = user.Adapt<ApplicationUserDto>()
        };
    }

    /// <summary>
    /// Helper method to create failed authentication result
    /// </summary>
    private AuthenticationResult CreateFailureResult(string errorMessage, bool requiresEmailConfirmation = false, 
        bool requiresTwoFactor = false, bool isAccountLocked = false, DateTime? lockoutEnd = null)
    {
        return new AuthenticationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            RequiresEmailConfirmation = requiresEmailConfirmation,
            RequiresTwoFactor = requiresTwoFactor,
            IsAccountLocked = isAccountLocked,
            LockoutEnd = lockoutEnd
        };
    }

    #endregion
}