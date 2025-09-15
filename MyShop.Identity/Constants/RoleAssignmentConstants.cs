namespace MyShop.Identity.Constants;

/// <summary>
/// Constants for role assignment categories
/// </summary>
public static class RoleAssignmentConstants
{
    /// <summary>
    /// Standard role assignment
    /// </summary>
    public const string Standard = "Standard";

    /// <summary>
    /// Temporary role assignment
    /// </summary>
    public const string Temporary = "Temporary";

    /// <summary>
    /// Emergency role assignment
    /// </summary>
    public const string Emergency = "Emergency";

    /// <summary>
    /// Administrative role assignment
    /// </summary>
    public const string Administrative = "Administrative";

    /// <summary>
    /// Business role assignment
    /// </summary>
    public const string Business = "Business";

    /// <summary>
    /// User role assignment
    /// </summary>
    public const string User = "User";

    /// <summary>
    /// Specialized role assignment
    /// </summary>
    public const string Specialized = "Specialized";

    /// <summary>
    /// System role assignment
    /// </summary>
    public const string System = "System";

    /// <summary>
    /// Contract role assignment
    /// </summary>
    public const string Contract = "Contract";

    /// <summary>
    /// Project role assignment
    /// </summary>
    public const string Project = "Project";
}

/// <summary>
/// Constants for role assignment reasons
/// </summary>
public static class RoleAssignmentReasonConstants
{
    /// <summary>
    /// Initial system setup
    /// </summary>
    public const string InitialSetup = "Initial system setup";

    /// <summary>
    /// User promotion
    /// </summary>
    public const string Promotion = "User promotion";

    /// <summary>
    /// User demotion
    /// </summary>
    public const string Demotion = "User demotion";

    /// <summary>
    /// New hire
    /// </summary>
    public const string NewHire = "New employee hire";

    /// <summary>
    /// Temporary coverage
    /// </summary>
    public const string TemporaryCoverage = "Temporary coverage";

    /// <summary>
    /// Emergency access
    /// </summary>
    public const string EmergencyAccess = "Emergency access required";

    /// <summary>
    /// Project assignment
    /// </summary>
    public const string ProjectAssignment = "Project-specific assignment";

    /// <summary>
    /// Contract work
    /// </summary>
    public const string ContractWork = "Contract work assignment";

    /// <summary>
    /// Customer registration
    /// </summary>
    public const string CustomerRegistration = "Customer registration";

    /// <summary>
    /// Role modification
    /// </summary>
    public const string RoleModification = "Role modification";

    /// <summary>
    /// Security requirement
    /// </summary>
    public const string SecurityRequirement = "Security requirement";

    /// <summary>
    /// Compliance requirement
    /// </summary>
    public const string ComplianceRequirement = "Compliance requirement";

    /// <summary>
    /// Audit requirement
    /// </summary>
    public const string AuditRequirement = "Audit requirement";

    /// <summary>
    /// Training assignment
    /// </summary>
    public const string TrainingAssignment = "Training assignment";

    /// <summary>
    /// Role expiration
    /// </summary>
    public const string RoleExpiration = "Role expiration";

    /// <summary>
    /// Role renewal
    /// </summary>
    public const string RoleRenewal = "Role renewal";

    /// <summary>
    /// Role removal
    /// </summary>
    public const string RoleRemoval = "Role removal";
}

/// <summary>
/// Constants for role assignment priorities
/// </summary>
public static class RoleAssignmentPriorityConstants
{
    /// <summary>
    /// Highest priority (1)
    /// </summary>
    public const int Highest = 1;

    /// <summary>
    /// High priority (2)
    /// </summary>
    public const int High = 2;

    /// <summary>
    /// Medium-high priority (3)
    /// </summary>
    public const int MediumHigh = 3;

    /// <summary>
    /// Medium priority (4)
    /// </summary>
    public const int Medium = 4;

    /// <summary>
    /// Medium-low priority (5)
    /// </summary>
    public const int MediumLow = 5;

    /// <summary>
    /// Low priority (6)
    /// </summary>
    public const int Low = 6;

