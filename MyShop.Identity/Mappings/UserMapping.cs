using Mapster;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Identity.Models;

namespace MyShop.Identity.Mappings;

/// <summary>
/// Mapping configuration for User entities and DTOs
/// </summary>
public class UserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, ApplicationUserDto>()
            // Base IdentityUser props
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.UserName, src => src.UserName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.EmailConfirmed, src => src.EmailConfirmed)
            .Map(dest => dest.PhoneNumberConfirmed, src => src.PhoneNumberConfirmed)
            .Map(dest => dest.TwoFactorEnabled, src => src.TwoFactorEnabled)
            .Map(dest => dest.LockoutEnabled, src => src.LockoutEnabled)
            .Map(dest => dest.AccessFailedCount, src => src.AccessFailedCount)
            .Map(dest => dest.LockoutEnd, src => src.LockoutEnd)

            // Custom props
            .Map(dest => dest.CustomerId, src => src.CustomerId)
            .Map(dest => dest.TotpEnabled, src => src.TotpEnabled)
            .Map(dest => dest.SmsEnabled, src => src.SmsEnabled)
            .Map(dest => dest.GoogleId, src => src.GoogleId)
            .Map(dest => dest.MicrosoftId, src => src.MicrosoftId)

            // Computed props (از متد یا پراپرتی محاسباتی کلاس)
            .Map(dest => dest.IsLocked, src => src.IsLocked)
            .Map(dest => dest.IsAccountLocked, src => src.IsAccountLocked)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.IsNewUser, src => src.IsNewUser)
            .Map(dest => dest.IsDeleted, src => src.IsDeleted)
            .Map(dest => dest.LastLoginAt, src => src.LastLoginAt)
            .Map(dest => dest.LastPasswordChangeAt, src => src.LastPasswordChangeAt)
            .Map(dest => dest.LoginAttempts, src => src.LoginAttempts)
            .Map(dest => dest.RequiresPasswordChange, src => src.RequiresPasswordChange)

            // Value Objects → DTO props
            .Map(dest => dest.CreatedAt, src => src.Account.CreatedAt)
            .Map(dest => dest.BranchId, src => src.Account.BranchId) // اگر BranchId توی AccountInfo باشه
            .Map(dest => dest.CreatedBy, src => src.Audit.CreatedBy)
            .Map(dest => dest.UpdatedAt, src => src.Audit.ModifiedAt)
            .Map(dest => dest.UpdatedBy, src => src.Audit.ModifiedBy);
    }
}
