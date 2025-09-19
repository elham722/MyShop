using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.Email;
using MyShop.Contracts.DTOs.Identity.Authentication.LockUser;
using MyShop.Contracts.DTOs.Identity.Authentication.Login;
using MyShop.Contracts.DTOs.Identity.Authentication.Logout;
using MyShop.Contracts.DTOs.Identity.Authentication.Password;
using MyShop.Contracts.DTOs.Identity.Authentication.Register;
using MyShop.Contracts.DTOs.Identity.Authentication.Token;
using MyShop.Contracts.DTOs.Identity.Authentication.TwoFactor;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface IAuthenticationFacade
{
    #region Login/Logout Operations
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, string? ipAddress = null, string? userAgent = null);

    Task<Result<LoginResponseDto>> LoginWithRefreshTokenAsync(RefreshTokenRequestDto request, string? ipAddress = null, string? userAgent = null);

    Task<Result> LogoutAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null);

    Task<Result> LogoutAllDevicesAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null);
    #endregion

    #region Registration Operations
    Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request, string? ipAddress = null, string? userAgent = null);

    Task<Result> ConfirmEmailAsync(ConfirmEmailRequestDto request);

    Task<Result> ResendEmailConfirmationAsync(ResendEmailConfirmationRequestDto request);
    #endregion

    #region Password Operations
    Task<Result> ForgotPasswordAsync(ForgotPasswordRequestDto request);

    Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request);

    Task<Result> ChangePasswordAsync(ChangePasswordRequestDto request);
    #endregion

    #region Two-Factor Authentication Operations
    Task<Result<TwoFactorResponseDto>> EnableTwoFactorAsync(TwoFactorRequestDto request);

    Task<Result<TwoFactorResponseDto>> DisableTwoFactorAsync(TwoFactorRequestDto request);

    Task<Result> VerifyTwoFactorTokenAsync(VerifyTwoFactorRequestDto request);

    Task<Result<TwoFactorTokenResponseDto>> GenerateTwoFactorTokenAsync(TwoFactorRequestDto request);
    #endregion

    #region Lockout Operations
    Task<Result<LockUserResponseDto>> LockUserAsync(LockUserRequestDto request);

    Task<Result<UnlockUserResponseDto>> UnlockUserAsync(UnlockUserRequestDto request);

    Task<Result<LockoutStatusResponseDto>> GetLockoutStatusAsync(string userId);

    Task<Result<TimeSpan?>> GetLockoutEndTimeAsync(string userId);
    #endregion
}