    /// <summary>
    /// Lowest priority (7)
    /// </summary>
    public const int Lowest = 7;

    /// <summary>
    /// Default priority (5)
    /// </summary>
    public const int Default = 5;
}

/// <summary>
/// Role assignment helper methods
/// </summary>
public static class RoleAssignmentHelper
{
    /// <summary>
    /// Gets all assignment categories
    /// </summary>
    public static IEnumerable<string> GetAssignmentCategories()
    {
        return new[]
        {
            RoleAssignmentConstants.Standard,
            RoleAssignmentConstants.Temporary,
            RoleAssignmentConstants.Emergency,
            RoleAssignmentConstants.Administrative,
            RoleAssignmentConstants.Business,
            RoleAssignmentConstants.User,
            RoleAssignmentConstants.Specialized,
            RoleAssignmentConstants.System,
            RoleAssignmentConstants.Contract,
            RoleAssignmentConstants.Project
        };
    }

    /// <summary>
    /// Gets all assignment reasons
    /// </summary>
    public static IEnumerable<string> GetAssignmentReasons()
    {
        return new[]
        {
            RoleAssignmentReasonConstants.InitialSetup,
            RoleAssignmentReasonConstants.Promotion,
            RoleAssignmentReasonConstants.Demotion,
            RoleAssignmentReasonConstants.NewHire,
            RoleAssignmentReasonConstants.TemporaryCoverage,
            RoleAssignmentReasonConstants.EmergencyAccess,
            RoleAssignmentReasonConstants.ProjectAssignment,
            RoleAssignmentReasonConstants.ContractWork,
            RoleAssignmentReasonConstants.CustomerRegistration,
            RoleAssignmentReasonConstants.RoleModification,
            RoleAssignmentReasonConstants.SecurityRequirement,
            RoleAssignmentReasonConstants.ComplianceRequirement,
            RoleAssignmentReasonConstants.AuditRequirement,
            RoleAssignmentReasonConstants.TrainingAssignment,
            RoleAssignmentReasonConstants.RoleExpiration,
            RoleAssignmentReasonConstants.RoleRenewal,
            RoleAssignmentReasonConstants.RoleRemoval
        };
    }

    /// <summary>
    /// Gets all priority levels
    /// </summary>
    public static IEnumerable<int> GetPriorityLevels()
    {
        return new[]
        {
            RoleAssignmentPriorityConstants.Highest,
            RoleAssignmentPriorityConstants.High,
            RoleAssignmentPriorityConstants.MediumHigh,
            RoleAssignmentPriorityConstants.Medium,
            RoleAssignmentPriorityConstants.MediumLow,
            RoleAssignmentPriorityConstants.Low,
            RoleAssignmentPriorityConstants.Lowest
        };
    }

    /// <summary>
    /// Gets assignment categories grouped by type
    /// </summary>
    public static Dictionary<string, IEnumerable<string>> GetAssignmentCategoriesByType()
    {
        return new Dictionary<string, IEnumerable<string>>
        {
            ["Standard"] = new[] { RoleAssignmentConstants.Standard, RoleAssignmentConstants.Administrative, RoleAssignmentConstants.Business, RoleAssignmentConstants.User },
            ["Temporary"] = new[] { RoleAssignmentConstants.Temporary, RoleAssignmentConstants.Emergency, RoleAssignmentConstants.Project, RoleAssignmentConstants.Contract },
            ["Specialized"] = new[] { RoleAssignmentConstants.Specialized, RoleAssignmentConstants.System }
        };
    }

