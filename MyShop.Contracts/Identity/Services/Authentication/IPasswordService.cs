using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.Password;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface IPasswordService
{
    Task<Result> ForgotPasswordAsync(ForgotPasswordRequestDto request);

    Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request);

    Task<Result> ChangePasswordAsync(ChangePasswordRequestDto request);
}