namespace MyShop.Identity.Constants;

/// <summary>
/// Constants for commonly used login providers
/// </summary>
public static class LoginProviderConstants
{
    /// <summary>
    /// Local authentication provider
    /// </summary>
    public const string Local = "Local";

    /// <summary>
    /// Google authentication provider
    /// </summary>
    public const string Google = "Google";

    /// <summary>
    /// Microsoft authentication provider
    /// </summary>
    public const string Microsoft = "Microsoft";

    /// <summary>
    /// Facebook authentication provider
    /// </summary>
    public const string Facebook = "Facebook";

    /// <summary>
    /// Twitter authentication provider
    /// </summary>
    public const string Twitter = "Twitter";

    /// <summary>
    /// LinkedIn authentication provider
    /// </summary>
    public const string LinkedIn = "LinkedIn";

    /// <summary>
    /// GitHub authentication provider
    /// </summary>
    public const string GitHub = "GitHub";

    /// <summary>
    /// Apple authentication provider
    /// </summary>
    public const string Apple = "Apple";

    /// <summary>
    /// Amazon authentication provider
    /// </summary>
    public const string Amazon = "Amazon";

    /// <summary>
    /// Discord authentication provider
    /// </summary>
    public const string Discord = "Discord";

    /// <summary>
    /// Twitch authentication provider
    /// </summary>
    public const string Twitch = "Twitch";

    /// <summary>
    /// Steam authentication provider
    /// </summary>
    public const string Steam = "Steam";
}

/// <summary>
/// Login provider helper methods
/// </summary>
public static class LoginProviderHelper
{
    /// <summary>
    /// Gets all external login providers
    /// </summary>
    public static IEnumerable<string> GetExternalProviders()
    {
        return new[]
        {
            LoginProviderConstants.Google,
            LoginProviderConstants.Microsoft,
            LoginProviderConstants.Facebook,
            LoginProviderConstants.Twitter,
            LoginProviderConstants.LinkedIn,
            LoginProviderConstants.GitHub,
            LoginProviderConstants.Apple,
            LoginProviderConstants.Amazon,
            LoginProviderConstants.Discord,
            LoginProviderConstants.Twitch,
            LoginProviderConstants.Steam
        };
    }

    /// <summary>
    /// Gets all social media providers
    /// </summary>
    public static IEnumerable<string> GetSocialMediaProviders()
    {
        return new[]
        {
            LoginProviderConstants.Facebook,
            LoginProviderConstants.Twitter,
            LoginProviderConstants.LinkedIn,
            LoginProviderConstants.Discord,
            LoginProviderConstants.Twitch
        };
    }

    /// <summary>
    /// Gets all enterprise providers
    /// </summary>
    public static IEnumerable<string> GetEnterpriseProviders()
    {
        return new[]
        {
            LoginProviderConstants.Google,
            LoginProviderConstants.Microsoft,
            LoginProviderConstants.LinkedIn
        };
    }

    /// <summary>
    /// Gets all gaming providers
    /// </summary>
    public static IEnumerable<string> GetGamingProviders()
    {
        return new[]
        {
            LoginProviderConstants.Steam,
            LoginProviderConstants.Discord,
            LoginProviderConstants.Twitch
        };
    }

    /// <summary>
    /// Gets all providers grouped by category
    /// </summary>
    public static Dictionary<string, IEnumerable<string>> GetAllProvidersByCategory()
    {
        return new Dictionary<string, IEnumerable<string>>
        {
            ["Local"] = new[] { LoginProviderConstants.Local },
            ["External"] = GetExternalProviders(),
            ["Social Media"] = GetSocialMediaProviders(),
            ["Enterprise"] = GetEnterpriseProviders(),
            ["Gaming"] = GetGamingProviders()
        };
    }

    /// <summary>
    /// Checks if a provider is external
    /// </summary>
    public static bool IsExternalProvider(string provider)
    {
        return GetExternalProviders().Contains(provider);
    }

    /// <summary>
    /// Checks if a provider is social media
    /// </summary>
    public static bool IsSocialMediaProvider(string provider)
    {
        return GetSocialMediaProviders().Contains(provider);
    }

    /// <summary>
    /// Checks if a provider is enterprise
    /// </summary>
    public static bool IsEnterpriseProvider(string provider)
    {
        return GetEnterpriseProviders().Contains(provider);
    }

