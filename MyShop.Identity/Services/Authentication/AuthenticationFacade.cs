using MyShop.Contracts.Common;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.Email;
using MyShop.Contracts.DTOs.Identity.Authentication.LockUser;
using MyShop.Contracts.DTOs.Identity.Authentication.Login;
using MyShop.Contracts.DTOs.Identity.Authentication.Logout;
using MyShop.Contracts.DTOs.Identity.Authentication.Password;
using MyShop.Contracts.DTOs.Identity.Authentication.Register;
using MyShop.Contracts.DTOs.Identity.Authentication.Token;
using MyShop.Contracts.DTOs.Identity.Authentication.TwoFactor;
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

    public Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, string? ipAddress = null, string? userAgent = null)
        => _loginService.LoginAsync(request, ipAddress, userAgent);

    public Task<Result<LoginResponseDto>> LoginWithRefreshTokenAsync(RefreshTokenRequestDto request, string? ipAddress = null, string? userAgent = null)
        => _loginService.LoginWithRefreshTokenAsync(request, ipAddress, userAgent);

    public Task<Result> LogoutAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null)
        => _loginService.LogoutAsync(request, ipAddress, userAgent);

    public Task<Result> LogoutAllDevicesAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null)
        => _loginService.LogoutAllDevicesAsync(request, ipAddress, userAgent);

    #endregion

    #region Registration Operations

    public Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request, string? ipAddress = null, string? userAgent = null)
        => _registrationService.RegisterAsync(request, ipAddress, userAgent);

    public Task<Result> ConfirmEmailAsync(ConfirmEmailRequestDto request)
        => _registrationService.ConfirmEmailAsync(request);

    public Task<Result> ResendEmailConfirmationAsync(ResendEmailConfirmationRequestDto request)
        => _registrationService.ResendEmailConfirmationAsync(request);

    #endregion

    #region Password Operations

    public Task<Result> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        => _passwordService.ForgotPasswordAsync(request);

    public Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request)
        => _passwordService.ResetPasswordAsync(request);

    public Task<Result> ChangePasswordAsync(ChangePasswordRequestDto request)
        => _passwordService.ChangePasswordAsync(request );

    #endregion

    #region Two-Factor Authentication Operations

    public Task<Result<TwoFactorResponseDto>> EnableTwoFactorAsync(TwoFactorRequestDto request)
        => _twoFactorService.EnableTwoFactorAsync(request);

    public Task<Result<TwoFactorResponseDto>> DisableTwoFactorAsync(TwoFactorRequestDto request)
        => _twoFactorService.DisableTwoFactorAsync(request);

    public Task<Result> VerifyTwoFactorTokenAsync(VerifyTwoFactorRequestDto request)
        => _twoFactorService.VerifyTwoFactorTokenAsync(request);

    public Task<Result<TwoFactorTokenResponseDto>> GenerateTwoFactorTokenAsync(TwoFactorRequestDto request)
        => _twoFactorService.GenerateTwoFactorTokenAsync(request);

    #endregion

    #region Lockout Operations

    public Task<Result<LockUserResponseDto>> LockUserAsync(LockUserRequestDto request)
        => _lockoutService.LockUserAsync(request);

    public Task<Result<UnlockUserResponseDto>> UnlockUserAsync(UnlockUserRequestDto request)
        => _lockoutService.UnlockUserAsync(request);

    public Task<Result<LockoutStatusResponseDto>> GetLockoutStatusAsync(string userId)
        => _lockoutService.GetLockoutStatusAsync(userId);

    public Task<Result<TimeSpan?>> GetLockoutEndTimeAsync(string userId)
        => _lockoutService.GetLockoutEndTimeAsync(userId);

    #endregion
}