    /// <summary>
    /// Gets assignment reasons grouped by category
    /// </summary>
    public static Dictionary<string, IEnumerable<string>> GetAssignmentReasonsByCategory()
    {
        return new Dictionary<string, IEnumerable<string>>
        {
            ["Administrative"] = new[]
            {
                RoleAssignmentReasonConstants.InitialSetup,
                RoleAssignmentReasonConstants.Promotion,
                RoleAssignmentReasonConstants.Demotion,
                RoleAssignmentReasonConstants.RoleModification,
                RoleAssignmentReasonConstants.RoleExpiration,
                RoleAssignmentReasonConstants.RoleRenewal,
                RoleAssignmentReasonConstants.RoleRemoval
            },
            ["Business"] = new[]
            {
                RoleAssignmentReasonConstants.NewHire,
                RoleAssignmentReasonConstants.CustomerRegistration,
                RoleAssignmentReasonConstants.ProjectAssignment,
                RoleAssignmentReasonConstants.ContractWork,
                RoleAssignmentReasonConstants.TrainingAssignment
            },
            ["Emergency"] = new[]
            {
                RoleAssignmentReasonConstants.EmergencyAccess,
                RoleAssignmentReasonConstants.TemporaryCoverage,
                RoleAssignmentReasonConstants.SecurityRequirement,
                RoleAssignmentReasonConstants.ComplianceRequirement,
                RoleAssignmentReasonConstants.AuditRequirement
            }
        };
    }

    /// <summary>
    /// Checks if a category is temporary
    /// </summary>
    public static bool IsTemporaryCategory(string category)
    {
        return category switch
        {
            RoleAssignmentConstants.Temporary or RoleAssignmentConstants.Emergency or RoleAssignmentConstants.Project or RoleAssignmentConstants.Contract => true,
            _ => false
        };
    }

