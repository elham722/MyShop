using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Contracts.Identity.Services;
using MyShop.Domain.Shared.Interfaces;

namespace MyShop.Identity.Services;


/// <summary>
/// Service for managing user profiles and account information
/// </summary>
public interface IUserProfileService
{
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
    Task<ApplicationUser?> GetUserByUserNameAsync(string userName);
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 50);
    Task<IEnumerable<ApplicationUser>> GetActiveUsersAsync(int pageNumber = 1, int pageSize = 50);
    Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName, int pageNumber = 1, int pageSize = 50);
    Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm, int pageNumber = 1, int pageSize = 50);
    Task<bool> UpdateUserProfileAsync(string userId, string? email = null, string? userName = null, 
        string? customerId = null, string updatedBy = "System");
    Task<bool> UpdateUserAccountInfoAsync(string userId, bool? isActive = null, string updatedBy = "System");
    Task<bool> UpdateUserSecurityInfoAsync(string userId, bool? totpEnabled = null, 
        bool? smsEnabled = null, string? totpSecretKey = null, string updatedBy = "System");
    Task<bool> UpdateUserSocialLoginsAsync(string userId, string? googleId = null, 
        string? microsoftId = null, string updatedBy = "System");
    Task<bool> DeleteUserAsync(string userId, string deletedBy = "System");
    Task<bool> ActivateUserAsync(string userId, string activatedBy = "System");
    Task<bool> DeactivateUserAsync(string userId, string deactivatedBy = "System");
    Task<bool> SuspendUserAsync(string userId, TimeSpan? suspensionDuration = null, 
        string? reason = null, string suspendedBy = "System");
    Task<bool> UnsuspendUserAsync(string userId, string unsuspendedBy = "System");
    Task<bool> IsUserActiveAsync(string userId);
    Task<bool> IsUserSuspendedAsync(string userId);
    Task<DateTime?> GetUserSuspensionEndAsync(string userId);
    Task<int> GetUserCountAsync();
    Task<int> GetActiveUserCountAsync();
    Task<int> GetUserCountByRoleAsync(string roleName);
    Task<Dictionary<string, int>> GetUserCountByRoleAsync();
    Task<Dictionary<string, int>> GetUserCountByStatusAsync();
}

