using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Domain.Shared.Interfaces;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.JwtToken;
using MyShop.Contracts.Identity.Services.Authentication;

namespace MyShop.Identity.Services.Authentication;

/// <summary>
/// Implementation of login service
/// </summary>
public class LoginService : ILoginService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly MyShopIdentityDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAuditService _auditService;
    private readonly IDateTimeService _dateTimeService;

    public LoginService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        MyShopIdentityDbContext context,
        IJwtTokenService jwtTokenService,
        IAuditService auditService,
        IDateTimeService dateTimeService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _jwtTokenService = jwtTokenService;
        _auditService = auditService;
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
            return CreateFailureResult("Invalid credentials");
        }

        // Check if user is locked
        if (user.IsLocked || user.IsAccountLocked)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Account is locked");
            return CreateFailureResult("Account is locked", isAccountLocked: true, lockoutEnd: user.LockoutEnd?.DateTime);
        }

        // Check if user is active
        if (!user.IsActive)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Account is inactive");
            return CreateFailureResult("Account is inactive");
        }

        // Check if email is confirmed
        if (!user.EmailConfirmed)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Email not confirmed");
            return CreateFailureResult("Please confirm your email address", requiresEmailConfirmation: true);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        
        if (!result.Succeeded)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Invalid password");
            return CreateFailureResult("Invalid credentials");
        }

        if (result.RequiresTwoFactor)
        {
            await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                errorMessage: "Two-factor authentication required");
            return CreateFailureResult("Two-factor authentication required", requiresTwoFactor: true);
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
            return CreateFailureResult("Invalid refresh token");
        }

        var user = await _userManager.FindByIdAsync(userToken.UserId);
        if (user == null || !user.IsActive)
        {
            await _auditService.LogTokenOperationAsync(userToken.UserId, "RefreshTokenValidation", 
                ipAddress: ipAddress, userAgent: userAgent, isSuccess: false, 
                errorMessage: "User not found or inactive");
            return CreateFailureResult("User not found");
        }

        // Generate new tokens
        var userDto = user.Adapt<ApplicationUserDto>();
        var newAccessToken = await _jwtTokenService.GenerateAccessTokenAsync(userDto);
        var newRefreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(userDto);

        // Revoke old refresh token
        await _jwtTokenService.RevokeTokenAsync(refreshToken);

        await _auditService.LogTokenOperationAsync(user.Id, "RefreshTokenUsed", 
            ipAddress: ipAddress, userAgent: userAgent, isSuccess: true);

        return CreateSuccessResult(user, newAccessToken, newRefreshToken);
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

    #region Helper Methods

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