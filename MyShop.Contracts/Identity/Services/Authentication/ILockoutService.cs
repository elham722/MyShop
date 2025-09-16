namespace MyShop.Contracts.Identity.Services.Authentication;

public interface ILockoutService
{
    Task<bool> LockUserAsync(string userId, TimeSpan? lockoutDuration = null);

    Task<bool> UnlockUserAsync(string userId);

    Task<bool> IsUserLockedAsync(string userId);

    Task<TimeSpan?> GetLockoutEndTimeAsync(string userId);
}