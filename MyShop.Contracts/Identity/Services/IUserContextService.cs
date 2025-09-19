namespace MyShop.Contracts.Identity.Services;

/// <summary>
/// Service for accessing current user context information
/// </summary>
public interface IUserContextService
{
    /// <summary>
    /// Gets the current user ID from the context
    /// </summary>
    string? GetCurrentUserId();

    /// <summary>
    /// Gets the current user name from the context
    /// </summary>
    string? GetCurrentUserName();

    /// <summary>
    /// Gets the current user email from the context
    /// </summary>
    string? GetCurrentUserEmail();

    /// <summary>
    /// Gets the current user roles from the context
    /// </summary>
    Task<IList<string>> GetCurrentUserRolesAsync();

    /// <summary>
    /// Checks if the current user has a specific role
    /// </summary>
    Task<bool> IsInRoleAsync(string role);

    /// <summary>
    /// Gets the current user's IP address
    /// </summary>
    string? GetCurrentUserIpAddress();

    /// <summary>
    /// Gets the current user's user agent
    /// </summary>
    string? GetCurrentUserAgent();

    /// <summary>
    /// Checks if there is an authenticated user in the context
    /// </summary>
    bool IsAuthenticated();

    /// <summary>
    /// Gets the current user's device information
    /// </summary>
    string? GetCurrentDeviceInfo();
}