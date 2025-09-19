using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyShop.Contracts.Identity.Services;
using MyShop.Identity.Models;
using System.Security.Claims;

namespace MyShop.Identity.Services;

/// <summary>
/// Implementation of IUserContextService for accessing current user context
/// </summary>
public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserContextService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string? GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public string? GetCurrentUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    public async Task<IList<string>> GetCurrentUserRolesAsync()
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
            return new List<string>();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new List<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> IsInRoleAsync(string role)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
            return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        return await _userManager.IsInRoleAsync(user, role);
    }

    public string? GetCurrentUserIpAddress()
    {
        return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
    }

    public string? GetCurrentUserAgent()
    {
        return _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    public string? GetCurrentDeviceInfo()
    {
        return _httpContextAccessor.HttpContext?.Request?.Headers["Device-Info"].ToString();
    }
}