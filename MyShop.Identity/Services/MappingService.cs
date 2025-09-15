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

    /// <summary>
    /// Maps Role entity to RoleDto
    /// </summary>
    public static RoleDto MapToDto(Role role)
    {
        if (role == null)
            throw new ArgumentNullException(nameof(role));

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            NormalizedName = role.NormalizedName,
            Description = role.Description,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt,
            CreatedBy = role.CreatedBy,
            UpdatedBy = role.UpdatedBy,
            IsActive = role.IsActive,
            IsSystemRole = role.IsSystemRole,
            Priority = role.Priority,
            Category = role.Category,
            Permissions = role.RolePermissions?.Select(rp => MapToDto(rp.Permission)).ToList()
        };
    }

    /// <summary>
    /// Maps Permission entity to PermissionDto
    /// </summary>
    public static PermissionDto MapToDto(Permission permission)
    {
        if (permission == null)
            throw new ArgumentNullException(nameof(permission));

        return new PermissionDto
        {
            Id = permission.Id,
            Name = permission.Name,
            Resource = permission.Resource,
            Action = permission.Action,
            Description = permission.Description,
            CreatedAt = permission.CreatedAt,
            UpdatedAt = permission.UpdatedAt,
            CreatedBy = permission.CreatedBy,
            UpdatedBy = permission.UpdatedBy,
            IsActive = permission.IsActive,
            IsSystemPermission = permission.IsSystemPermission,
            Category = permission.Category,
            Priority = permission.Priority
        };
    }

    /// <summary>
    /// Maps UserToken entity to UserTokenDto
    /// </summary>
    public static UserTokenDto MapToDto(UserToken userToken)
    {
        if (userToken == null)
            throw new ArgumentNullException(nameof(userToken));

        return new UserTokenDto
        {
            UserId = userToken.UserId,
            LoginProvider = userToken.LoginProvider,
            Name = userToken.Name,
            Value = userToken.Value,
            CreatedAt = userToken.CreatedAt,
            UpdatedAt = userToken.UpdatedAt,
            CreatedBy = userToken.CreatedBy,
            UpdatedBy = userToken.UpdatedBy,
            IsActive = userToken.IsActive,
            ExpiresAt = userToken.ExpiresAt,
            DeviceInfo = userToken.DeviceInfo,
            IpAddress = userToken.IpAddress,
            UserAgent = userToken.UserAgent,
            IsRevoked = userToken.IsRevoked,
            RevokedAt = userToken.RevokedAt,
            RevokedBy = userToken.RevokedBy,
            RevocationReason = userToken.RevocationReason,
            TokenType = userToken.TokenType,
            TokenPurpose = userToken.TokenPurpose,
            UsageCount = userToken.UsageCount,
            LastUsedAt = userToken.LastUsedAt,
            ParentTokenId = userToken.ParentTokenId,
            IsRotated = userToken.IsRotated,
            RotatedAt = userToken.RotatedAt,
            RotatedBy = userToken.RotatedBy
        };
    }

    /// <summary>
    /// Maps RolePermission entity to RolePermissionDto
    /// </summary>
    public static RolePermissionDto MapToDto(RolePermission rolePermission)
    {
        if (rolePermission == null)
            throw new ArgumentNullException(nameof(rolePermission));

        return new RolePermissionDto
        {
            RoleId = rolePermission.RoleId,
            PermissionId = rolePermission.PermissionId,
            IsGranted = rolePermission.IsGranted,
            IsActive = rolePermission.IsActive,
            ExpiresAt = rolePermission.ExpiresAt,
            CreatedAt = rolePermission.CreatedAt,
            CreatedBy = rolePermission.CreatedBy,
            Role = rolePermission.Role != null ? MapToDto(rolePermission.Role) : null,
            Permission = rolePermission.Permission != null ? MapToDto(rolePermission.Permission) : null
        };
    }

    /// <summary>
    /// Maps collection of Role entities to RoleDto collection
    /// </summary>
    public static IEnumerable<RoleDto> MapToDto(IEnumerable<Role> roles)
    {
        if (roles == null)
            throw new ArgumentNullException(nameof(roles));

        return roles.Select(MapToDto);
    }

    /// <summary>
    /// Maps collection of Permission entities to PermissionDto collection
    /// </summary>
    public static IEnumerable<PermissionDto> MapToDto(IEnumerable<Permission> permissions)
    {
        if (permissions == null)
            throw new ArgumentNullException(nameof(permissions));

        return permissions.Select(MapToDto);
    }

    /// <summary>
    /// Maps collection of UserToken entities to UserTokenDto collection
    /// </summary>
    public static IEnumerable<UserTokenDto> MapToDto(IEnumerable<UserToken> userTokens)
    {
        if (userTokens == null)
            throw new ArgumentNullException(nameof(userTokens));

        return userTokens.Select(MapToDto);
    }
}