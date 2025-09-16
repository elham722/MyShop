using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.Identity.Services.Authentication;

namespace MyShop.Identity.Services.Authentication;

public class AuthenticationFacade : IAuthenticationFacade
{
    private readonly ILoginService _loginService;
    private readonly IRegistrationService _registrationService;
    private readonly IPasswordService _passwordService;
    private readonly ITwoFactorService _twoFactorService;
    private readonly ILockoutService _lockoutService;

    public AuthenticationFacade(
        ILoginService loginService,
        IRegistrationService registrationService,
        IPasswordService passwordService,
        ITwoFactorService twoFactorService,
        ILockoutService lockoutService)
    {
        _loginService = loginService;
        _registrationService = registrationService;
        _passwordService = passwordService;
        _twoFactorService = twoFactorService;
        _lockoutService = lockoutService;
    }

    #region Login/Logout Operations

    public Task<AuthenticationResult> LoginAsync(string email, string password, string? ipAddress = null, 
        string? userAgent = null, string? deviceInfo = null)
        => _loginService.LoginAsync(email, password, ipAddress, userAgent, deviceInfo);

    public Task<AuthenticationResult> LoginWithRefreshTokenAsync(string refreshToken, string? ipAddress = null, 
        string? userAgent = null)
        => _loginService.LoginWithRefreshTokenAsync(refreshToken, ipAddress, userAgent);

    public Task<bool> LogoutAsync(string userId, string? ipAddress = null, string? userAgent = null)
        => _loginService.LogoutAsync(userId, ipAddress, userAgent);

    public Task<bool> LogoutAllDevicesAsync(string userId, string? ipAddress = null, string? userAgent = null)
        => _loginService.LogoutAllDevicesAsync(userId, ipAddress, userAgent);

    #endregion

    #region Registration Operations

    public Task<AuthenticationResult> RegisterAsync(string email, string userName, string password, 
        string? customerId = null, string? ipAddress = null, string? userAgent = null)
        => _registrationService.RegisterAsync(email, userName, password, customerId, ipAddress, userAgent);

    public Task<bool> ConfirmEmailAsync(string userId, string token)
        => _registrationService.ConfirmEmailAsync(userId, token);

    public Task<bool> ResendEmailConfirmationAsync(string email)
        => _registrationService.ResendEmailConfirmationAsync(email);

    #endregion

    #region Password Operations

    public Task<bool> ForgotPasswordAsync(string email)
        => _passwordService.ForgotPasswordAsync(email);

    public Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        => _passwordService.ResetPasswordAsync(userId, token, newPassword);

    public Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        => _passwordService.ChangePasswordAsync(userId, currentPassword, newPassword);

    #endregion

    #region Two-Factor Authentication Operations

    public Task<bool> EnableTwoFactorAsync(string userId)
        => _twoFactorService.EnableTwoFactorAsync(userId);

    public Task<bool> DisableTwoFactorAsync(string userId)
        => _twoFactorService.DisableTwoFactorAsync(userId);

    public Task<bool> VerifyTwoFactorTokenAsync(string userId, string token)
        => _twoFactorService.VerifyTwoFactorTokenAsync(userId, token);

    public Task<string> GenerateTwoFactorTokenAsync(string userId)
        => _twoFactorService.GenerateTwoFactorTokenAsync(userId);

    #endregion

    #region Lockout Operations

    public Task<bool> LockUserAsync(string userId, TimeSpan? lockoutDuration = null)
        => _lockoutService.LockUserAsync(userId, lockoutDuration);

    public Task<bool> UnlockUserAsync(string userId)
        => _lockoutService.UnlockUserAsync(userId);

    public Task<bool> IsUserLockedAsync(string userId)
        => _lockoutService.IsUserLockedAsync(userId);

    public Task<TimeSpan?> GetLockoutEndTimeAsync(string userId)
        => _lockoutService.GetLockoutEndTimeAsync(userId);

    #endregion
}