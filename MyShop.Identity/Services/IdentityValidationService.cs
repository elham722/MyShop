using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Contracts.Enums.Identity;

namespace MyShop.Identity.Services;

/// <summary>
/// Service for managing Identity-related validation
/// </summary>
public interface IIdentityValidationService
{
    Task<ValidationResult> ValidateUserAsync(ApplicationUser user);
    Task<ValidationResult> ValidateUserAsync(string userId);
    Task<ValidationResult> ValidateRoleAsync(Role role);
    Task<ValidationResult> ValidateRoleAsync(string roleId);
    Task<ValidationResult> ValidatePermissionAsync(Permission permission);
    Task<ValidationResult> ValidatePermissionAsync(string permissionId);
    Task<ValidationResult> ValidateUserRoleAsync(string userId, string roleId);
    Task<ValidationResult> ValidateRolePermissionAsync(string roleId, string permissionId);
    Task<ValidationResult> ValidatePasswordAsync(string password);
    Task<ValidationResult> ValidateEmailAsync(string email);
    Task<ValidationResult> ValidateUserNameAsync(string userName);
    Task<ValidationResult> ValidatePhoneNumberAsync(string phoneNumber);
    Task<ValidationResult> ValidateTotpTokenAsync(string secret, string token);
    Task<ValidationResult> ValidateRefreshTokenAsync(string refreshToken);
    Task<ValidationResult> ValidateAccessTokenAsync(string accessToken);
    Task<ValidationResult> ValidateUserClaimAsync(string userId, string claimType, string claimValue);
    Task<ValidationResult> ValidateUserLoginAsync(string userId, string loginProvider, string providerKey);
    Task<ValidationResult> ValidateUserTokenAsync(string userId, string loginProvider, string name, string value);
    Task<ValidationResult> ValidateAuditLogAsync(AuditLog auditLog);
    Task<ValidationResult> ValidateBusinessRuleAsync(string ruleName, object data);
    Task<ValidationResult> ValidateComplianceAsync(string requirement, object data);
    Task<ValidationResult> ValidateSecurityAsync(string userId, string action, object data);
    Task<ValidationResult> ValidatePerformanceAsync(string operation, object data);
    Task<ValidationResult> ValidateDataIntegrityAsync();
    Task<ValidationResult> ValidateSystemHealthAsync();
    Task<ValidationResult> ValidateConfigurationAsync();
    Task<ValidationResult> ValidatePermissionsAsync(string userId, string resource, string action);
    Task<ValidationResult> ValidateRoleHierarchyAsync(string userId, string roleId);
    Task<ValidationResult> ValidatePermissionHierarchyAsync(string roleId, string permissionId);
}

/// <summary>
/// Validation result model
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<ValidationError> Errors { get; set; } = new();
    public List<ValidationWarning> Warnings { get; set; } = new();
    public List<ValidationInfo> Infos { get; set; } = new();
    public DateTime ValidatedAt { get; set; } = DateTime.UtcNow;
    public string ValidatedBy { get; set; } = "System";
    public string ValidationType { get; set; } = string.Empty;
    public object? Data { get; set; }
}

