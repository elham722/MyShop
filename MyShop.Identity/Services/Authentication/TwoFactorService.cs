using Microsoft.AspNetCore.Identity;
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

    public TwoFactorService(UserManager<ApplicationUser> userManager, IAuditService auditService)
    {
        _userManager = userManager;
        _auditService = auditService;
    }

    public async Task<bool> EnableTwoFactorAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.SetTwoFactorEnabledAsync(user, true);
        
        await _auditService.LogUserActionAsync(userId, "TwoFactorEnabled", "User", userId, 
            isSuccess: result.Succeeded);
        
        return result.Succeeded;
    }

    public async Task<bool> DisableTwoFactorAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
        
        await _auditService.LogUserActionAsync(userId, "TwoFactorDisabled", "User", userId, 
            isSuccess: result.Succeeded);
        
        return result.Succeeded;
    }

    public async Task<bool> VerifyTwoFactorTokenAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", token);
        
        await _auditService.LogUserActionAsync(userId, "TwoFactorVerification", "User", userId, 
            isSuccess: result, errorMessage: result ? null : "Invalid token");
        
        return result;
    }

    public async Task<string> GenerateTwoFactorTokenAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return string.Empty;

        return await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
    }
}