    /// <summary>
    /// Checks if a reason is temporary
    /// </summary>
    public static bool IsTemporaryReason(string reason)
    {
        return reason switch
        {
            RoleAssignmentReasonConstants.TemporaryCoverage or RoleAssignmentReasonConstants.EmergencyAccess or RoleAssignmentReasonConstants.ProjectAssignment or RoleAssignmentReasonConstants.ContractWork => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets the default priority for a category
    /// </summary>
    public static int GetDefaultPriorityForCategory(string category)
    {
        return category switch
        {
            RoleAssignmentConstants.System => RoleAssignmentPriorityConstants.Highest,
            RoleAssignmentConstants.Administrative => RoleAssignmentPriorityConstants.High,
            RoleAssignmentConstants.Emergency => RoleAssignmentPriorityConstants.Highest,
            RoleAssignmentConstants.Business => RoleAssignmentPriorityConstants.Medium,
            RoleAssignmentConstants.Specialized => RoleAssignmentPriorityConstants.MediumHigh,
            RoleAssignmentConstants.Temporary => RoleAssignmentPriorityConstants.MediumHigh,
            RoleAssignmentConstants.Project => RoleAssignmentPriorityConstants.Medium,
            RoleAssignmentConstants.Contract => RoleAssignmentPriorityConstants.Medium,
            RoleAssignmentConstants.User => RoleAssignmentPriorityConstants.Low,
            _ => RoleAssignmentPriorityConstants.Default
        };
    }

    /// <summary>
    /// Gets the default priority for a reason
    /// </summary>
    public static int GetDefaultPriorityForReason(string reason)
    {
        return reason switch
        {
            RoleAssignmentReasonConstants.InitialSetup or RoleAssignmentReasonConstants.EmergencyAccess => RoleAssignmentPriorityConstants.Highest,
            RoleAssignmentReasonConstants.Promotion or RoleAssignmentReasonConstants.SecurityRequirement => RoleAssignmentPriorityConstants.High,
            RoleAssignmentReasonConstants.TemporaryCoverage or RoleAssignmentReasonConstants.AuditRequirement => RoleAssignmentPriorityConstants.MediumHigh,
            RoleAssignmentReasonConstants.NewHire or RoleAssignmentReasonConstants.ProjectAssignment => RoleAssignmentPriorityConstants.Medium,
            RoleAssignmentReasonConstants.CustomerRegistration or RoleAssignmentReasonConstants.TrainingAssignment => RoleAssignmentPriorityConstants.Low,
            _ => RoleAssignmentPriorityConstants.Default
        };
    }

    /// <summary>
    /// Gets the display name for a priority
    /// </summary>
    public static string GetPriorityDisplayName(int priority)
    {
        return priority switch
        {
            RoleAssignmentPriorityConstants.Highest => "Highest",
            RoleAssignmentPriorityConstants.High => "High",
            RoleAssignmentPriorityConstants.MediumHigh => "Medium-High",
            RoleAssignmentPriorityConstants.Medium => "Medium",
            RoleAssignmentPriorityConstants.MediumLow => "Medium-Low",
            RoleAssignmentPriorityConstants.Low => "Low",
            RoleAssignmentPriorityConstants.Lowest => "Lowest",
            _ => "Default"
        };
    }

    /// <summary>
    /// Gets the color for a priority (for UI)
    /// </summary>
    public static string GetPriorityColor(int priority)
    {
        return priority switch
        {
            RoleAssignmentPriorityConstants.Highest => "#dc3545", // Red
            RoleAssignmentPriorityConstants.High => "#fd7e14", // Orange
            RoleAssignmentPriorityConstants.MediumHigh => "#ffc107", // Yellow
            RoleAssignmentPriorityConstants.Medium => "#28a745", // Green
            RoleAssignmentPriorityConstants.MediumLow => "#17a2b8", // Cyan
            RoleAssignmentPriorityConstants.Low => "#6c757d", // Gray
            RoleAssignmentPriorityConstants.Lowest => "#6c757d", // Gray
            _ => "#6c757d" // Default gray
        };
    }

    /// <summary>
    /// Gets the icon for a priority (for UI)
    /// </summary>
    public static string GetPriorityIcon(int priority)
    {
        return priority switch
        {
            RoleAssignmentPriorityConstants.Highest => "fas fa-exclamation-triangle",
            RoleAssignmentPriorityConstants.High => "fas fa-exclamation-circle",
            RoleAssignmentPriorityConstants.MediumHigh => "fas fa-info-circle",
            RoleAssignmentPriorityConstants.Medium => "fas fa-check-circle",
            RoleAssignmentPriorityConstants.MediumLow => "fas fa-minus-circle",
            RoleAssignmentPriorityConstants.Low => "fas fa-circle",
            RoleAssignmentPriorityConstants.Lowest => "fas fa-circle",
            _ => "fas fa-circle"
        };
    }

    /// <summary>
    /// Gets the default expiration period for a category
    /// </summary>
    public static TimeSpan? GetDefaultExpirationForCategory(string category)
    {
        return category switch
        {
            RoleAssignmentConstants.Temporary => TimeSpan.FromDays(30),
            RoleAssignmentConstants.Emergency => TimeSpan.FromDays(7),
            RoleAssignmentConstants.Project => TimeSpan.FromDays(90),
            RoleAssignmentConstants.Contract => TimeSpan.FromDays(365),
            _ => null // No expiration
        };
    }

    /// <summary>
    /// Gets the default expiration period for a reason
    /// </summary>
    public static TimeSpan? GetDefaultExpirationForReason(string reason)
    {
        return reason switch
        {
            RoleAssignmentReasonConstants.TemporaryCoverage => TimeSpan.FromDays(30),
            RoleAssignmentReasonConstants.EmergencyAccess => TimeSpan.FromDays(7),
            RoleAssignmentReasonConstants.ProjectAssignment => TimeSpan.FromDays(90),
            RoleAssignmentReasonConstants.ContractWork => TimeSpan.FromDays(365),
            RoleAssignmentReasonConstants.TrainingAssignment => TimeSpan.FromDays(14),
            _ => null // No expiration
        };
    }

    /// <summary>
    /// Validates if a priority is valid
    /// </summary>
    public static bool IsValidPriority(int priority)
    {
        return priority >= RoleAssignmentPriorityConstants.Highest && priority <= RoleAssignmentPriorityConstants.Lowest;
    }

    /// <summary>
    /// Validates if a category is valid
    /// </summary>
    public static bool IsValidCategory(string category)
    {
        return GetAssignmentCategories().Contains(category);
    }

    /// <summary>
    /// Validates if a reason is valid
    /// </summary>
    public static bool IsValidReason(string reason)
    {
        return GetAssignmentReasons().Contains(reason);
    }
}