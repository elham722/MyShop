namespace MyShop.Contracts.Identity.Services.Authentication;

public interface ITwoFactorService
{
    Task<bool> EnableTwoFactorAsync(string userId);

    Task<bool> DisableTwoFactorAsync(string userId);

    Task<bool> VerifyTwoFactorTokenAsync(string userId, string token);

    Task<string> GenerateTwoFactorTokenAsync(string userId);
}