/// <summary>
/// Validation error model
/// </summary>
public class ValidationError
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public string Severity { get; set; } = "Error";
    public string Category { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Validation warning model
/// </summary>
public class ValidationWarning
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public string Severity { get; set; } = "Warning";
    public string Category { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Validation info model
/// </summary>
public class ValidationInfo
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public string Severity { get; set; } = "Info";
    public string Category { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Implementation of identity validation service
/// </summary>
public class IdentityValidationService : IIdentityValidationService
{
    private readonly MyShopIdentityDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<IdentityValidationService> _logger;

    public IdentityValidationService(MyShopIdentityDbContext context, UserManager<ApplicationUser> userManager,
        RoleManager<Role> roleManager, ILogger<IdentityValidationService> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<ValidationResult> ValidateUserAsync(ApplicationUser user)
    {
        var result = new ValidationResult
        {
            ValidationType = "User",
            Data = user
        };

        try
        {
            // Validate email
            if (string.IsNullOrEmpty(user.Email))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "USER_EMAIL_REQUIRED",
                    Message = "Email is required",
                    Field = "Email",
                    Category = "Required",
                    Recommendation = "Provide a valid email address"
                });
            }
            else if (!IsValidEmail(user.Email))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "USER_EMAIL_INVALID",
                    Message = "Email format is invalid",
                    Field = "Email",
                    Category = "Format",
                    Recommendation = "Provide a valid email address format"
                });
            }

            // Validate username
            if (string.IsNullOrEmpty(user.UserName))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "USER_USERNAME_REQUIRED",
                    Message = "Username is required",
                    Field = "UserName",
                    Category = "Required",
                    Recommendation = "Provide a valid username"
                });
            }
            else if (user.UserName.Length < 3)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "USER_USERNAME_TOO_SHORT",
                    Message = "Username must be at least 3 characters long",
                    Field = "UserName",
                    Category = "Length",
                    Recommendation = "Use a username with at least 3 characters"
                });
            }

            // Validate phone number
            if (!string.IsNullOrEmpty(user.PhoneNumber) && !IsValidPhoneNumber(user.PhoneNumber))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "USER_PHONE_INVALID",
                    Message = "Phone number format is invalid",
                    Field = "PhoneNumber",
                    Category = "Format",
                    Recommendation = "Provide a valid phone number format"
                });
            }

            // Validate account status
            if (!user.IsActive)
            {
                result.Warnings.Add(new ValidationWarning
                {
                    Code = "USER_INACTIVE",
                    Message = "User account is inactive",
                    Field = "IsActive",
                    Category = "Status",
                    Recommendation = "Consider activating the user account"
                });
            }

            // Validate lockout status
            if (user.IsLocked)
            {
                result.Warnings.Add(new ValidationWarning
                {
                    Code = "USER_LOCKED",
                    Message = "User account is locked",
                    Field = "IsLocked",
                    Category = "Status",
                    Recommendation = "Review lockout reason and consider unlocking"
                });
            }

            // Validate two-factor authentication
            if (user.TwoFactorEnabled && string.IsNullOrEmpty(user.TotpSecretKey))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "USER_TOTP_MISSING",
                    Message = "TOTP secret key is missing for two-factor authentication",
                    Field = "TotpSecretKey",
                    Category = "Security",
                    Recommendation = "Generate TOTP secret key or disable two-factor authentication"
                });
            }

            result.IsValid = result.Errors.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user: {UserId}", user.Id);
            result.Errors.Add(new ValidationError
            {
                Code = "VALIDATION_ERROR",
                Message = "An error occurred during validation",
                Field = "System",
                Category = "System",
                Recommendation = "Contact system administrator"
            });
        }

        return result;
    }

    public async Task<ValidationResult> ValidateUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ValidationResult
            {
                IsValid = false,
                ValidationType = "User",
                Errors = new List<ValidationError>
                {
                    new ValidationError
                    {
                        Code = "USER_NOT_FOUND",
                        Message = "User not found",
                        Field = "UserId",
                        Category = "Existence",
                        Recommendation = "Provide a valid user ID"
                    }
                }
            };
        }

        return await ValidateUserAsync(user);
    }

    public async Task<ValidationResult> ValidateRoleAsync(Role role)
    {
        var result = new ValidationResult
        {
            ValidationType = "Role",
            Data = role
        };

        try
        {
            // Validate name
            if (string.IsNullOrEmpty(role.Name))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "ROLE_NAME_REQUIRED",
                    Message = "Role name is required",
                    Field = "Name",
                    Category = "Required",
                    Recommendation = "Provide a valid role name"
                });
            }
            else if (role.Name.Length < 2)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "ROLE_NAME_TOO_SHORT",
                    Message = "Role name must be at least 2 characters long",
                    Field = "Name",
                    Category = "Length",
                    Recommendation = "Use a role name with at least 2 characters"
                });
            }

            // Validate description
            if (string.IsNullOrEmpty(role.Description))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "ROLE_DESCRIPTION_REQUIRED",
                    Message = "Role description is required",
                    Field = "Description",
                    Category = "Required",
                    Recommendation = "Provide a valid role description"
                });
            }

            // Validate priority
            if (role.Priority < 0)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "ROLE_PRIORITY_INVALID",
                    Message = "Role priority must be non-negative",
                    Field = "Priority",
                    Category = "Range",
                    Recommendation = "Use a non-negative priority value"
                });
            }

            // Validate system role
            if (role.IsSystemRole && !IsValidSystemRole(role.Name))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "ROLE_SYSTEM_INVALID",
                    Message = "Invalid system role name",
                    Field = "Name",
                    Category = "System",
                    Recommendation = "Use a valid system role name"
                });
            }

            result.IsValid = result.Errors.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating role: {RoleId}", role.Id);
            result.Errors.Add(new ValidationError
            {
                Code = "VALIDATION_ERROR",
                Message = "An error occurred during validation",
                Field = "System",
                Category = "System",
                Recommendation = "Contact system administrator"
            });
        }

        return result;
    }

    public async Task<ValidationResult> ValidateRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            return new ValidationResult
            {
                IsValid = false,
                ValidationType = "Role",
                Errors = new List<ValidationError>
                {
                    new ValidationError
                    {
                        Code = "ROLE_NOT_FOUND",
                        Message = "Role not found",
                        Field = "RoleId",
                        Category = "Existence",
                        Recommendation = "Provide a valid role ID"
                    }
                }
            };
        }

        return await ValidateRoleAsync(role);
    }

    public async Task<ValidationResult> ValidatePermissionAsync(Permission permission)
    {
        var result = new ValidationResult
        {
            ValidationType = "Permission",
            Data = permission
        };

        try
        {
            // Validate name
            if (string.IsNullOrEmpty(permission.Name))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "PERMISSION_NAME_REQUIRED",
                    Message = "Permission name is required",
                    Field = "Name",
                    Category = "Required",
                    Recommendation = "Provide a valid permission name"
                });
            }

            // Validate resource
            if (!Enum.IsDefined(typeof(Resource), permission.Resource))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "PERMISSION_RESOURCE_INVALID",
                    Message = "Invalid resource type",
                    Field = "Resource",
                    Category = "Enum",
                    Recommendation = "Use a valid resource type"
                });
            }

            // Validate action
            if (!Enum.IsDefined(typeof(ActionEnum), permission.Action))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "PERMISSION_ACTION_INVALID",
                    Message = "Invalid action type",
                    Field = "Action",
                    Category = "Enum",
                    Recommendation = "Use a valid action type"
                });
            }

            // Validate description
            if (string.IsNullOrEmpty(permission.Description))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "PERMISSION_DESCRIPTION_REQUIRED",
                    Message = "Permission description is required",
                    Field = "Description",
                    Category = "Required",
                    Recommendation = "Provide a valid permission description"
                });
            }

            // Validate priority
            if (permission.Priority < 0)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "PERMISSION_PRIORITY_INVALID",
                    Message = "Permission priority must be non-negative",
                    Field = "Priority",
                    Category = "Range",
                    Recommendation = "Use a non-negative priority value"
                });
            }

            result.IsValid = result.Errors.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating permission: {PermissionId}", permission.Id);
            result.Errors.Add(new ValidationError
            {
                Code = "VALIDATION_ERROR",
                Message = "An error occurred during validation",
                Field = "System",
                Category = "System",
                Recommendation = "Contact system administrator"
            });
        }

        return result;
    }

    public async Task<ValidationResult> ValidatePermissionAsync(string permissionId)
    {
        var permission = await _context.Permissions.FindAsync(permissionId);
        if (permission == null)
        {
            return new ValidationResult
            {
                IsValid = false,
                ValidationType = "Permission",
                Errors = new List<ValidationError>
                {
                    new ValidationError
                    {
                        Code = "PERMISSION_NOT_FOUND",
                        Message = "Permission not found",
                        Field = "PermissionId",
                        Category = "Existence",
                        Recommendation = "Provide a valid permission ID"
                    }
                }
            };
        }

        return await ValidatePermissionAsync(permission);
    }

    public async Task<ValidationResult> ValidateUserRoleAsync(string userId, string roleId)
    {
        var result = new ValidationResult
        {
            ValidationType = "UserRole",
            Data = new { UserId = userId, RoleId = roleId }
        };

        try
        {
            // Validate user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "USER_NOT_FOUND",
                    Message = "User not found",
                    Field = "UserId",
                    Category = "Existence",
                    Recommendation = "Provide a valid user ID"
                });
            }

            // Validate role exists
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "ROLE_NOT_FOUND",
                    Message = "Role not found",
                    Field = "RoleId",
                    Category = "Existence",
                    Recommendation = "Provide a valid role ID"
                });
            }

            // Validate user is active
            if (user != null && !user.IsActive)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "USER_INACTIVE",
                    Message = "Cannot assign role to inactive user",
                    Field = "UserId",
                    Category = "Status",
                    Recommendation = "Activate the user before assigning roles"
                });
            }

            // Validate role is active
            if (role != null && !role.IsActive)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "ROLE_INACTIVE",
                    Message = "Cannot assign inactive role to user",
                    Field = "RoleId",
                    Category = "Status",
                    Recommendation = "Activate the role before assigning to users"
                });
            }

            // Check if user already has this role
            if (user != null && role != null)
            {
                var hasRole = await _userManager.IsInRoleAsync(user, role.Name);
                if (hasRole)
                {
                    result.Warnings.Add(new ValidationWarning
                    {
                        Code = "USER_ROLE_EXISTS",
                        Message = "User already has this role",
                        Field = "UserRole",
                        Category = "Duplicate",
                        Recommendation = "User already has this role assigned"
                    });
                }
            }

            result.IsValid = result.Errors.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user role: {UserId}, {RoleId}", userId, roleId);
            result.Errors.Add(new ValidationError
            {
                Code = "VALIDATION_ERROR",
                Message = "An error occurred during validation",
                Field = "System",
                Category = "System",
                Recommendation = "Contact system administrator"
            });
        }

        return result;
    }

    public async Task<ValidationResult> ValidateRolePermissionAsync(string roleId, string permissionId)
    {
        var result = new ValidationResult
        {
            ValidationType = "RolePermission",
            Data = new { RoleId = roleId, PermissionId = permissionId }
        };

        try
        {
            // Validate role exists
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "ROLE_NOT_FOUND",
                    Message = "Role not found",
                    Field = "RoleId",
                    Category = "Existence",
                    Recommendation = "Provide a valid role ID"
                });
            }

            // Validate permission exists
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "PERMISSION_NOT_FOUND",
                    Message = "Permission not found",
                    Field = "PermissionId",
                    Category = "Existence",
                    Recommendation = "Provide a valid permission ID"
                });
            }

            // Validate role is active
            if (role != null && !role.IsActive)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "ROLE_INACTIVE",
                    Message = "Cannot assign permission to inactive role",
                    Field = "RoleId",
                    Category = "Status",
                    Recommendation = "Activate the role before assigning permissions"
                });
            }

            // Validate permission is active
            if (permission != null && !permission.IsActive)
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "PERMISSION_INACTIVE",
                    Message = "Cannot assign inactive permission to role",
                    Field = "PermissionId",
                    Category = "Status",
                    Recommendation = "Activate the permission before assigning to roles"
                });
            }

            // Check if role already has this permission
            if (role != null && permission != null)
            {
                var hasPermission = await _context.RolePermissions
                    .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && rp.IsActive);

                if (hasPermission)
                {
                    result.Warnings.Add(new ValidationWarning
                    {
                        Code = "ROLE_PERMISSION_EXISTS",
                        Message = "Role already has this permission",
                        Field = "RolePermission",
                        Category = "Duplicate",
                        Recommendation = "Role already has this permission assigned"
                    });
                }
            }

            result.IsValid = result.Errors.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating role permission: {RoleId}, {PermissionId}", roleId, permissionId);
            result.Errors.Add(new ValidationError
            {
                Code = "VALIDATION_ERROR",
                Message = "An error occurred during validation",
                Field = "System",
                Category = "System",
                Recommendation = "Contact system administrator"
            });
        }

        return result;
    }

    public async Task<ValidationResult> ValidatePasswordAsync(string password)
    {
        var result = new ValidationResult
        {
            ValidationType = "Password",
            Data = new { Password = "***" }
        };

        try
        {
            if (string.IsNullOrEmpty(password))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "PASSWORD_REQUIRED",
                    Message = "Password is required",
                    Field = "Password",
                    Category = "Required",
                    Recommendation = "Provide a password"
                });
            }
            else
            {
                if (password.Length < 8)
                {
                    result.Errors.Add(new ValidationError
                    {
                        Code = "PASSWORD_TOO_SHORT",
                        Message = "Password must be at least 8 characters long",
                        Field = "Password",
                        Category = "Length",
                        Recommendation = "Use a password with at least 8 characters"
                    });
                }

                if (!password.Any(char.IsUpper))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Code = "PASSWORD_NO_UPPERCASE",
                        Message = "Password must contain at least one uppercase letter",
                        Field = "Password",
                        Category = "Complexity",
                        Recommendation = "Include at least one uppercase letter"
                    });
                }

                if (!password.Any(char.IsLower))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Code = "PASSWORD_NO_LOWERCASE",
                        Message = "Password must contain at least one lowercase letter",
                        Field = "Password",
                        Category = "Complexity",
                        Recommendation = "Include at least one lowercase letter"
                    });
                }

                if (!password.Any(char.IsDigit))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Code = "PASSWORD_NO_DIGIT",
                        Message = "Password must contain at least one digit",
                        Field = "Password",
                        Category = "Complexity",
                        Recommendation = "Include at least one digit"
                    });
                }

                if (!password.Any(c => !char.IsLetterOrDigit(c)))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Code = "PASSWORD_NO_SPECIAL",
                        Message = "Password must contain at least one special character",
                        Field = "Password",
                        Category = "Complexity",
                        Recommendation = "Include at least one special character"
                    });
                }
            }

            result.IsValid = result.Errors.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating password");
            result.Errors.Add(new ValidationError
            {
                Code = "VALIDATION_ERROR",
                Message = "An error occurred during validation",
                Field = "System",
                Category = "System",
                Recommendation = "Contact system administrator"
            });
        }

        return result;
    }

    public async Task<ValidationResult> ValidateEmailAsync(string email)
    {
        var result = new ValidationResult
        {
            ValidationType = "Email",
            Data = new { Email = email }
        };

        try
        {
            if (string.IsNullOrEmpty(email))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "EMAIL_REQUIRED",
                    Message = "Email is required",
                    Field = "Email",
                    Category = "Required",
                    Recommendation = "Provide an email address"
                });
            }
            else if (!IsValidEmail(email))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "EMAIL_INVALID",
                    Message = "Email format is invalid",
                    Field = "Email",
                    Category = "Format",
                    Recommendation = "Provide a valid email address format"
                });
            }

            result.IsValid = result.Errors.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating email: {Email}", email);
            result.Errors.Add(new ValidationError
            {
                Code = "VALIDATION_ERROR",
                Message = "An error occurred during validation",
                Field = "System",
                Category = "System",
                Recommendation = "Contact system administrator"
            });
        }

        return result;
    }

    public async Task<ValidationResult> ValidateUserNameAsync(string userName)
    {
        var result = new ValidationResult
        {
            ValidationType = "UserName",
            Data = new { UserName = userName }
        };

        try
        {
            if (string.IsNullOrEmpty(userName))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "USERNAME_REQUIRED",
                    Message = "Username is required",
                    Field = "UserName",
                    Category = "Required",
                    Recommendation = "Provide a username"
                });
            }
            else
            {
                if (userName.Length < 3)
                {
                    result.Errors.Add(new ValidationError
                    {
                        Code = "USERNAME_TOO_SHORT",
                        Message = "Username must be at least 3 characters long",
                        Field = "UserName",
                        Category = "Length",
                        Recommendation = "Use a username with at least 3 characters"
                    });
                }

                if (userName.Length > 50)
                {
                    result.Errors.Add(new ValidationError
                    {
                        Code = "USERNAME_TOO_LONG",
                        Message = "Username must be at most 50 characters long",
                        Field = "UserName",
                        Category = "Length",
                        Recommendation = "Use a username with at most 50 characters"
                    });
                }

                if (!userName.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.'))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Code = "USERNAME_INVALID_CHARS",
                        Message = "Username contains invalid characters",
                        Field = "UserName",
                        Category = "Format",
                        Recommendation = "Use only letters, digits, underscores, hyphens, and dots"
                    });
                }
            }

            result.IsValid = result.Errors.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating username: {UserName}", userName);
            result.Errors.Add(new ValidationError
            {
                Code = "VALIDATION_ERROR",
                Message = "An error occurred during validation",
                Field = "System",
                Category = "System",
                Recommendation = "Contact system administrator"
            });
        }

        return result;
    }

    public async Task<ValidationResult> ValidatePhoneNumberAsync(string phoneNumber)
    {
        var result = new ValidationResult
        {
            ValidationType = "PhoneNumber",
            Data = new { PhoneNumber = phoneNumber }
        };

        try
        {
            if (!string.IsNullOrEmpty(phoneNumber) && !IsValidPhoneNumber(phoneNumber))
            {
                result.Errors.Add(new ValidationError
                {
                    Code = "PHONE_INVALID",
                    Message = "Phone number format is invalid",
                    Field = "PhoneNumber",
                    Category = "Format",
                    Recommendation = "Provide a valid phone number format"
                });
            }

            result.IsValid = result.Errors.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating phone number: {PhoneNumber}", phoneNumber);
            result.Errors.Add(new ValidationError
            {
                Code = "VALIDATION_ERROR",
                Message = "An error occurred during validation",
                Field = "System",
                Category = "System",
                Recommendation = "Contact system administrator"
            });
        }

        return result;
    }

    // Placeholder implementations for other validation methods
    public async Task<ValidationResult> ValidateTotpTokenAsync(string secret, string token)
    {
        return new ValidationResult { IsValid = true, ValidationType = "TotpToken" };
    }

    public async Task<ValidationResult> ValidateRefreshTokenAsync(string refreshToken)
    {
        return new ValidationResult { IsValid = true, ValidationType = "RefreshToken" };
    }

    public async Task<ValidationResult> ValidateAccessTokenAsync(string accessToken)
    {
        return new ValidationResult { IsValid = true, ValidationType = "AccessToken" };
    }

    public async Task<ValidationResult> ValidateUserClaimAsync(string userId, string claimType, string claimValue)
    {
        return new ValidationResult { IsValid = true, ValidationType = "UserClaim" };
    }

    public async Task<ValidationResult> ValidateUserLoginAsync(string userId, string loginProvider, string providerKey)
    {
        return new ValidationResult { IsValid = true, ValidationType = "UserLogin" };
    }

    public async Task<ValidationResult> ValidateUserTokenAsync(string userId, string loginProvider, string name, string value)
    {
        return new ValidationResult { IsValid = true, ValidationType = "UserToken" };
    }

    public async Task<ValidationResult> ValidateAuditLogAsync(AuditLog auditLog)
    {
        return new ValidationResult { IsValid = true, ValidationType = "AuditLog" };
    }

    public async Task<ValidationResult> ValidateBusinessRuleAsync(string ruleName, object data)
    {
        return new ValidationResult { IsValid = true, ValidationType = "BusinessRule" };
    }

    public async Task<ValidationResult> ValidateComplianceAsync(string requirement, object data)
    {
        return new ValidationResult { IsValid = true, ValidationType = "Compliance" };
    }

    public async Task<ValidationResult> ValidateSecurityAsync(string userId, string action, object data)
    {
        return new ValidationResult { IsValid = true, ValidationType = "Security" };
    }

    public async Task<ValidationResult> ValidatePerformanceAsync(string operation, object data)
    {
        return new ValidationResult { IsValid = true, ValidationType = "Performance" };
    }

    public async Task<ValidationResult> ValidateDataIntegrityAsync()
    {
        return new ValidationResult { IsValid = true, ValidationType = "DataIntegrity" };
    }

    public async Task<ValidationResult> ValidateSystemHealthAsync()
    {
        return new ValidationResult { IsValid = true, ValidationType = "SystemHealth" };
    }

    public async Task<ValidationResult> ValidateConfigurationAsync()
    {
        return new ValidationResult { IsValid = true, ValidationType = "Configuration" };
    }

    public async Task<ValidationResult> ValidatePermissionsAsync(string userId, string resource, string action)
    {
        return new ValidationResult { IsValid = true, ValidationType = "Permissions" };
    }

    public async Task<ValidationResult> ValidateRoleHierarchyAsync(string userId, string roleId)
    {
        return new ValidationResult { IsValid = true, ValidationType = "RoleHierarchy" };
    }

    public async Task<ValidationResult> ValidatePermissionHierarchyAsync(string roleId, string permissionId)
    {
        return new ValidationResult { IsValid = true, ValidationType = "PermissionHierarchy" };
    }

    // Helper methods
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        // Simple phone number validation
        return phoneNumber.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == '(' || c == ')' || c == ' ');
    }

    private bool IsValidSystemRole(string roleName)
    {
        var validSystemRoles = new[] { "SuperAdmin", "SystemAdmin" };
        return validSystemRoles.Contains(roleName);
    }
}