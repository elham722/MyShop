using Microsoft.AspNetCore.Identity;
using MyShop.Contracts.Common;
using MyShop.Contracts.DTOs.Identity.Authentication.TwoFactor;
using MyShop.Contracts.Identity.Services;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.Authentication;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services.Authentication;

/// <summary>
/// Implementation of two-factor authentication service
/// </summary>
public class TwoFactorService : ITwoFactorService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _auditService;
    private readonly IUserContextService _userContextService;

    public TwoFactorService(
        UserManager<ApplicationUser> userManager, 
        IAuditService auditService,
        IUserContextService userContextService)
    {
        _userManager = userManager;
        _auditService = auditService;
        _userContextService = userContextService;
    }

    public async Task<Result<TwoFactorResponseDto>> EnableTwoFactorAsync(TwoFactorRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                await _auditService.LogUserActionAsync(
                    request.UserId, 
                    "TwoFactorEnabled", 
                    "User", 
                    request.UserId,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User not found"
                );
                
                return Result<TwoFactorResponseDto>.Failure("User not found", "USER_NOT_FOUND");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorEnabled", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is inactive"
                );
                
                return Result<TwoFactorResponseDto>.Failure("User account is inactive", "ACCOUNT_INACTIVE");
            }

            // Check if 2FA is already enabled
            if (user.TwoFactorEnabled)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorEnabled", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Two-factor authentication is already enabled"
                );
                
                return Result<TwoFactorResponseDto>.Failure("Two-factor authentication is already enabled", "TWO_FACTOR_ALREADY_ENABLED");
            }

            // Check if email is confirmed (required for 2FA)
            if (!user.EmailConfirmed)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorEnabled", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Email not confirmed"
                );
                
                return Result<TwoFactorResponseDto>.Failure("Please confirm your email address before enabling two-factor authentication", "EMAIL_NOT_CONFIRMED");
            }

            var result = await _userManager.SetTwoFactorEnabledAsync(user, true);
            
            if (result.Succeeded)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorEnabled", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}, TwoFactorEnabled: true",
                    isSuccess: true
                );

                var response = new TwoFactorResponseDto
                {
                    IsEnabled = true,
                    Method = "AuthenticatorApp",
                    Message = "Two-factor authentication has been enabled successfully",
                    Timestamp = DateTime.UtcNow
                };

                return Result<TwoFactorResponseDto>.Success(response);
            }

            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            await _auditService.LogUserActionAsync(
                user.Id, 
                "TwoFactorEnabled", 
                "User", 
                user.Id,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: errorMessage
            );
            
            return Result<TwoFactorResponseDto>.Failure(errorMessage, "TWO_FACTOR_ENABLE_FAILED");
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                request.UserId, 
                "TwoFactorEnabled", 
                "User", 
                request.UserId,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result<TwoFactorResponseDto>.Failure("An error occurred while enabling two-factor authentication", "INTERNAL_ERROR");
        }
    }

    public async Task<Result<TwoFactorResponseDto>> DisableTwoFactorAsync(TwoFactorRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                await _auditService.LogUserActionAsync(
                    request.UserId, 
                    "TwoFactorDisabled", 
                    "User", 
                    request.UserId,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User not found"
                );
                
                return Result<TwoFactorResponseDto>.Failure("User not found", "USER_NOT_FOUND");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorDisabled", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is inactive"
                );
                
                return Result<TwoFactorResponseDto>.Failure("User account is inactive", "ACCOUNT_INACTIVE");
            }

            // Check if 2FA is already disabled
            if (!user.TwoFactorEnabled)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorDisabled", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Two-factor authentication is already disabled"
                );
                
                return Result<TwoFactorResponseDto>.Failure("Two-factor authentication is already disabled", "TWO_FACTOR_ALREADY_DISABLED");
            }

            var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
            
            if (result.Succeeded)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorDisabled", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}, TwoFactorDisabled: true",
                    isSuccess: true
                );

                var response = new TwoFactorResponseDto
                {
                    IsEnabled = false,
                    Method = "AuthenticatorApp",
                    Message = "Two-factor authentication has been disabled successfully",
                    Timestamp = DateTime.UtcNow
                };

                return Result<TwoFactorResponseDto>.Success(response);
            }

            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            await _auditService.LogUserActionAsync(
                user.Id, 
                "TwoFactorDisabled", 
                "User", 
                user.Id,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: errorMessage
            );
            
            return Result<TwoFactorResponseDto>.Failure(errorMessage, "TWO_FACTOR_DISABLE_FAILED");
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                request.UserId, 
                "TwoFactorDisabled", 
                "User", 
                request.UserId,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result<TwoFactorResponseDto>.Failure("An error occurred while disabling two-factor authentication", "INTERNAL_ERROR");
        }
    }

    public async Task<Result> VerifyTwoFactorTokenAsync(VerifyTwoFactorRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                await _auditService.LogUserActionAsync(
                    request.UserId, 
                    "TwoFactorVerification", 
                    "User", 
                    request.UserId,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User not found"
                );
                
                return Result.Failure("User not found", "USER_NOT_FOUND");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorVerification", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is inactive"
                );
                
                return Result.Failure("User account is inactive", "ACCOUNT_INACTIVE");
            }

            // Check if 2FA is enabled
            if (!user.TwoFactorEnabled)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorVerification", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Two-factor authentication is not enabled"
                );
                
                return Result.Failure("Two-factor authentication is not enabled", "TWO_FACTOR_NOT_ENABLED");
            }

            // Validate token format
            if (string.IsNullOrWhiteSpace(request.Token) || request.Token.Length != 6 || !request.Token.All(char.IsDigit))
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorVerification", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Invalid token format"
                );
                
                return Result.Failure("Invalid token format", "INVALID_TOKEN_FORMAT");
            }

            var result = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Token);
            
            if (result)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorVerification", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}, TokenVerified: true",
                    isSuccess: true
                );
                
                return Result.Success();
            }

            await _auditService.LogUserActionAsync(
                user.Id, 
                "TwoFactorVerification", 
                "User", 
                user.Id,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: "Invalid token"
            );
            
            return Result.Failure("Invalid token", "INVALID_TOKEN");
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                request.UserId, 
                "TwoFactorVerification", 
                "User", 
                request.UserId,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result.Failure("An error occurred while verifying two-factor token", "INTERNAL_ERROR");
        }
    }

    public async Task<Result<TwoFactorTokenResponseDto>> GenerateTwoFactorTokenAsync(TwoFactorRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                await _auditService.LogUserActionAsync(
                    request.UserId, 
                    "TwoFactorTokenGenerated", 
                    "User", 
                    request.UserId,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User not found"
                );
                
                return Result<TwoFactorTokenResponseDto>.Failure("User not found", "USER_NOT_FOUND");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorTokenGenerated", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is inactive"
                );
                
                return Result<TwoFactorTokenResponseDto>.Failure("User account is inactive", "ACCOUNT_INACTIVE");
            }

            // Check if 2FA is enabled
            if (!user.TwoFactorEnabled)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorTokenGenerated", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Two-factor authentication is not enabled"
                );
                
                return Result<TwoFactorTokenResponseDto>.Failure("Two-factor authentication is not enabled", "TWO_FACTOR_NOT_ENABLED");
            }

            // Check if email is confirmed
            if (!user.EmailConfirmed)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "TwoFactorTokenGenerated", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Email not confirmed"
                );
                
                return Result<TwoFactorTokenResponseDto>.Failure("Please confirm your email address before generating two-factor token", "EMAIL_NOT_CONFIRMED");
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var expiresAt = DateTime.UtcNow.AddMinutes(5); // Token expires in 5 minutes
            
            await _auditService.LogUserActionAsync(
                user.Id, 
                "TwoFactorTokenGenerated", 
                "User", 
                user.Id,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}, TokenGenerated: true, ExpiresAt: {expiresAt:yyyy-MM-dd HH:mm:ss}",
                isSuccess: true
            );

            // TODO: Send token via email/SMS
            // await _emailService.SendTwoFactorTokenAsync(user.Email, token);

            var response = new TwoFactorTokenResponseDto
            {
                IsSuccess = true,
                Token = token, // In production, don't return token in response
                ExpiresAt = expiresAt,
                ExpiresInSeconds = 300, // 5 minutes
                TokenType = "TOTP"
            };

            return Result<TwoFactorTokenResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                request.UserId, 
                "TwoFactorTokenGenerated", 
                "User", 
                request.UserId,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result<TwoFactorTokenResponseDto>.Failure("An error occurred while generating two-factor token", "INTERNAL_ERROR");
        }
    }
}