/// <summary>
/// Implementation of user profile service
/// </summary>
public class UserProfileService : IUserProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MyShopIdentityDbContext _context;
    private readonly IAuditService _auditService;

    public UserProfileService(UserManager<ApplicationUser> userManager, MyShopIdentityDbContext context,
        IAuditService auditService)
    {
        _userManager = userManager;
        _context = context;
        _auditService = auditService;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationUser?> GetUserByUserNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 50)
    {
        return await _context.Users
            .OrderBy(u => u.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationUser>> GetActiveUsersAsync(int pageNumber = 1, int pageSize = 50)
    {
        return await _context.Users
            .Where(u => u.IsActive)
            .OrderBy(u => u.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.UserRoles
            .Where(ur => ur.Role.Name == roleName && ur.IsActive)
            .Include(ur => ur.User)
            .Select(ur => ur.User)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.Users
            .Where(u => u.UserName!.Contains(searchTerm) || u.Email!.Contains(searchTerm))
            .OrderBy(u => u.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<bool> UpdateUserProfileAsync(string userId, string? email = null, string? userName = null, 
        string? customerId = null, string updatedBy = "System")
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var oldValues = $"Email: {user.Email}, UserName: {user.UserName}, CustomerId: {user.CustomerId}";

        if (!string.IsNullOrEmpty(email) && email != user.Email)
        {
            user.Email = email;
            user.NormalizedEmail = email.ToUpperInvariant();
        }

        if (!string.IsNullOrEmpty(userName) && userName != user.UserName)
        {
            user.UserName = userName;
            user.NormalizedUserName = userName.ToUpperInvariant();
        }

        if (customerId != user.CustomerId)
        {
            user.SetCustomerId(customerId);
        }

        var result = await _userManager.UpdateAsync(user);
        
        if (result.Succeeded)
        {
            var newValues = $"Email: {user.Email}, UserName: {user.UserName}, CustomerId: {user.CustomerId}";
            await _auditService.LogUserActionAsync(userId, "ProfileUpdated", "User", userId, 
                oldValues: oldValues, newValues: newValues, isSuccess: true);
        }

        return result.Succeeded;
    }

    public async Task<bool> UpdateUserAccountInfoAsync(string userId, bool? isActive = null, string updatedBy = "System")
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var oldValues = $"IsActive: {user.IsActive}";

        if (isActive.HasValue && isActive.Value != user.IsActive)
        {
            var dateTimeService = new SimpleDateTimeService();
            var newAccount = isActive.Value ? user.Account.Activate() : user.Account.Deactivate();
            user.UpdateAccount(newAccount);
        }

        var result = await _userManager.UpdateAsync(user);
        
        if (result.Succeeded)
        {
            var newValues = $"IsActive: {user.IsActive}";
            await _auditService.LogUserActionAsync(userId, "AccountInfoUpdated", "User", userId, 
                oldValues: oldValues, newValues: newValues, isSuccess: true);
        }

        return result.Succeeded;
    }

    public async Task<bool> UpdateUserSecurityInfoAsync(string userId, bool? totpEnabled = null, 
        bool? smsEnabled = null, string? totpSecretKey = null, string updatedBy = "System")
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var oldValues = $"TotpEnabled: {user.TotpEnabled}, SmsEnabled: {user.SmsEnabled}";

        if (totpEnabled.HasValue)
        {
            user.SetTotpEnabled(totpEnabled.Value);
        }

        if (smsEnabled.HasValue)
        {
            user.SetSmsEnabled(smsEnabled.Value);
        }

        if (!string.IsNullOrEmpty(totpSecretKey))
        {
            user.SetTotpSecretKey(totpSecretKey);
        }

        var result = await _userManager.UpdateAsync(user);
        
        if (result.Succeeded)
        {
            var newValues = $"TotpEnabled: {user.TotpEnabled}, SmsEnabled: {user.SmsEnabled}";
            await _auditService.LogUserActionAsync(userId, "SecurityInfoUpdated", "User", userId, 
                oldValues: oldValues, newValues: newValues, isSuccess: true);
        }

        return result.Succeeded;
    }

    public async Task<bool> UpdateUserSocialLoginsAsync(string userId, string? googleId = null, 
        string? microsoftId = null, string updatedBy = "System")
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var oldValues = $"GoogleId: {user.GoogleId}, MicrosoftId: {user.MicrosoftId}";

        if (googleId != user.GoogleId)
        {
            user.SetGoogleId(googleId);
        }

        if (microsoftId != user.MicrosoftId)
        {
            user.SetMicrosoftId(microsoftId);
        }

        var result = await _userManager.UpdateAsync(user);
        
        if (result.Succeeded)
        {
            var newValues = $"GoogleId: {user.GoogleId}, MicrosoftId: {user.MicrosoftId}";
            await _auditService.LogUserActionAsync(userId, "SocialLoginsUpdated", "User", userId, 
                oldValues: oldValues, newValues: newValues, isSuccess: true);
        }

        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(string userId, string deletedBy = "System")
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.DeleteAsync(user);
        
        if (result.Succeeded)
        {
            await _auditService.LogUserActionAsync(userId, "UserDeleted", "User", userId, 
                isSuccess: true);
        }

        return result.Succeeded;
    }

    public async Task<bool> ActivateUserAsync(string userId, string activatedBy = "System")
    {
        return await UpdateUserAccountInfoAsync(userId, true, activatedBy);
    }

    public async Task<bool> DeactivateUserAsync(string userId, string deactivatedBy = "System")
    {
        return await UpdateUserAccountInfoAsync(userId, false, deactivatedBy);
    }

    public async Task<bool> SuspendUserAsync(string userId, TimeSpan? suspensionDuration = null, 
        string? reason = null, string suspendedBy = "System")
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var lockoutEnd = DateTime.UtcNow.Add(suspensionDuration ?? TimeSpan.FromHours(24));
        var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        
        if (result.Succeeded)
        {
            await _auditService.LogUserActionAsync(userId, "UserSuspended", "User", userId, 
                additionalData: $"Reason: {reason}, Duration: {suspensionDuration}", isSuccess: true);
        }

        return result.Succeeded;
    }

    public async Task<bool> UnsuspendUserAsync(string userId, string unsuspendedBy = "System")
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.SetLockoutEndDateAsync(user, null);
        
        if (result.Succeeded)
        {
            await _auditService.LogUserActionAsync(userId, "UserUnsuspended", "User", userId, 
                isSuccess: true);
        }

        return result.Succeeded;
    }

    public async Task<bool> IsUserActiveAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.IsActive ?? false;
    }

    public async Task<bool> IsUserSuspendedAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.IsLocked ?? false;
    }

    public async Task<DateTime?> GetUserSuspensionEndAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.LockoutEnd?.DateTime;
    }

    public async Task<int> GetUserCountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<int> GetActiveUserCountAsync()
    {
        return await _context.Users.CountAsync(u => u.IsActive);
    }

    public async Task<int> GetUserCountByRoleAsync(string roleName)
    {
        return await _context.UserRoles
            .CountAsync(ur => ur.Role.Name == roleName && ur.IsActive);
    }

    public async Task<Dictionary<string, int>> GetUserCountByRoleAsync()
    {
        return await _context.UserRoles
            .Where(ur => ur.IsActive)
            .GroupBy(ur => ur.Role.Name)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetUserCountByStatusAsync()
    {
        var activeCount = await _context.Users.CountAsync(u => u.IsActive);
        var inactiveCount = await _context.Users.CountAsync(u => !u.IsActive);
        var lockedCount = await _context.Users.CountAsync(u => u.IsLocked);

        return new Dictionary<string, int>
        {
            { "Active", activeCount },
            { "Inactive", inactiveCount },
            { "Locked", lockedCount }
        };
    }
}