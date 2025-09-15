using MyShop.Contracts.Enums.Identity;

namespace MyShop.Identity.Constants;

/// <summary>
/// Constants for commonly used permissions
/// </summary>
public static class PermissionConstants
{
    /// <summary>
    /// System permissions
    /// </summary>
    public static class System
    {
        public const string Configure = "System.Configure";
        public const string Monitor = "System.Monitor";
        public const string Backup = "System.Backup";
        public const string Migrate = "System.Migrate";
    }

    /// <summary>
    /// User management permissions
    /// </summary>
    public static class User
    {
        public const string Create = "User.Create";
        public const string Read = "User.Read";
        public const string Update = "User.Update";
        public const string Delete = "User.Delete";
        public const string List = "User.List";
        public const string Activate = "User.Activate";
        public const string Deactivate = "User.Deactivate";
        public const string Suspend = "User.Suspend";
    }

    /// <summary>
    /// Role management permissions
    /// </summary>
    public static class Role
    {
        public const string Create = "Role.Create";
        public const string Read = "Role.Read";
        public const string Update = "Role.Update";
        public const string Delete = "Role.Delete";
        public const string List = "Role.List";
        public const string Assign = "Role.Assign";
        public const string Unassign = "Role.Unassign";
    }

    /// <summary>
    /// Permission management permissions
    /// </summary>
    public static class Permission
    {
        public const string Create = "Permission.Create";
        public const string Read = "Permission.Read";
        public const string Update = "Permission.Update";
        public const string Delete = "Permission.Delete";
        public const string List = "Permission.List";
        public const string Assign = "Permission.Assign";
        public const string Unassign = "Permission.Unassign";
    }

    /// <summary>
    /// Customer management permissions
    /// </summary>
    public static class Customer
    {
        public const string Create = "Customer.Create";
        public const string Read = "Customer.Read";
        public const string Update = "Customer.Update";
        public const string Delete = "Customer.Delete";
        public const string List = "Customer.List";
        public const string Activate = "Customer.Activate";
        public const string Deactivate = "Customer.Deactivate";
        public const string Suspend = "Customer.Suspend";
    }

    /// <summary>
    /// Audit permissions
    /// </summary>
    public static class Audit
    {
        public const string Read = "Audit.Read";
        public const string List = "Audit.List";
        public const string Execute = "Audit.Execute";
    }
}

/// <summary>
/// Permission helper methods
/// </summary>
public static class PermissionHelper
{
    /// <summary>
    /// Gets all system permissions
    /// </summary>
    public static IEnumerable<string> GetSystemPermissions()
    {
        return new[]
        {
            PermissionConstants.System.Configure,
            PermissionConstants.System.Monitor,
            PermissionConstants.System.Backup,
            PermissionConstants.System.Migrate,
            PermissionConstants.Audit.Read,
            PermissionConstants.Audit.List,
            PermissionConstants.Audit.Execute
        };
    }

    /// <summary>
    /// Gets all user management permissions
    /// </summary>
    public static IEnumerable<string> GetUserManagementPermissions()
    {
        return new[]
        {
            PermissionConstants.User.Create,
            PermissionConstants.User.Read,
            PermissionConstants.User.Update,
            PermissionConstants.User.Delete,
            PermissionConstants.User.List,
            PermissionConstants.User.Activate,
            PermissionConstants.User.Deactivate,
            PermissionConstants.User.Suspend,
            PermissionConstants.Role.Create,
            PermissionConstants.Role.Read,
            PermissionConstants.Role.Update,
            PermissionConstants.Role.Delete,
            PermissionConstants.Role.List,
            PermissionConstants.Role.Assign,
            PermissionConstants.Role.Unassign,
            PermissionConstants.Permission.Create,
            PermissionConstants.Permission.Read,
            PermissionConstants.Permission.Update,
            PermissionConstants.Permission.Delete,
            PermissionConstants.Permission.List,
            PermissionConstants.Permission.Assign,
            PermissionConstants.Permission.Unassign
        };
    }

    /// <summary>
    /// Gets all business permissions
    /// </summary>
    public static IEnumerable<string> GetBusinessPermissions()
    {
        return new[]
        {
            PermissionConstants.Customer.Create,
            PermissionConstants.Customer.Read,
            PermissionConstants.Customer.Update,
            PermissionConstants.Customer.Delete,
            PermissionConstants.Customer.List,
            PermissionConstants.Customer.Activate,
            PermissionConstants.Customer.Deactivate,
            PermissionConstants.Customer.Suspend
        };
    }

    /// <summary>
    /// Gets all permissions grouped by category
    /// </summary>
    public static Dictionary<string, IEnumerable<string>> GetAllPermissionsByCategory()
    {
        return new Dictionary<string, IEnumerable<string>>
        {
            ["System"] = GetSystemPermissions(),
            ["User Management"] = GetUserManagementPermissions(),
            ["Business"] = GetBusinessPermissions()
        };
    }

    /// <summary>
    /// Checks if a permission is a system permission
    /// </summary>
    public static bool IsSystemPermission(string permissionName)
    {
        return GetSystemPermissions().Contains(permissionName);
    }

    /// <summary>
    /// Gets the resource and action from a permission name
    /// </summary>
    public static (Resource resource, ActionEnum action) ParsePermission(string permissionName)
    {
        var parts = permissionName.Split('.');
        if (parts.Length != 2)
            throw new ArgumentException("Invalid permission format. Expected format: Resource.Action", nameof(permissionName));

        var resource = ResourceExtensions.ParseFromString(parts[0]);
        var action = ActionExtensions.ParseFromString(parts[1]);

        return (resource, action);
    }

    /// <summary>
    /// Creates a permission name from resource and action
    /// </summary>
    public static string CreatePermissionName(Resource resource, ActionEnum action)
    {
        return $"{resource.ToStringValue()}.{action.ToStringValue()}";
    }
}