using Microsoft.AspNetCore.Mvc;
using MyShop.Contracts.DTOs.Responses;
using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.API.Controllers.Common.V1;
using MyShop.Contracts.DTOs.Identity.Authentication.Login;
using MyShop.Contracts.DTOs.Identity.Authentication.Register;
using MyShop.Contracts.DTOs.Identity.Authentication.Token;
using MyShop.Contracts.Identity.Services.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.Logout;

namespace MyShop.API.Controllers.Auth.V1;

/// <summary>
/// Authentication controller for API v1.0
/// </summary>
[ApiVersion("1.0")]
public class AuthController : BaseController
{
    private readonly IAuthenticationFacade _authFacade;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthenticationFacade authFacade, ILogger<AuthController> logger)
    {
        _authFacade = authFacade;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        // Call the facade/service
        var result = await _authFacade.LoginAsync(request);

        // Return standardized API response
        return FromResult(result);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<RegisterResponseDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

        var result = await _authFacade.RegisterAsync(request);

        // Created response for successful registration
        return FromResult(result);
    }

    [HttpPost("logout")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
    {
        var result = await _authFacade.LogoutAsync(request);
        return FromResult(result);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authFacade.LoginWithRefreshTokenAsync(request);
        return FromResult(result);
    }

}