namespace MyShop.Identity.Constants;

/// <summary>
/// Constants for token types
/// </summary>
public static class TokenTypeConstants
{
    /// <summary>
    /// Bearer token type
    /// </summary>
    public const string Bearer = "Bearer";

    /// <summary>
    /// JWT token type
    /// </summary>
    public const string JWT = "JWT";

    /// <summary>
    /// OAuth token type
    /// </summary>
    public const string OAuth = "OAuth";

    /// <summary>
    /// API key token type
    /// </summary>
    public const string ApiKey = "ApiKey";

    /// <summary>
    /// Session token type
    /// </summary>
    public const string Session = "Session";

    /// <summary>
    /// Cookie token type
    /// </summary>
    public const string Cookie = "Cookie";

    /// <summary>
    /// Custom token type
    /// </summary>
    public const string Custom = "Custom";
}

/// <summary>
/// Constants for token purposes
/// </summary>
public static class TokenPurposeConstants
{
    /// <summary>
    /// Access token purpose
    /// </summary>
    public const string Access = "Access";

    /// <summary>
    /// Refresh token purpose
    /// </summary>
    public const string Refresh = "Refresh";

    /// <summary>
    /// Authentication token purpose
    /// </summary>
    public const string Authentication = "Authentication";

    /// <summary>
    /// Authorization token purpose
    /// </summary>
    public const string Authorization = "Authorization";

    /// <summary>
    /// API access token purpose
    /// </summary>
    public const string ApiAccess = "ApiAccess";

    /// <summary>
    /// Session token purpose
    /// </summary>
    public const string Session = "Session";

    /// <summary>
    /// Password reset token purpose
    /// </summary>
    public const string PasswordReset = "PasswordReset";

    /// <summary>
    /// Email verification token purpose
    /// </summary>
    public const string EmailVerification = "EmailVerification";

    /// <summary>
    /// Two-factor authentication token purpose
    /// </summary>
    public const string TwoFactorAuth = "TwoFactorAuth";

    /// <summary>
    /// Device registration token purpose
    /// </summary>
    public const string DeviceRegistration = "DeviceRegistration";

    /// <summary>
    /// Custom token purpose
    /// </summary>
    public const string Custom = "Custom";
}

/// <summary>
/// Constants for token names
/// </summary>
public static class TokenNameConstants
{
    /// <summary>
    /// Access token name
    /// </summary>
    public const string AccessToken = "AccessToken";

    /// <summary>
    /// Refresh token name
    /// </summary>
    public const string RefreshToken = "RefreshToken";

    /// <summary>
    /// Authentication token name
    /// </summary>
    public const string AuthToken = "AuthToken";

    /// <summary>
    /// API token name
    /// </summary>
    public const string ApiToken = "ApiToken";

    /// <summary>
    /// Session token name
    /// </summary>
    public const string SessionToken = "SessionToken";

    /// <summary>
    /// Password reset token name
    /// </summary>
    public const string PasswordResetToken = "PasswordResetToken";

    /// <summary>
    /// Email verification token name
    /// </summary>
    public const string EmailVerificationToken = "EmailVerificationToken";

    /// <summary>
    /// Two-factor authentication token name
    /// </summary>
    public const string TwoFactorAuthToken = "TwoFactorAuthToken";

    /// <summary>
    /// Device registration token name
    /// </summary>
    public const string DeviceRegistrationToken = "DeviceRegistrationToken";

    /// <summary>
    /// Custom token name
    /// </summary>
    public const string CustomToken = "CustomToken";
}

/// <summary>
/// Constants for token expiration periods
/// </summary>
public static class TokenExpirationConstants
{
    /// <summary>
    /// Short-term expiration (15 minutes)
    /// </summary>
    public static readonly TimeSpan ShortTerm = TimeSpan.FromMinutes(15);

    /// <summary>
    /// Medium-term expiration (1 hour)
    /// </summary>
    public static readonly TimeSpan MediumTerm = TimeSpan.FromHours(1);

    /// <summary>
    /// Long-term expiration (24 hours)
    /// </summary>
    public static readonly TimeSpan LongTerm = TimeSpan.FromHours(24);

    /// <summary>
    /// Refresh token expiration (30 days)
    /// </summary>
    public static readonly TimeSpan RefreshToken = TimeSpan.FromDays(30);

    /// <summary>
    /// Session token expiration (7 days)
    /// </summary>
    public static readonly TimeSpan SessionToken = TimeSpan.FromDays(7);

    /// <summary>
    /// Password reset token expiration (1 hour)
    /// </summary>
    public static readonly TimeSpan PasswordReset = TimeSpan.FromHours(1);

