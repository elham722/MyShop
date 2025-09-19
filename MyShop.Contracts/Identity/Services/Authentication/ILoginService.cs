using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.Login;
using MyShop.Contracts.DTOs.Identity.Authentication.Logout;
using MyShop.Contracts.DTOs.Identity.Authentication.Token;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface ILoginService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, string? ipAddress = null, string? userAgent = null);

    Task<Result<LoginResponseDto>> LoginWithRefreshTokenAsync(RefreshTokenRequestDto request, string? ipAddress = null, string? userAgent = null);

    Task<Result> LogoutAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null);

    Task<Result> LogoutAllDevicesAsync(LogoutRequestDto request, string? ipAddress = null, string? userAgent = null);
}