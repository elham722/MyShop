using Microsoft.AspNetCore.Identity;
using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.LockUser;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.Authentication;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services.Authentication;

public class LockoutService : ILockoutService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _auditService;

    public LockoutService(UserManager<ApplicationUser> userManager, IAuditService auditService)
    {
        _userManager = userManager;
        _auditService = auditService;
    }

    public async Task<OperationResponseDto> LockUserAsync(LockUserRequestDto request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null) return false;

        var lockoutEnd = DateTime.UtcNow.Add(lockoutDuration ?? TimeSpan.FromMinutes(15));
        var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        
        await _auditService.LogUserActionAsync(userId, "UserLocked", "User", userId, 
            isSuccess: result.Succeeded);
        
        return result.Succeeded;
    }

    public async Task<bool> UnlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.SetLockoutEndDateAsync(user, null);
        
        await _auditService.LogUserActionAsync(userId, "UserUnlocked", "User", userId, 
            isSuccess: result.Succeeded);
        
        return result.Succeeded;
    }

    public async Task<bool> IsUserLockedAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.IsLocked ?? false;
    }

    public async Task<TimeSpan?> GetLockoutEndTimeAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user?.LockoutEnd == null) return null;

        var remaining = user.LockoutEnd.Value - DateTime.UtcNow;
        return remaining > TimeSpan.Zero ? remaining : null;
    }
}