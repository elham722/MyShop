using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.TwoFactor;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface ITwoFactorService
{
    Task<OperationResponseDto> EnableTwoFactorAsync(TwoFactorRequestDto request);

    Task<OperationResponseDto> DisableTwoFactorAsync(TwoFactorRequestDto request);

    Task<OperationResponseDto> VerifyTwoFactorTokenAsync(VerifyTwoFactorRequestDto request);

    Task<TwoFactorTokenResponseDto> GenerateTwoFactorTokenAsync(TwoFactorRequestDto request);
}