    /// <summary>
    /// Email verification token expiration (24 hours)
    /// </summary>
    public static readonly TimeSpan EmailVerification = TimeSpan.FromHours(24);

    /// <summary>
    /// Two-factor authentication token expiration (5 minutes)
    /// </summary>
    public static readonly TimeSpan TwoFactorAuth = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Device registration token expiration (7 days)
    /// </summary>
    public static readonly TimeSpan DeviceRegistration = TimeSpan.FromDays(7);

    /// <summary>
    /// API token expiration (90 days)
    /// </summary>
    public static readonly TimeSpan ApiToken = TimeSpan.FromDays(90);
}

/// <summary>
/// Constants for token revocation reasons
/// </summary>
public static class TokenRevocationReasonConstants
{
    /// <summary>
    /// User logout
    /// </summary>
    public const string UserLogout = "User logout";

    /// <summary>
    /// Security breach
    /// </summary>
    public const string SecurityBreach = "Security breach detected";

    /// <summary>
    /// Token expiration
    /// </summary>
    public const string TokenExpiration = "Token expiration";

    /// <summary>
    /// Password change
    /// </summary>
    public const string PasswordChange = "Password changed";

    /// <summary>
    /// Account suspension
    /// </summary>
    public const string AccountSuspension = "Account suspended";

    /// <summary>
    /// Account deletion
    /// </summary>
    public const string AccountDeletion = "Account deleted";

    /// <summary>
    /// Device change
    /// </summary>
    public const string DeviceChange = "Device changed";

    /// <summary>
    /// Location change
    /// </summary>
    public const string LocationChange = "Location changed";

    /// <summary>
    /// Admin revocation
    /// </summary>
    public const string AdminRevocation = "Revoked by administrator";

    /// <summary>
    /// System maintenance
    /// </summary>
    public const string SystemMaintenance = "System maintenance";

    /// <summary>
    /// Token rotation
    /// </summary>
    public const string TokenRotation = "Token rotated";

    /// <summary>
    /// Suspicious activity
    /// </summary>
    public const string SuspiciousActivity = "Suspicious activity detected";

    /// <summary>
    /// Policy violation
    /// </summary>
    public const string PolicyViolation = "Policy violation";

    /// <summary>
    /// Custom reason
    /// </summary>
    public const string Custom = "Custom reason";
}

/// <summary>
/// Token helper methods
/// </summary>
public static class TokenHelper
{
    /// <summary>
    /// Gets all token types
    /// </summary>
    public static IEnumerable<string> GetTokenTypes()
    {
        return new[]
        {
            TokenTypeConstants.Bearer,
            TokenTypeConstants.JWT,
            TokenTypeConstants.OAuth,
            TokenTypeConstants.ApiKey,
            TokenTypeConstants.Session,
            TokenTypeConstants.Cookie,
            TokenTypeConstants.Custom
        };
    }

    /// <summary>
    /// Gets all token purposes
    /// </summary>
    public static IEnumerable<string> GetTokenPurposes()
    {
        return new[]
        {
            TokenPurposeConstants.Access,
            TokenPurposeConstants.Refresh,
            TokenPurposeConstants.Authentication,
            TokenPurposeConstants.Authorization,
            TokenPurposeConstants.ApiAccess,
            TokenPurposeConstants.Session,
            TokenPurposeConstants.PasswordReset,
            TokenPurposeConstants.EmailVerification,
            TokenPurposeConstants.TwoFactorAuth,
            TokenPurposeConstants.DeviceRegistration,
            TokenPurposeConstants.Custom
        };
    }

    /// <summary>
    /// Gets all token names
    /// </summary>
    public static IEnumerable<string> GetTokenNames()
    {
        return new[]
        {
            TokenNameConstants.AccessToken,
            TokenNameConstants.RefreshToken,
            TokenNameConstants.AuthToken,
            TokenNameConstants.ApiToken,
            TokenNameConstants.SessionToken,
            TokenNameConstants.PasswordResetToken,
            TokenNameConstants.EmailVerificationToken,
            TokenNameConstants.TwoFactorAuthToken,
            TokenNameConstants.DeviceRegistrationToken,
            TokenNameConstants.CustomToken
        };
    }

