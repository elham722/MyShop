using MyShop.Contracts.DTOs.Identity;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface ILoginService
{
    Task<AuthenticationResult> LoginAsync(string email, string password, string? ipAddress = null, 
        string? userAgent = null, string? deviceInfo = null);

    Task<AuthenticationResult> LoginWithRefreshTokenAsync(string refreshToken, string? ipAddress = null, 
        string? userAgent = null);

    Task<bool> LogoutAsync(string userId, string? ipAddress = null, string? userAgent = null);

    Task<bool> LogoutAllDevicesAsync(string userId, string? ipAddress = null, string? userAgent = null);
}