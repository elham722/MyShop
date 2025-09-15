namespace MyShop.Contracts.Enums.Identity;

/// <summary>
/// Represents the resources that can be accessed in the system
/// </summary>
public enum Resource
{
    // System Resources
    System = 0,
    Audit = 1,
    Configuration = 2,
    
    // User Management Resources
    User = 10,
    Role = 11,
    Permission = 12,
    UserRole = 13,
    RolePermission = 14,
    
    // Business Resources
    Customer = 20,
    Order = 21,
    Product = 22,
    Category = 23,
    Inventory = 24,
    
    // Financial Resources
    Payment = 30,
    Invoice = 31,
    Transaction = 32,
    Report = 33,
    
    // Communication Resources
    Notification = 40,
    Email = 41,
    Sms = 42,
    Message = 43,
    
    // File Resources
    File = 50,
    Document = 51,
    Image = 52,
    Attachment = 53,
    
    // API Resources
    Api = 60,
    Webhook = 61,
    Integration = 62,
    
    // Custom Resources (for extensibility)
    Custom = 100
}

/// <summary>
/// Extension methods for Resource enum
/// </summary>
public static class ResourceExtensions
{
    /// <summary>
    /// Gets the string representation of the resource
    /// </summary>
    public static string ToStringValue(this Resource resource)
    {
        return resource switch
        {
            Resource.System => "System",
            Resource.Audit => "Audit",
            Resource.Configuration => "Configuration",
            Resource.User => "User",
            Resource.Role => "Role",
            Resource.Permission => "Permission",
            Resource.UserRole => "UserRole",
            Resource.RolePermission => "RolePermission",
            Resource.Customer => "Customer",
            Resource.Order => "Order",
            Resource.Product => "Product",
            Resource.Category => "Category",
            Resource.Inventory => "Inventory",
            Resource.Payment => "Payment",
            Resource.Invoice => "Invoice",
            Resource.Transaction => "Transaction",
            Resource.Report => "Report",
            Resource.Notification => "Notification",
            Resource.Email => "Email",
            Resource.Sms => "Sms",
            Resource.Message => "Message",
            Resource.File => "File",
            Resource.Document => "Document",
            Resource.Image => "Image",
            Resource.Attachment => "Attachment",
            Resource.Api => "Api",
            Resource.Webhook => "Webhook",
            Resource.Integration => "Integration",
            Resource.Custom => "Custom",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Parses string value to Resource enum
    /// </summary>
    public static Resource ParseFromString(string? value)
    {
        return value?.ToLowerInvariant() switch
        {
            "system" => Resource.System,
            "audit" => Resource.Audit,
            "configuration" => Resource.Configuration,
            "user" => Resource.User,
            "role" => Resource.Role,
            "permission" => Resource.Permission,
            "userrole" => Resource.UserRole,
            "rolepermission" => Resource.RolePermission,
            "customer" => Resource.Customer,
            "order" => Resource.Order,
            "product" => Resource.Product,
            "category" => Resource.Category,
            "inventory" => Resource.Inventory,
            "payment" => Resource.Payment,
            "invoice" => Resource.Invoice,
            "transaction" => Resource.Transaction,
            "report" => Resource.Report,
            "notification" => Resource.Notification,
            "email" => Resource.Email,
            "sms" => Resource.Sms,
            "message" => Resource.Message,
            "file" => Resource.File,
            "document" => Resource.Document,
            "image" => Resource.Image,
            "attachment" => Resource.Attachment,
            "api" => Resource.Api,
            "webhook" => Resource.Webhook,
            "integration" => Resource.Integration,
            "custom" => Resource.Custom,
            _ => Resource.Custom
        };
    }

    /// <summary>
    /// Gets the category of the resource
    /// </summary>
    public static string GetCategory(this Resource resource)
    {
        return resource switch
        {
            Resource.System or Resource.Audit or Resource.Configuration => "System",
            Resource.User or Resource.Role or Resource.Permission or Resource.UserRole or Resource.RolePermission => "User Management",
            Resource.Customer or Resource.Order or Resource.Product or Resource.Category or Resource.Inventory => "Business",
            Resource.Payment or Resource.Invoice or Resource.Transaction or Resource.Report => "Financial",
            Resource.Notification or Resource.Email or Resource.Sms or Resource.Message => "Communication",
            Resource.File or Resource.Document or Resource.Image or Resource.Attachment => "File Management",
            Resource.Api or Resource.Webhook or Resource.Integration => "Integration",
            _ => "Custom"
        };
    }

    /// <summary>
    /// Determines if the resource is a system resource
    /// </summary>
    public static bool IsSystemResource(this Resource resource)
    {
        return resource switch
        {
            Resource.System or Resource.Audit or Resource.Configuration => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets the priority level for the resource
    /// </summary>
    public static int GetPriority(this Resource resource)
    {
        return resource switch
        {
            Resource.System or Resource.Audit => 1, // Highest priority
            Resource.User or Resource.Role or Resource.Permission => 2,
            Resource.Customer or Resource.Order => 3,
            Resource.Product or Resource.Category => 4,
            Resource.Payment or Resource.Invoice => 5,
            _ => 10 // Default priority
        };
    }
}