using MyShop.Contracts.DTOs.Identity;

namespace MyShop.Contracts.Identity.Services
{
    /// <summary>
    /// Service for managing authentication operations
    /// </summary>
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(string email, string password, string? ipAddress = null, 
            string? userAgent = null, string? deviceInfo = null);
        Task<AuthenticationResult> LoginWithRefreshTokenAsync(string refreshToken, string? ipAddress = null, 
            string? userAgent = null);
        Task<bool> LogoutAsync(string userId, string? ipAddress = null, string? userAgent = null);
        Task<bool> LogoutAllDevicesAsync(string userId, string? ipAddress = null, string? userAgent = null);
        Task<AuthenticationResult> RegisterAsync(string email, string userName, string password, 
            string? customerId = null, string? ipAddress = null, string? userAgent = null);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<bool> ResendEmailConfirmationAsync(string email);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> EnableTwoFactorAsync(string userId);
        Task<bool> DisableTwoFactorAsync(string userId);
        Task<bool> VerifyTwoFactorTokenAsync(string userId, string token);
        Task<string> GenerateTwoFactorTokenAsync(string userId);
        Task<bool> LockUserAsync(string userId, TimeSpan? lockoutDuration = null);
        Task<bool> UnlockUserAsync(string userId);
        Task<bool> IsUserLockedAsync(string userId);
        Task<TimeSpan?> GetLockoutEndTimeAsync(string userId);
    }
}