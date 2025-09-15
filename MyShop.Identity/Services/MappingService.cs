using MyShop.Contracts.DTOs.Identity;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services;

/// <summary>
/// Service for mapping entities to DTOs
/// </summary>
public static class MappingService
{
    /// <summary>
    /// Maps ApplicationUser entity to ApplicationUserDto
    /// </summary>
    public static ApplicationUserDto MapToDto(ApplicationUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return new ApplicationUserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            TwoFactorEnabled = user.TwoFactorEnabled,
            LockoutEnabled = user.LockoutEnabled,
            AccessFailedCount = user.AccessFailedCount,
            LockoutEnd = user.LockoutEnd,
            CustomerId = user.CustomerId,
            TotpEnabled = user.TotpEnabled,
            SmsEnabled = user.SmsEnabled,
            GoogleId = user.GoogleId,
            MicrosoftId = user.MicrosoftId,
            IsLocked = user.IsLocked,
            IsAccountLocked = user.IsAccountLocked,
            IsActive = user.IsActive,
            IsNewUser = user.IsNewUser,
            IsDeleted = user.IsDeleted,
            LastLoginAt = user.LastLoginAt,
            LastPasswordChangeAt = user.LastPasswordChangeAt,
            LoginAttempts = user.LoginAttempts,
            RequiresPasswordChange = user.RequiresPasswordChange,
            CreatedAt = user.Account.CreatedAt,
            BranchId = user.Account.BranchId,
            CreatedBy = user.Audit.CreatedBy,
            UpdatedAt = user.Audit.ModifiedAt,
            UpdatedBy = user.Audit.ModifiedBy
        };
    }

    /// <summary>
    /// Maps collection of ApplicationUser entities to ApplicationUserDto collection
    /// </summary>
    public static IEnumerable<ApplicationUserDto> MapToDto(IEnumerable<ApplicationUser> users)
    {
        if (users == null)
            throw new ArgumentNullException(nameof(users));

        return users.Select(MapToDto);
    }

    /// <summary>
    /// Maps ApplicationUser entity to ApplicationUserDto with null check
    /// </summary>
    public static ApplicationUserDto? MapToDtoSafe(ApplicationUser? user)
    {
        return user == null ? null : MapToDto(user);
    }
}