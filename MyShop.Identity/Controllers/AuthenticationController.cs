using Microsoft.AspNetCore.Mvc;
using MyShop.Contracts.Common;
using MyShop.Contracts.DTOs.Identity.Authentication.Login;
using MyShop.Contracts.DTOs.Identity.Authentication.Logout;
using MyShop.Contracts.DTOs.Identity.Authentication.Token;
using MyShop.Contracts.Identity.Services.Authentication;

namespace MyShop.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationFacade _authenticationFacade;

    public AuthenticationController(IAuthenticationFacade authenticationFacade)
    {
        _authenticationFacade = authenticationFacade;
    }

    /// <summary>
    /// User login endpoint
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        var result = await _authenticationFacade.LoginAsync(request, ipAddress, userAgent);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(new { Error = result.Error });
    }

    /// <summary>
    /// Refresh token endpoint
    /// </summary>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        var result = await _authenticationFacade.LoginWithRefreshTokenAsync(request, ipAddress, userAgent);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(new { Error = result.Error });
    }

    /// <summary>
    /// User logout endpoint
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        var result = await _authenticationFacade.LogoutAsync(request, ipAddress, userAgent);

        if (result.IsSuccess)
        {
            return Ok(new { Message = "Logged out successfully" });
        }

        return BadRequest(new { Error = result.Error });
    }

    /// <summary>
    /// Logout from all devices endpoint
    /// </summary>
    [HttpPost("logout-all-devices")]
    public async Task<IActionResult> LogoutAllDevices([FromBody] LogoutRequestDto request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        var result = await _authenticationFacade.LogoutAllDevicesAsync(request, ipAddress, userAgent);

        if (result.IsSuccess)
        {
            return Ok(new { Message = "Logged out from all devices successfully" });
        }

        return BadRequest(new { Error = result.Error });
    }
}