using MyShop.Contracts.DTOs.Identity;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface IRegistrationService
{
    Task<AuthenticationResult> RegisterAsync(string email, string userName, string password, 
        string? customerId = null, string? ipAddress = null, string? userAgent = null);

    Task<bool> ConfirmEmailAsync(string userId, string token);

    Task<bool> ResendEmailConfirmationAsync(string email);
}