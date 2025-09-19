
using MyShop.Contracts.DTOs.Identity.Authentication.LockUser;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface ILockoutService
{
    Task<Result<LockUserResponseDto>> LockUserAsync(LockUserRequestDto request);
    Task<Result<UnlockUserResponseDto>> UnlockUserAsync(UnlockUserRequestDto request);
    Task<Result<LockoutStatusResponseDto>> GetLockoutStatusAsync(string userId);
    Task<Result<TimeSpan?>> GetLockoutEndTimeAsync(string userId);
}