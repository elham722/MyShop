using Microsoft.AspNetCore.Identity;
using MyShop.Contracts.Common;
using MyShop.Contracts.DTOs.Identity.Authentication.Password;
using MyShop.Contracts.Identity.Services;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.Authentication;
using MyShop.Domain.Shared.Interfaces;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services.Authentication;

/// <summary>
/// Implementation of password service
/// </summary>
public class PasswordService : IPasswordService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _auditService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IUserContextService _userContextService;

    public PasswordService(
        UserManager<ApplicationUser> userManager,
        IAuditService auditService,
        IDateTimeService dateTimeService,
        IUserContextService userContextService)
    {
        _userManager = userManager;
        _auditService = auditService;
        _dateTimeService = dateTimeService;
        _userContextService = userContextService;
    }

    public async Task<Result> ForgotPasswordAsync(ForgotPasswordRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal if user exists or not for security reasons
                await _auditService.LogUserActionAsync(
                    "", 
                    "PasswordResetRequested", 
                    "User", 
                    "",
                    additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User not found"
                );
                
                // Return success even if user doesn't exist for security
                return Result.Success();
            }

            // Check if user is active
            if (!user.IsActive)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "PasswordResetRequested", 
                    "User", 
                    user.Id,
                    additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is inactive"
                );
                
                return Result.Failure("User account is inactive", "ACCOUNT_INACTIVE");
            }

            // Check if email is confirmed
            if (!user.EmailConfirmed)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "PasswordResetRequested", 
                    "User", 
                    user.Id,
                    additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Email not confirmed"
                );
                
                return Result.Failure("Please confirm your email address before resetting password", "EMAIL_NOT_CONFIRMED");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // TODO: Send email with reset token
            // await _emailService.SendPasswordResetEmailAsync(user.Email, token);
            
            await _auditService.LogUserActionAsync(
                user.Id, 
                "PasswordResetRequested", 
                "User", 
                user.Id,
                additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}, TokenGenerated: true",
                isSuccess: true
            );
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                "", 
                "PasswordResetRequested", 
                "User", 
                "",
                additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result.Failure("An error occurred while processing password reset request", "INTERNAL_ERROR");
        }
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        try
        {
            // Validate request
            if (request.NewPassword != request.ConfirmPassword)
            {
                return Result.Failure("New password and confirm password do not match", "PASSWORD_MISMATCH");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                await _auditService.LogUserActionAsync(
                    request.UserId, 
                    "PasswordReset", 
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
                    "PasswordReset", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is inactive"
                );
                
                return Result.Failure("User account is inactive", "ACCOUNT_INACTIVE");
            }

            // Check if user is locked
            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "PasswordReset", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is locked"
                );
                
                return Result.Failure("User account is locked", "ACCOUNT_LOCKED");
            }

            // Reset password
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            
            if (result.Succeeded)
            {
                // Update user account info
                user.UpdateAccount(user.Account.WithPasswordChanged(_dateTimeService));
                await _userManager.UpdateAsync(user);

                // Revoke all existing tokens for security
                await _userManager.UpdateSecurityStampAsync(user);

                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "PasswordReset", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}, PasswordChanged: true",
                    isSuccess: true
                );
                
                return Result.Success();
            }

            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            await _auditService.LogUserActionAsync(
                user.Id, 
                "PasswordReset", 
                "User", 
                user.Id,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: errorMessage
            );
            
            return Result.Failure(errorMessage, "PASSWORD_RESET_FAILED");
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                request.UserId, 
                "PasswordReset", 
                "User", 
                request.UserId,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result.Failure("An error occurred while resetting password", "INTERNAL_ERROR");
        }
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordRequestDto request)
    {
        try
        {
            // Validate request
            if (request.NewPassword != request.ConfirmPassword)
            {
                return Result.Failure("New password and confirm password do not match", "PASSWORD_MISMATCH");
            }

            if (request.CurrentPassword == request.NewPassword)
            {
                return Result.Failure("New password must be different from current password", "PASSWORD_SAME");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                await _auditService.LogUserActionAsync(
                    request.UserId, 
                    "PasswordChanged", 
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
                    "PasswordChanged", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is inactive"
                );
                
                return Result.Failure("User account is inactive", "ACCOUNT_INACTIVE");
            }

            // Check if user is locked
            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "PasswordChanged", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is locked"
                );
                
                return Result.Failure("User account is locked", "ACCOUNT_LOCKED");
            }

            // Change password
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            
            if (result.Succeeded)
            {
                // Update user account info
                user.UpdateAccount(user.Account.WithPasswordChanged(_dateTimeService));
                await _userManager.UpdateAsync(user);

                // Revoke all existing tokens for security
                await _userManager.UpdateSecurityStampAsync(user);

                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "PasswordChanged", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}, PasswordChanged: true",
                    isSuccess: true
                );
                
                return Result.Success();
            }

            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            await _auditService.LogUserActionAsync(
                user.Id, 
                "PasswordChanged", 
                "User", 
                user.Id,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: errorMessage
            );
            
            return Result.Failure(errorMessage, "PASSWORD_CHANGE_FAILED");
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                request.UserId, 
                "PasswordChanged", 
                "User", 
                request.UserId,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result.Failure("An error occurred while changing password", "INTERNAL_ERROR");
        }
    }
}