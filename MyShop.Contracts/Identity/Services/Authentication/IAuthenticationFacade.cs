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

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface IAuthenticationFacade
{
    #region Login/Logout Operations
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string? ipAddress = null, string? userAgent = null);
    Task<LoginResponseDto> LoginWithRefreshTokenAsync(RefreshTokenRequestDto request, string? ipAddress = null, string? userAgent = null);
    Task<OperationResponseDto> LogoutAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null);
    Task<OperationResponseDto> LogoutAllDevicesAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null);
    #endregion

    #region Registration Operations
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, string? ipAddress = null, string? userAgent = null);
    Task<OperationResponseDto> ConfirmEmailAsync(ConfirmEmailRequestDto request);
    Task<OperationResponseDto> ResendEmailConfirmationAsync(ResendEmailConfirmationRequestDto request);
    #endregion

    #region Password Operations
    Task<OperationResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request);
    Task<OperationResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);
    Task<OperationResponseDto> ChangePasswordAsync(ChangePasswordRequestDto request);
    #endregion

    #region Two-Factor Authentication Operations
    Task<OperationResponseDto> EnableTwoFactorAsync(TwoFactorRequestDto request);
    Task<OperationResponseDto> DisableTwoFactorAsync(TwoFactorRequestDto request);
    Task<OperationResponseDto> VerifyTwoFactorTokenAsync(VerifyTwoFactorRequestDto request);
    Task<TwoFactorTokenResponseDto> GenerateTwoFactorTokenAsync(TwoFactorRequestDto request);
    #endregion

    #region Lockout Operations
    Task<OperationResponseDto> LockUserAsync(LockUserRequestDto request);
    Task<OperationResponseDto> UnlockUserAsync(UnlockUserRequestDto request);
    Task<LockoutStatusResponseDto> GetLockoutStatusAsync(string userId);
    Task<TimeSpan?> GetLockoutEndTimeAsync(string userId);
    #endregion
}