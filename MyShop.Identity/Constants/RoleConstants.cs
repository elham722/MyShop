namespace MyShop.Identity.Constants;

/// <summary>
/// Constants for commonly used roles
/// </summary>
public static class RoleConstants
{
    /// <summary>
    /// System roles (cannot be modified or deleted)
    /// </summary>
    public static class System
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string SystemAdmin = "SystemAdmin";
    }

    /// <summary>
    /// Administrative roles
    /// </summary>
    public static class Administrative
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
    }

    /// <summary>
    /// Business roles
    /// </summary>
    public static class Business
    {
        public const string CustomerService = "CustomerService";
        public const string SalesRep = "SalesRep";
        public const string SupportAgent = "SupportAgent";
    }

    /// <summary>
    /// User roles
    /// </summary>
    public static class User
    {
        public const string Customer = "Customer";
        public const string Guest = "Guest";
    }

    /// <summary>
    /// Specialized roles
    /// </summary>
    public static class Specialized
    {
        public const string Auditor = "Auditor";
        public const string ReportViewer = "ReportViewer";
    }
}

/// <summary>
/// Role helper methods
/// </summary>
public static class RoleHelper
{
    /// <summary>
    /// Gets all system roles
    /// </summary>
    public static IEnumerable<string> GetSystemRoles()
    {
        return new[]
        {
            RoleConstants.System.SuperAdmin,
            RoleConstants.System.SystemAdmin
        };
    }

    /// <summary>
    /// Gets all administrative roles
    /// </summary>
    public static IEnumerable<string> GetAdministrativeRoles()
    {
        return new[]
        {
            RoleConstants.Administrative.Admin,
            RoleConstants.Administrative.Manager
        };
    }

    /// <summary>
    /// Gets all business roles
    /// </summary>
    public static IEnumerable<string> GetBusinessRoles()
    {
        return new[]
        {
            RoleConstants.Business.CustomerService,
            RoleConstants.Business.SalesRep,
            RoleConstants.Business.SupportAgent
        };
    }

    /// <summary>
    /// Gets all user roles
    /// </summary>
    public static IEnumerable<string> GetUserRoles()
    {
        return new[]
        {
            RoleConstants.User.Customer,
            RoleConstants.User.Guest
        };
    }

    /// <summary>
    /// Gets all specialized roles
    /// </summary>
    public static IEnumerable<string> GetSpecializedRoles()
    {
        return new[]
        {
            RoleConstants.Specialized.Auditor,
            RoleConstants.Specialized.ReportViewer
        };
    }

    /// <summary>
    /// Gets all roles grouped by category
    /// </summary>
    public static Dictionary<string, IEnumerable<string>> GetAllRolesByCategory()
    {
        return new Dictionary<string, IEnumerable<string>>
        {
            ["System"] = GetSystemRoles(),
            ["Administrative"] = GetAdministrativeRoles(),
            ["Business"] = GetBusinessRoles(),
            ["User"] = GetUserRoles(),
            ["Specialized"] = GetSpecializedRoles()
        };
    }

    /// <summary>
    /// Checks if a role is a system role
    /// </summary>
    public static bool IsSystemRole(string roleName)
    {
        return GetSystemRoles().Contains(roleName);
    }

    /// <summary>
    /// Gets the category of a role
    /// </summary>
    public static string GetRoleCategory(string roleName)
    {
        if (GetSystemRoles().Contains(roleName))
            return "System";
        
        if (GetAdministrativeRoles().Contains(roleName))
            return "Administrative";
        
        if (GetBusinessRoles().Contains(roleName))
            return "Business";
        
        if (GetUserRoles().Contains(roleName))
            return "User";
        
        if (GetSpecializedRoles().Contains(roleName))
            return "Specialized";
        
        return "Custom";
    }

    /// <summary>
    /// Gets the priority of a role
    /// </summary>
    public static int GetRolePriority(string roleName)
    {
        return roleName switch
        {
            RoleConstants.System.SuperAdmin => 1,
            RoleConstants.System.SystemAdmin => 2,
            RoleConstants.Administrative.Admin => 3,
            RoleConstants.Administrative.Manager => 4,
            RoleConstants.Business.CustomerService => 5,
            RoleConstants.Specialized.Auditor => 5,
            RoleConstants.Business.SalesRep => 6,
            RoleConstants.Specialized.ReportViewer => 6,
            RoleConstants.Business.SupportAgent => 7,
            RoleConstants.User.Customer => 8,
            RoleConstants.User.Guest => 9,
            _ => 10
        };
    }

    /// <summary>
    /// Gets roles that can be assigned to users
    /// </summary>
    public static IEnumerable<string> GetAssignableRoles()
    {
        return GetAllRolesByCategory()
            .Where(kvp => kvp.Key != "System")
            .SelectMany(kvp => kvp.Value);
    }

    /// <summary>
    /// Gets roles that require special privileges to assign
    /// </summary>
    public static IEnumerable<string> GetSpecialPrivilegeRoles()
    {
        return new[]
        {
            RoleConstants.System.SuperAdmin,
            RoleConstants.System.SystemAdmin,
            RoleConstants.Administrative.Admin,
            RoleConstants.Specialized.Auditor
        };
    }

    /// <summary>
    /// Checks if a role requires special privileges to assign
    /// </summary>
    public static bool RequiresSpecialPrivileges(string roleName)
    {
        return GetSpecialPrivilegeRoles().Contains(roleName);
    }
}