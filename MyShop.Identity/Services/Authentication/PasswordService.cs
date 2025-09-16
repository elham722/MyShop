using Microsoft.AspNetCore.Identity;
using MyShop.Identity.Models;
using MyShop.Domain.Shared.Interfaces;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.Authentication;

namespace MyShop.Identity.Services.Authentication;

/// <summary>
/// Implementation of password service
/// </summary>
public class PasswordService : IPasswordService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _auditService;
    private readonly IDateTimeService _dateTimeService;

    public PasswordService(
        UserManager<ApplicationUser> userManager,
        IAuditService auditService,
        IDateTimeService dateTimeService)
    {
        _userManager = userManager;
        _auditService = auditService;
        _dateTimeService = dateTimeService;
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        await _auditService.LogUserActionAsync(user.Id, "PasswordResetRequested", "User", user.Id, 
            isSuccess: true);
        
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        
        if (result.Succeeded)
        {
            user.UpdateAccount(user.Account.WithPasswordChanged(_dateTimeService));
            await _userManager.UpdateAsync(user);
        }
        
        await _auditService.LogUserActionAsync(userId, "PasswordReset", "User", userId, 
            isSuccess: result.Succeeded, errorMessage: result.Succeeded ? null : "Invalid token");
        
        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        
        if (result.Succeeded)
        {
            user.UpdateAccount(user.Account.WithPasswordChanged(_dateTimeService));
            await _userManager.UpdateAsync(user);
        }
        
        await _auditService.LogUserActionAsync(userId, "PasswordChanged", "User", userId, 
            isSuccess: result.Succeeded, errorMessage: result.Succeeded ? null : "Invalid current password");
        
        return result.Succeeded;
    }
}