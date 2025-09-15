namespace MyShop.Identity.Enums;

/// <summary>
/// Represents the actions that can be performed on resources
/// </summary>
public enum ActionEnum
{
    // Basic CRUD Operations
    Create = 1,
    Read = 2,
    Update = 3,
    Delete = 4,
    
    // Extended Operations
    List = 5,
    View = 6,
    Edit = 7,
    Remove = 8,
    
    // Special Operations
    Execute = 10,
    Approve = 11,
    Reject = 12,
    Publish = 13,
    Unpublish = 14,
    
    // User Management Operations
    Assign = 20,
    Unassign = 21,
    Activate = 22,
    Deactivate = 23,
    Suspend = 24,
    Unsuspend = 25,
    
    // File Operations
    Upload = 30,
    Download = 31,
    Share = 32,
    Archive = 33,
    Restore = 34,
    
    // Financial Operations
    Pay = 40,
    Refund = 41,
    Transfer = 42,
    Withdraw = 43,
    Deposit = 44,
    
    // Communication Operations
    Send = 50,
    Receive = 51,
    Broadcast = 52,
    Notify = 53,
    
    // System Operations
    Configure = 60,
    Monitor = 61,
    Backup = 62,
    Migrate = 63,
    
    // API Operations
    Invoke = 70,
    Callback = 71,
    Webhook = 72,
    
