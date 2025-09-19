using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Contracts.Common;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.DTOs.Identity.Authentication.Login;
using MyShop.Contracts.DTOs.Identity.Authentication.Logout;
using MyShop.Contracts.DTOs.Identity.Authentication.Token;
using MyShop.Contracts.Identity.Services;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.Authentication;
using MyShop.Contracts.Identity.Services.JwtToken;
using MyShop.Domain.Shared.Interfaces;
using MyShop.Identity.Context;
using MyShop.Identity.Models;

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
    private readonly IUserContextService _userContextService;

    public LoginService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        MyShopIdentityDbContext context,
        IJwtTokenService jwtTokenService,
        IAuditService auditService,
        IDateTimeService dateTimeService,
        IUserContextService userContextService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _jwtTokenService = jwtTokenService;
        _auditService = auditService;
        _dateTimeService = dateTimeService;
        _userContextService = userContextService;
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, string? ipAddress = null, string? userAgent = null)
    {
        try
        {
            // Get context information if not provided
            ipAddress ??= _userContextService.GetCurrentUserIpAddress();
            userAgent ??= _userContextService.GetCurrentUserAgent();
            var deviceInfo = request.DeviceInfo ?? _userContextService.GetCurrentDeviceInfo();

            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null)
            {
                await _auditService.LogLoginAsync("", false, ipAddress, userAgent, deviceInfo, 
                    errorMessage: "User not found");
                return Result<LoginResponseDto>.Failure("Invalid credentials");
            }

            // Check if user is locked using UserManager for accuracy
            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {
                await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                    errorMessage: "Account is locked");
                return Result<LoginResponseDto>.Success(CreateLockedAccountResponse(user));
            }

            // Check if user is active
            if (!user.IsActive)
            {
                await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                    errorMessage: "Account is inactive");
                return Result<LoginResponseDto>.Failure("Account is inactive");
            }

            // Check if email is confirmed
            if (!user.EmailConfirmed)
            {
                await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                    errorMessage: "Email not confirmed");
                return Result<LoginResponseDto>.Success(CreateEmailConfirmationRequiredResponse());

            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            
            if (!result.Succeeded)
            {
                await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                    errorMessage: "Invalid password");
                return Result<LoginResponseDto>.Failure("Invalid credentials");
            }

            if (result.RequiresTwoFactor)
            {
                await _auditService.LogLoginAsync(user.Id, false, ipAddress, userAgent, deviceInfo, 
                    errorMessage: "Two-factor authentication required");
                return Result<LoginResponseDto>.Success(CreateTwoFactorRequiredResponse());

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

            var loginResponse = CreateSuccessLoginResponse(userDto, accessToken, refreshToken);
            return Result<LoginResponseDto>.Success(loginResponse);
        }
        catch (Exception ex)
        {
            await _auditService.LogLoginAsync("", false, ipAddress, userAgent, request.DeviceInfo, 
                errorMessage: ex.Message);
            return Result<LoginResponseDto>.Failure("An error occurred during login");
        }
    }

    public async Task<Result<LoginResponseDto>> LoginWithRefreshTokenAsync(RefreshTokenRequestDto request, string? ipAddress = null, string? userAgent = null)
    {
        try
        {
            // Get context information if not provided
            ipAddress ??= _userContextService.GetCurrentUserIpAddress();
            userAgent ??= _userContextService.GetCurrentUserAgent();

            var userToken = await _context.UserTokens
                .FirstOrDefaultAsync(ut => ut.Value == request.RefreshToken && ut.IsActive && !ut.IsRevoked);

            if (userToken == null || userToken.ExpiresAt < DateTime.UtcNow)
            {
                await _auditService.LogTokenOperationAsync("", "RefreshTokenValidation", 
                    ipAddress: ipAddress, userAgent: userAgent, isSuccess: false, 
                    errorMessage: "Invalid or expired refresh token");
                return Result<LoginResponseDto>.Failure("Invalid refresh token");
            }

            var user = await _userManager.FindByIdAsync(userToken.UserId);
            if (user == null || !user.IsActive)
            {
                await _auditService.LogTokenOperationAsync(userToken.UserId, "RefreshTokenValidation", 
                    ipAddress: ipAddress, userAgent: userAgent, isSuccess: false, 
                    errorMessage: "User not found or inactive");
                return Result<LoginResponseDto>.Failure("User not found");
            }

            // Check if user is locked
            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {
                await _auditService.LogTokenOperationAsync(user.Id, "RefreshTokenValidation", 
                    ipAddress: ipAddress, userAgent: userAgent, isSuccess: false, 
                    errorMessage: "Account is locked");
                return Result<LoginResponseDto>.Success(CreateLockedAccountResponse(user));

            }

            // Generate new tokens
            var userDto = user.Adapt<ApplicationUserDto>();
            var newAccessToken = await _jwtTokenService.GenerateAccessTokenAsync(userDto);
            var newRefreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(userDto);

            // Revoke old refresh token
            await _jwtTokenService.RevokeTokenAsync(request.RefreshToken);

            await _auditService.LogTokenOperationAsync(user.Id, "RefreshTokenUsed", 
                ipAddress: ipAddress, userAgent: userAgent, isSuccess: true);

            var loginResponse = CreateSuccessLoginResponse(userDto, newAccessToken, newRefreshToken);
            return Result<LoginResponseDto>.Success(loginResponse);
        }
        catch (Exception ex)
        {
            await _auditService.LogTokenOperationAsync("", "RefreshTokenValidation", 
                ipAddress: ipAddress, userAgent: userAgent, isSuccess: false, 
                errorMessage: ex.Message);
            return Result<LoginResponseDto>.Failure("An error occurred during token refresh");
        }
    }

    public async Task<Result> LogoutAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null)
    {
        try
        {
            // Get context information if not provided
            ipAddress ??= _userContextService.GetCurrentUserIpAddress();
            userAgent ??= _userContextService.GetCurrentUserAgent();

            await _auditService.LogLogoutAsync(request.UserId, ipAddress, userAgent);
            return Result.Success();
        }
        catch (Exception ex)
        {
            await _auditService.LogLogoutAsync(request.UserId, ipAddress, userAgent, ex.Message);
            return Result.Failure("An error occurred during logout");
        }
    }

    public async Task<Result> LogoutAllDevicesAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null)
    {
        try
        {
            // Get context information if not provided
            ipAddress ??= _userContextService.GetCurrentUserIpAddress();
            userAgent ??= _userContextService.GetCurrentUserAgent();

            await _jwtTokenService.RevokeAllUserTokensAsync(request.UserId);
            await _auditService.LogLogoutAsync(request.UserId, ipAddress, userAgent);
            return Result.Success();
        }
        catch (Exception ex)
        {
            await _auditService.LogLogoutAsync(request.UserId, ipAddress, userAgent, ex.Message);
            return Result.Failure("An error occurred during logout from all devices");
        }
    }

    #region Helper Methods

    private LoginResponseDto CreateSuccessLoginResponse(ApplicationUserDto userDto, string accessToken, string refreshToken)
    {
        return new LoginResponseDto
        {
            IsSuccess = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = userDto,
            ExpiresAt = DateTime.UtcNow.AddHours(1), // This should come from JWT token
            TokenType = "Bearer"
        };
    }

    private LoginResponseDto CreateLockedAccountResponse(ApplicationUser user)
    {
        return new LoginResponseDto
        {
            IsSuccess = false,
            ErrorMessage = "Account is locked",
            IsAccountLocked = true,
            LockoutEnd = user.LockoutEnd?.DateTime
        };
    }

    private LoginResponseDto CreateEmailConfirmationRequiredResponse()
    {
        return new LoginResponseDto
        {
            IsSuccess = false,
            ErrorMessage = "Please confirm your email address",
            RequiresEmailConfirmation = true
        };
    }

    private LoginResponseDto CreateTwoFactorRequiredResponse()
    {
        return new LoginResponseDto
        {
            IsSuccess = false,
            ErrorMessage = "Two-factor authentication required",
            RequiresTwoFactor = true
        };
    }

    #endregion
}