    /// <summary>
    /// Gets all revocation reasons
    /// </summary>
    public static IEnumerable<string> GetRevocationReasons()
    {
        return new[]
        {
            TokenRevocationReasonConstants.UserLogout,
            TokenRevocationReasonConstants.SecurityBreach,
            TokenRevocationReasonConstants.TokenExpiration,
            TokenRevocationReasonConstants.PasswordChange,
            TokenRevocationReasonConstants.AccountSuspension,
            TokenRevocationReasonConstants.AccountDeletion,
            TokenRevocationReasonConstants.DeviceChange,
            TokenRevocationReasonConstants.LocationChange,
            TokenRevocationReasonConstants.AdminRevocation,
            TokenRevocationReasonConstants.SystemMaintenance,
            TokenRevocationReasonConstants.TokenRotation,
            TokenRevocationReasonConstants.SuspiciousActivity,
            TokenRevocationReasonConstants.PolicyViolation,
            TokenRevocationReasonConstants.Custom
        };
    }

    /// <summary>
    /// Gets token types grouped by category
    /// </summary>
    public static Dictionary<string, IEnumerable<string>> GetTokenTypesByCategory()
    {
        return new Dictionary<string, IEnumerable<string>>
        {
            ["Standard"] = new[] { TokenTypeConstants.Bearer, TokenTypeConstants.JWT, TokenTypeConstants.OAuth },
            ["Session"] = new[] { TokenTypeConstants.Session, TokenTypeConstants.Cookie },
            ["API"] = new[] { TokenTypeConstants.ApiKey },
            ["Custom"] = new[] { TokenTypeConstants.Custom }
        };
    }

    /// <summary>
    /// Gets token purposes grouped by category
    /// </summary>
    public static Dictionary<string, IEnumerable<string>> GetTokenPurposesByCategory()
    {
        return new Dictionary<string, IEnumerable<string>>
        {
            ["Authentication"] = new[] { TokenPurposeConstants.Access, TokenPurposeConstants.Refresh, TokenPurposeConstants.Authentication },
            ["Authorization"] = new[] { TokenPurposeConstants.Authorization, TokenPurposeConstants.ApiAccess },
            ["Session"] = new[] { TokenPurposeConstants.Session },
            ["Security"] = new[] { TokenPurposeConstants.PasswordReset, TokenPurposeConstants.EmailVerification, TokenPurposeConstants.TwoFactorAuth },
            ["Device"] = new[] { TokenPurposeConstants.DeviceRegistration },
            ["Custom"] = new[] { TokenPurposeConstants.Custom }
        };
    }

    /// <summary>
    /// Gets the default expiration period for a token purpose
    /// </summary>
    public static TimeSpan GetDefaultExpirationForPurpose(string purpose)
    {
        return purpose switch
        {
            TokenPurposeConstants.Access => TokenExpirationConstants.MediumTerm,
            TokenPurposeConstants.Refresh => TokenExpirationConstants.RefreshToken,
            TokenPurposeConstants.Authentication => TokenExpirationConstants.MediumTerm,
            TokenPurposeConstants.Authorization => TokenExpirationConstants.MediumTerm,
            TokenPurposeConstants.ApiAccess => TokenExpirationConstants.ApiToken,
            TokenPurposeConstants.Session => TokenExpirationConstants.SessionToken,
            TokenPurposeConstants.PasswordReset => TokenExpirationConstants.PasswordReset,
            TokenPurposeConstants.EmailVerification => TokenExpirationConstants.EmailVerification,
            TokenPurposeConstants.TwoFactorAuth => TokenExpirationConstants.TwoFactorAuth,
            TokenPurposeConstants.DeviceRegistration => TokenExpirationConstants.DeviceRegistration,
            _ => TokenExpirationConstants.MediumTerm
        };
    }

    /// <summary>
    /// Gets the default expiration period for a token type
    /// </summary>
    public static TimeSpan GetDefaultExpirationForType(string type)
    {
        return type switch
        {
            TokenTypeConstants.Bearer => TokenExpirationConstants.MediumTerm,
            TokenTypeConstants.JWT => TokenExpirationConstants.MediumTerm,
            TokenTypeConstants.OAuth => TokenExpirationConstants.RefreshToken,
            TokenTypeConstants.ApiKey => TokenExpirationConstants.ApiToken,
            TokenTypeConstants.Session => TokenExpirationConstants.SessionToken,
            TokenTypeConstants.Cookie => TokenExpirationConstants.SessionToken,
            _ => TokenExpirationConstants.MediumTerm
        };
    }

    /// <summary>
    /// Checks if a token type is secure
    /// </summary>
    public static bool IsSecureTokenType(string type)
    {
        return type switch
        {
            TokenTypeConstants.Bearer or TokenTypeConstants.JWT or TokenTypeConstants.OAuth => true,
            _ => false
        };
    }

