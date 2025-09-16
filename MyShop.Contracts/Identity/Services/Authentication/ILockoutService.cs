using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.LockUser;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface ILockoutService
{
    Task<OperationResponseDto> LockUserAsync(LockUserRequestDto request);

    Task<OperationResponseDto> UnlockUserAsync(UnlockUserRequestDto request);

    Task<LockoutStatusResponseDto> GetLockoutStatusAsync(string userId);

    Task<TimeSpan?> GetLockoutEndTimeAsync(string userId);
}