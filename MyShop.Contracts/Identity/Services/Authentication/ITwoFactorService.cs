using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.TwoFactor;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface ITwoFactorService
{
    Task<Result<TwoFactorResponseDto>> EnableTwoFactorAsync(TwoFactorRequestDto request);

    Task<Result<TwoFactorResponseDto>> DisableTwoFactorAsync(TwoFactorRequestDto request);

    Task<Result> VerifyTwoFactorTokenAsync(VerifyTwoFactorRequestDto request);

    Task<Result<TwoFactorTokenResponseDto>> GenerateTwoFactorTokenAsync(TwoFactorRequestDto request);
}