    /// <summary>
    /// Checks if a token purpose is sensitive
    /// </summary>
    public static bool IsSensitiveTokenPurpose(string purpose)
    {
        return purpose switch
        {
            TokenPurposeConstants.Access or TokenPurposeConstants.Refresh or TokenPurposeConstants.Authorization => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets the security level for a token purpose
    /// </summary>
    public static int GetSecurityLevelForPurpose(string purpose)
    {
        return purpose switch
        {
            TokenPurposeConstants.Access or TokenPurposeConstants.Refresh => 3, // High security
            TokenPurposeConstants.Authorization or TokenPurposeConstants.ApiAccess => 2, // Medium security
            TokenPurposeConstants.Authentication or TokenPurposeConstants.Session => 2, // Medium security
            TokenPurposeConstants.PasswordReset or TokenPurposeConstants.EmailVerification => 3, // High security
            TokenPurposeConstants.TwoFactorAuth => 3, // High security
            TokenPurposeConstants.DeviceRegistration => 2, // Medium security
            _ => 1 // Low security
        };
    }

    /// <summary>
    /// Gets the display name for a token type
    /// </summary>
    public static string GetTokenTypeDisplayName(string type)
    {
        return type switch
        {
            TokenTypeConstants.Bearer => "Bearer Token",
            TokenTypeConstants.JWT => "JWT Token",
            TokenTypeConstants.OAuth => "OAuth Token",
            TokenTypeConstants.ApiKey => "API Key",
            TokenTypeConstants.Session => "Session Token",
            TokenTypeConstants.Cookie => "Cookie Token",
            _ => type
        };
    }

    /// <summary>
    /// Gets the display name for a token purpose
    /// </summary>
    public static string GetTokenPurposeDisplayName(string purpose)
    {
        return purpose switch
        {
            TokenPurposeConstants.Access => "Access Token",
            TokenPurposeConstants.Refresh => "Refresh Token",
            TokenPurposeConstants.Authentication => "Authentication Token",
            TokenPurposeConstants.Authorization => "Authorization Token",
            TokenPurposeConstants.ApiAccess => "API Access Token",
            TokenPurposeConstants.Session => "Session Token",
            TokenPurposeConstants.PasswordReset => "Password Reset Token",
            TokenPurposeConstants.EmailVerification => "Email Verification Token",
            TokenPurposeConstants.TwoFactorAuth => "Two-Factor Authentication Token",
            TokenPurposeConstants.DeviceRegistration => "Device Registration Token",
            _ => purpose
        };
    }

    /// <summary>
    /// Gets the color for a token type (for UI)
    /// </summary>
    public static string GetTokenTypeColor(string type)
    {
        return type switch
        {
            TokenTypeConstants.Bearer => "#007bff", // Blue
            TokenTypeConstants.JWT => "#28a745", // Green
            TokenTypeConstants.OAuth => "#ffc107", // Yellow
            TokenTypeConstants.ApiKey => "#dc3545", // Red
            TokenTypeConstants.Session => "#6c757d", // Gray
            TokenTypeConstants.Cookie => "#17a2b8", // Cyan
            _ => "#6c757d" // Default gray
        };
    }

    /// <summary>
    /// Gets the color for a token purpose (for UI)
    /// </summary>
    public static string GetTokenPurposeColor(string purpose)
    {
        return purpose switch
        {
            TokenPurposeConstants.Access => "#007bff", // Blue
            TokenPurposeConstants.Refresh => "#28a745", // Green
            TokenPurposeConstants.Authentication => "#17a2b8", // Cyan
            TokenPurposeConstants.Authorization => "#ffc107", // Yellow
            TokenPurposeConstants.ApiAccess => "#dc3545", // Red
            TokenPurposeConstants.Session => "#6c757d", // Gray
            TokenPurposeConstants.PasswordReset => "#fd7e14", // Orange
            TokenPurposeConstants.EmailVerification => "#20c997", // Teal
            TokenPurposeConstants.TwoFactorAuth => "#e83e8c", // Pink
            TokenPurposeConstants.DeviceRegistration => "#6f42c1", // Purple
            _ => "#6c757d" // Default gray
        };
    }

    /// <summary>
    /// Validates if a token type is valid
    /// </summary>
    public static bool IsValidTokenType(string type)
    {
        return GetTokenTypes().Contains(type);
    }

    /// <summary>
    /// Validates if a token purpose is valid
    /// </summary>
    public static bool IsValidTokenPurpose(string purpose)
    {
        return GetTokenPurposes().Contains(purpose);
    }

    /// <summary>
    /// Validates if a token name is valid
    /// </summary>
    public static bool IsValidTokenName(string name)
    {
        return GetTokenNames().Contains(name);
    }

    /// <summary>
    /// Validates if a revocation reason is valid
    /// </summary>
    public static bool IsValidRevocationReason(string reason)
    {
        return GetRevocationReasons().Contains(reason);
    }
}