namespace MyShop.Contracts.Identity.Services.Authentication;

public interface IPasswordService
{
    Task<bool> ForgotPasswordAsync(string email);

    Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);

    Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
}