    /// <summary>
    /// Checks if a provider is gaming
    /// </summary>
    public static bool IsGamingProvider(string provider)
    {
        return GetGamingProviders().Contains(provider);
    }

    /// <summary>
    /// Gets the category of a provider
    /// </summary>
    public static string GetProviderCategory(string provider)
    {
        if (provider == LoginProviderConstants.Local)
            return "Local";
        
        if (IsSocialMediaProvider(provider))
            return "Social Media";
        
        if (IsEnterpriseProvider(provider))
            return "Enterprise";
        
        if (IsGamingProvider(provider))
            return "Gaming";
        
        if (IsExternalProvider(provider))
            return "External";
        
        return "Unknown";
    }

    /// <summary>
    /// Gets the display name of a provider
    /// </summary>
    public static string GetProviderDisplayName(string provider)
    {
        return provider switch
        {
            LoginProviderConstants.Local => "Local Account",
            LoginProviderConstants.Google => "Google",
            LoginProviderConstants.Microsoft => "Microsoft",
            LoginProviderConstants.Facebook => "Facebook",
            LoginProviderConstants.Twitter => "Twitter",
            LoginProviderConstants.LinkedIn => "LinkedIn",
            LoginProviderConstants.GitHub => "GitHub",
            LoginProviderConstants.Apple => "Apple",
            LoginProviderConstants.Amazon => "Amazon",
            LoginProviderConstants.Discord => "Discord",
            LoginProviderConstants.Twitch => "Twitch",
            LoginProviderConstants.Steam => "Steam",
            _ => provider
        };
    }

    /// <summary>
    /// Gets the icon class for a provider (for UI)
    /// </summary>
    public static string GetProviderIconClass(string provider)
    {
        return provider switch
        {
            LoginProviderConstants.Google => "fab fa-google",
            LoginProviderConstants.Microsoft => "fab fa-microsoft",
            LoginProviderConstants.Facebook => "fab fa-facebook",
            LoginProviderConstants.Twitter => "fab fa-twitter",
            LoginProviderConstants.LinkedIn => "fab fa-linkedin",
            LoginProviderConstants.GitHub => "fab fa-github",
            LoginProviderConstants.Apple => "fab fa-apple",
            LoginProviderConstants.Amazon => "fab fa-amazon",
            LoginProviderConstants.Discord => "fab fa-discord",
            LoginProviderConstants.Twitch => "fab fa-twitch",
            LoginProviderConstants.Steam => "fab fa-steam",
            _ => "fas fa-user"
        };
    }

    /// <summary>
    /// Gets the color for a provider (for UI)
    /// </summary>
    public static string GetProviderColor(string provider)
    {
        return provider switch
        {
            LoginProviderConstants.Google => "#4285F4",
            LoginProviderConstants.Microsoft => "#00BCF2",
            LoginProviderConstants.Facebook => "#1877F2",
            LoginProviderConstants.Twitter => "#1DA1F2",
            LoginProviderConstants.LinkedIn => "#0077B5",
            LoginProviderConstants.GitHub => "#333333",
            LoginProviderConstants.Apple => "#000000",
            LoginProviderConstants.Amazon => "#FF9900",
            LoginProviderConstants.Discord => "#5865F2",
            LoginProviderConstants.Twitch => "#9146FF",
            LoginProviderConstants.Steam => "#171a21",
            _ => "#6c757d"
        };
    }

    /// <summary>
    /// Checks if a provider requires special security measures
    /// </summary>
    public static bool RequiresSpecialSecurity(string provider)
    {
        return provider switch
        {
            LoginProviderConstants.Local => true, // Local accounts need strong passwords
            LoginProviderConstants.Microsoft => true, // Enterprise provider
            LoginProviderConstants.LinkedIn => true, // Professional network
            _ => false
        };
    }

    /// <summary>
    /// Gets the default trust level for a provider
    /// </summary>
    public static bool GetDefaultTrustLevel(string provider)
    {
        return provider switch
        {
            LoginProviderConstants.Local => true, // Local accounts are trusted by default
            LoginProviderConstants.Microsoft => true, // Enterprise provider
            LoginProviderConstants.LinkedIn => true, // Professional network
            _ => false // External providers are not trusted by default
        };
    }
}