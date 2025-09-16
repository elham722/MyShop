using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.Password;

namespace MyShop.Contracts.Identity.Services.Authentication;

public interface IPasswordService
{
    Task<OperationResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request);

    Task<OperationResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);

    Task<OperationResponseDto> ChangePasswordAsync(ChangePasswordRequestDto request);
}