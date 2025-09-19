using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.Email;
using MyShop.Contracts.DTOs.Identity.Authentication.Register;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface IRegistrationService
{
    Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request, string? ipAddress = null, string? userAgent = null);

    Task<Result> ConfirmEmailAsync(ConfirmEmailRequestDto request);

    Task<Result> ResendEmailConfirmationAsync(ResendEmailConfirmationRequestDto request);
}