    // Custom Actions (for extensibility)
    Custom = 100
}
/// <summary>
/// Extension methods for Action enum
/// </summary>
public static class ActionExtensions
{
    /// <summary>
    /// Gets the string representation of the action
    /// </summary>
    public static string ToStringValue(this ActionEnum action)
    {
        return action switch
        {
            ActionEnum.Create => "Create",
            ActionEnum.Read => "Read",
            ActionEnum.Update => "Update",
            ActionEnum.Delete => "Delete",
            ActionEnum.List => "List",
            ActionEnum.View => "View",
            ActionEnum.Edit => "Edit",
            ActionEnum.Remove => "Remove",
            ActionEnum.Execute => "Execute",
            ActionEnum.Approve => "Approve",
            ActionEnum.Reject => "Reject",
            ActionEnum.Publish => "Publish",
            ActionEnum.Unpublish => "Unpublish",
            ActionEnum.Assign => "Assign",
            ActionEnum.Unassign => "Unassign",
            ActionEnum.Activate => "Activate",
            ActionEnum.Deactivate => "Deactivate",
            ActionEnum.Suspend => "Suspend",
            ActionEnum.Unsuspend => "Unsuspend",
            ActionEnum.Upload => "Upload",
            ActionEnum.Download => "Download",
            ActionEnum.Share => "Share",
            ActionEnum.Archive => "Archive",
            ActionEnum.Restore => "Restore",
            ActionEnum.Pay => "Pay",
            ActionEnum.Refund => "Refund",
            ActionEnum.Transfer => "Transfer",
            ActionEnum.Withdraw => "Withdraw",
            ActionEnum.Deposit => "Deposit",
            ActionEnum.Send => "Send",
            ActionEnum.Receive => "Receive",
            ActionEnum.Broadcast => "Broadcast",
            ActionEnum.Notify => "Notify",
            ActionEnum.Configure => "Configure",
            ActionEnum.Monitor => "Monitor",
            ActionEnum.Backup => "Backup",
            ActionEnum.Migrate => "Migrate",
            ActionEnum.Invoke => "Invoke",
            ActionEnum.Callback => "Callback",
            ActionEnum.Webhook => "Webhook",
            ActionEnum.Custom => "Custom",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Parses string value to Action enum
    /// </summary>
    public static ActionEnum ParseFromString(string? value)
    {
        return value?.ToLowerInvariant() switch
        {
            "create" => ActionEnum.Create,
            "read" => ActionEnum.Read,
            "update" => ActionEnum.Update,
            "delete" => ActionEnum.Delete,
            "list" => ActionEnum.List,
            "view" => ActionEnum.View,
            "edit" => ActionEnum.Edit,
            "remove" => ActionEnum.Remove,
            "execute" => ActionEnum.Execute,
            "approve" => ActionEnum.Approve,
            "reject" => ActionEnum.Reject,
            "publish" => ActionEnum.Publish,
            "unpublish" => ActionEnum.Unpublish,
            "assign" => ActionEnum.Assign,
            "unassign" => ActionEnum.Unassign,
            "activate" => ActionEnum.Activate,
            "deactivate" => ActionEnum.Deactivate,
            "suspend" => ActionEnum.Suspend,
            "unsuspend" => ActionEnum.Unsuspend,
            "upload" => ActionEnum.Upload,
            "download" => ActionEnum.Download,
            "share" => ActionEnum.Share,
            "archive" => ActionEnum.Archive,
            "restore" => ActionEnum.Restore,
            "pay" => ActionEnum.Pay,
            "refund" => ActionEnum.Refund,
            "transfer" => ActionEnum.Transfer,
            "withdraw" => ActionEnum.Withdraw,
            "deposit" => ActionEnum.Deposit,
            "send" => ActionEnum.Send,
            "receive" => ActionEnum.Receive,
            "broadcast" => ActionEnum.Broadcast,
            "notify" => ActionEnum.Notify,
            "configure" => ActionEnum.Configure,
            "monitor" => ActionEnum.Monitor,
            "backup" => ActionEnum.Backup,
            "migrate" => ActionEnum.Migrate,
            "invoke" => ActionEnum.Invoke,
            "callback" => ActionEnum.Callback,
            "webhook" => ActionEnum.Webhook,
            "custom" => ActionEnum.Custom,
            _ => ActionEnum.Custom
        };
    }

    /// <summary>
    /// Gets the category of the action
    /// </summary>
    public static string GetCategory(this ActionEnum action)
    {
        return action switch
        {
            ActionEnum.Create or ActionEnum.Read or ActionEnum.Update or ActionEnum.Delete => "CRUD",
            ActionEnum.List or ActionEnum.View or ActionEnum.Edit or ActionEnum.Remove => "Basic",
            ActionEnum.Execute or ActionEnum.Approve or ActionEnum.Reject or ActionEnum.Publish or ActionEnum.Unpublish => "Special",
            ActionEnum.Assign or ActionEnum.Unassign or ActionEnum.Activate or ActionEnum.Deactivate or ActionEnum.Suspend or ActionEnum.Unsuspend => "User Management",
            ActionEnum.Upload or ActionEnum.Download or ActionEnum.Share or ActionEnum.Archive or ActionEnum.Restore => "File Operations",
            ActionEnum.Pay or ActionEnum.Refund or ActionEnum.Transfer or ActionEnum.Withdraw or ActionEnum.Deposit => "Financial",
            ActionEnum.Send or ActionEnum.Receive or ActionEnum.Broadcast or ActionEnum.Notify => "Communication",
            ActionEnum.Configure or ActionEnum.Monitor or ActionEnum.Backup or ActionEnum.Restore or ActionEnum.Migrate => "System",
            ActionEnum.Invoke or ActionEnum.Callback or ActionEnum.Webhook => "API",
            _ => "Custom"
        };
    }

    /// <summary>
    /// Determines if the action is a destructive operation
    /// </summary>
    public static bool IsDestructive(this ActionEnum action)
    {
        return action switch
        {
            ActionEnum.Delete or ActionEnum.Remove or ActionEnum.Unassign or ActionEnum.Deactivate or ActionEnum.Suspend or ActionEnum.Unpublish => true,
            _ => false
        };
    }

    /// <summary>
    /// Determines if the action requires special privileges
    /// </summary>
    public static bool RequiresSpecialPrivileges(this ActionEnum action)
    {
        return action switch
        {
            ActionEnum.Execute or ActionEnum.Approve or ActionEnum.Reject or ActionEnum.Configure or ActionEnum.Monitor or ActionEnum.Backup or ActionEnum.Migrate => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets the priority level for the action
    /// </summary>
    public static int GetPriority(this ActionEnum action)
    {
        return action switch
        {
            ActionEnum.Create or ActionEnum.Update or ActionEnum.Delete => 1, // High priority
            ActionEnum.Read or ActionEnum.List or ActionEnum.View => 2, // Medium priority
            ActionEnum.Execute or ActionEnum.Approve or ActionEnum.Reject => 3, // Special priority
            _ => 5 // Default priority
        };
    }
}
