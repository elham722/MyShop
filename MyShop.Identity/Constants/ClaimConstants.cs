namespace MyShop.Identity.Constants;

/// <summary>
/// Constants for commonly used claim types
/// </summary>
public static class ClaimConstants
{
    /// <summary>
    /// System claims (managed by the system)
    /// </summary>
    public static class System
    {
        public const string UserId = "user_id";
        public const string UserName = "user_name";
        public const string Email = "email";
        public const string Role = "role";
        public const string Permission = "permission";
        public const string TenantId = "tenant_id";
        public const string SessionId = "session_id";
    }

    /// <summary>
    /// Business claims (managed by business logic)
    /// </summary>
    public static class Business
    {
        public const string Department = "department";
        public const string Region = "region";
        public const string AccessLevel = "access_level";
        public const string CustomerType = "customer_type";
        public const string SubscriptionLevel = "subscription_level";
        public const string CompanyId = "company_id";
        public const string BranchId = "branch_id";
        public const string ManagerId = "manager_id";
    }

    /// <summary>
    /// Temporary claims (expire after a certain time)
    /// </summary>
    public static class Temporary
    {
        public const string TemporaryAccess = "temporary_access";
        public const string SpecialPermission = "special_permission";
        public const string EmergencyAccess = "emergency_access";
        public const string MaintenanceAccess = "maintenance_access";
        public const string BetaAccess = "beta_access";
    }

    /// <summary>
    /// Security claims (related to security and compliance)
    /// </summary>
    public static class Security
    {
        public const string SecurityClearance = "security_clearance";
        public const string ComplianceLevel = "compliance_level";
        public const string DataClassification = "data_classification";
        public const string AuditLevel = "audit_level";
        public const string EncryptionLevel = "encryption_level";
    }

    /// <summary>
    /// Custom claims (application-specific)
    /// </summary>
    public static class Custom
    {
        public const string CustomField1 = "custom_field_1";
        public const string CustomField2 = "custom_field_2";
        public const string CustomField3 = "custom_field_3";
        public const string CustomField4 = "custom_field_4";
        public const string CustomField5 = "custom_field_5";
    }
}

/// <summary>
/// Claim helper methods
/// </summary>
public static class ClaimHelper
{
    /// <summary>
    /// Gets all system claims
    /// </summary>
    public static IEnumerable<string> GetSystemClaims()
    {
        return new[]
        {
            ClaimConstants.System.UserId,
            ClaimConstants.System.UserName,
            ClaimConstants.System.Email,
            ClaimConstants.System.Role,
            ClaimConstants.System.Permission,
            ClaimConstants.System.TenantId,
            ClaimConstants.System.SessionId
        };
    }

    /// <summary>
    /// Gets all business claims
    /// </summary>
    public static IEnumerable<string> GetBusinessClaims()
    {
        return new[]
        {
            ClaimConstants.Business.Department,
            ClaimConstants.Business.Region,
            ClaimConstants.Business.AccessLevel,
            ClaimConstants.Business.CustomerType,
            ClaimConstants.Business.SubscriptionLevel,
            ClaimConstants.Business.CompanyId,
            ClaimConstants.Business.BranchId,
            ClaimConstants.Business.ManagerId
        };
    }

    /// <summary>
    /// Gets all temporary claims
    /// </summary>
    public static IEnumerable<string> GetTemporaryClaims()
    {
        return new[]
        {
            ClaimConstants.Temporary.TemporaryAccess,
            ClaimConstants.Temporary.SpecialPermission,
            ClaimConstants.Temporary.EmergencyAccess,
            ClaimConstants.Temporary.MaintenanceAccess,
            ClaimConstants.Temporary.BetaAccess
        };
    }

    /// <summary>
    /// Gets all security claims
    /// </summary>
    public static IEnumerable<string> GetSecurityClaims()
    {
        return new[]
        {
            ClaimConstants.Security.SecurityClearance,
            ClaimConstants.Security.ComplianceLevel,
            ClaimConstants.Security.DataClassification,
            ClaimConstants.Security.AuditLevel,
            ClaimConstants.Security.EncryptionLevel
        };
    }

    /// <summary>
    /// Gets all custom claims
    /// </summary>
    public static IEnumerable<string> GetCustomClaims()
    {
        return new[]
        {
            ClaimConstants.Custom.CustomField1,
            ClaimConstants.Custom.CustomField2,
            ClaimConstants.Custom.CustomField3,
            ClaimConstants.Custom.CustomField4,
            ClaimConstants.Custom.CustomField5
        };
    }

    /// <summary>
    /// Gets all claims grouped by category
    /// </summary>
    public static Dictionary<string, IEnumerable<string>> GetAllClaimsByCategory()
    {
        return new Dictionary<string, IEnumerable<string>>
        {
            ["System"] = GetSystemClaims(),
            ["Business"] = GetBusinessClaims(),
            ["Temporary"] = GetTemporaryClaims(),
            ["Security"] = GetSecurityClaims(),
            ["Custom"] = GetCustomClaims()
        };
    }

    /// <summary>
    /// Checks if a claim is a system claim
    /// </summary>
    public static bool IsSystemClaim(string claimType)
    {
        return GetSystemClaims().Contains(claimType);
    }

    /// <summary>
    /// Checks if a claim is a business claim
    /// </summary>
    public static bool IsBusinessClaim(string claimType)
    {
        return GetBusinessClaims().Contains(claimType);
    }

    /// <summary>
    /// Checks if a claim is a temporary claim
    /// </summary>
    public static bool IsTemporaryClaim(string claimType)
    {
        return GetTemporaryClaims().Contains(claimType);
    }

    /// <summary>
    /// Checks if a claim is a security claim
    /// </summary>
    public static bool IsSecurityClaim(string claimType)
    {
        return GetSecurityClaims().Contains(claimType);
    }

    /// <summary>
    /// Checks if a claim is a custom claim
    /// </summary>
    public static bool IsCustomClaim(string claimType)
    {
        return GetCustomClaims().Contains(claimType);
    }

    /// <summary>
    /// Gets the category of a claim
    /// </summary>
    public static string GetClaimCategory(string claimType)
    {
        if (IsSystemClaim(claimType))
            return "System";
        
        if (IsBusinessClaim(claimType))
            return "Business";
        
        if (IsTemporaryClaim(claimType))
            return "Temporary";
        
        if (IsSecurityClaim(claimType))
            return "Security";
        
        if (IsCustomClaim(claimType))
            return "Custom";
        
        return "Unknown";
    }

    /// <summary>
    /// Checks if a claim should be included in JWT tokens
    /// </summary>
    public static bool ShouldIncludeInJwt(string claimType)
    {
        // System claims are typically included in JWT
        // Business claims might be included depending on the use case
        // Temporary claims are usually not included in JWT
        // Security claims are typically not included in JWT
        // Custom claims depend on the specific use case
        
        return IsSystemClaim(claimType) || 
               (IsBusinessClaim(claimType) && !IsTemporaryClaim(claimType));
    }

    /// <summary>
    /// Gets the default expiration period for temporary claims
    /// </summary>
    public static TimeSpan GetDefaultExpirationPeriod(string claimType)
    {
        return claimType switch
        {
            ClaimConstants.Temporary.TemporaryAccess => TimeSpan.FromDays(30),
            ClaimConstants.Temporary.SpecialPermission => TimeSpan.FromDays(7),
            ClaimConstants.Temporary.EmergencyAccess => TimeSpan.FromHours(24),
            ClaimConstants.Temporary.MaintenanceAccess => TimeSpan.FromHours(8),
            ClaimConstants.Temporary.BetaAccess => TimeSpan.FromDays(90),
            _ => TimeSpan.FromDays(30) // Default
        };
    }
}