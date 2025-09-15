using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyShop.Identity.Models;
using MyShop.Identity.Context;

namespace MyShop.Identity.Services;

/// <summary>
/// Service for managing JWT tokens
/// </summary>
public interface IJwtTokenService
{
    Task<string> GenerateAccessTokenAsync(ApplicationUser user);
    Task<string> GenerateRefreshTokenAsync(ApplicationUser user);
    Task<ClaimsPrincipal?> ValidateTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
    Task<bool> RevokeAllUserTokensAsync(string userId);
    Task<IEnumerable<UserToken>> GetUserTokensAsync(string userId);
    Task CleanupExpiredTokensAsync();
}

/// <summary>
/// Implementation of JWT token service
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MyShopIdentityDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IAuditService _auditService;

    public JwtTokenService(UserManager<ApplicationUser> userManager, MyShopIdentityDbContext context, 
        IConfiguration configuration, IAuditService auditService)
    {
        _userManager = userManager;
        _context = context;
        _configuration = configuration;
        _auditService = auditService;
    }

    public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var claims = await GetUserClaimsAsync(user);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(ApplicationUser user)
    {
        var refreshToken = Guid.NewGuid().ToString();
        var expiresAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7"));

        var userToken = UserToken.Create(
            userId: user.Id,
            loginProvider: "JWT",
            name: "RefreshToken",
            value: refreshToken,
            expiresAt: expiresAt,
            tokenType: "RefreshToken",
            tokenPurpose: "Authentication",
            createdBy: "System"
        );

        _context.UserTokens.Add(userToken);
        await _context.SaveChangesAsync();

        await _auditService.LogTokenOperationAsync(
            userId: user.Id,
            action: "RefreshTokenGenerated",
            tokenId: userToken.Id,
            isSuccess: true
        );

        return refreshToken;
    }

    public async Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "");

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var userToken = await _context.UserTokens
            .FirstOrDefaultAsync(ut => ut.Value == token && ut.IsActive && !ut.IsRevoked);

        if (userToken == null) return false;

        userToken.Revoke("System", "ManualRevocation");
        await _context.SaveChangesAsync();

        await _auditService.LogTokenOperationAsync(
            userId: userToken.UserId,
            action: "TokenRevoked",
            tokenId: userToken.Id,
            isSuccess: true
        );

        return true;
    }

    public async Task<bool> RevokeAllUserTokensAsync(string userId)
    {
        var userTokens = await _context.UserTokens
            .Where(ut => ut.UserId == userId && ut.IsActive && !ut.IsRevoked)
            .ToListAsync();

        foreach (var token in userTokens)
        {
            token.Revoke("System", "BulkRevocation");
        }

        await _context.SaveChangesAsync();

        await _auditService.LogTokenOperationAsync(
            userId: userId,
            action: "AllTokensRevoked",
            additionalData: $"Revoked {userTokens.Count} tokens",
            isSuccess: true
        );

        return true;
    }

    public async Task<IEnumerable<UserToken>> GetUserTokensAsync(string userId)
    {
        return await _context.UserTokens
            .Where(ut => ut.UserId == userId && ut.IsActive)
            .OrderByDescending(ut => ut.CreatedAt)
            .ToListAsync();
    }

    public async Task CleanupExpiredTokensAsync()
    {
        var expiredTokens = await _context.UserTokens
            .Where(ut => ut.ExpiresAt.HasValue && ut.ExpiresAt.Value < DateTime.UtcNow && ut.IsActive)
            .ToListAsync();

        foreach (var token in expiredTokens)
        {
            token.Revoke("System", "Expired");
        }

        await _context.SaveChangesAsync();
    }

    private async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? ""),
            new(ClaimTypes.Email, user.Email ?? ""),
            new("CustomerId", user.CustomerId ?? ""),
            new("IsActive", user.IsActive.ToString()),
            new("IsLocked", user.IsLocked.ToString()),
            new("IsAccountLocked", user.IsAccountLocked.ToString()),
            new("LastLoginAt", user.LastLoginAt?.ToString("O") ?? ""),
            new("LastPasswordChangeAt", user.LastPasswordChangeAt?.ToString("O") ?? ""),
            new("LoginAttempts", user.LoginAttempts.ToString()),
            new("RequiresPasswordChange", user.RequiresPasswordChange.ToString()),
            new("TotpEnabled", user.TotpEnabled.ToString()),
            new("SmsEnabled", user.SmsEnabled.ToString())
        };

        // Add roles
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Add permissions
        var rolePermissionService = new RolePermissionService(_context, null!, _userManager);
        var permissions = await rolePermissionService.GetUserPermissionNamesAsync(user.Id);
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("Permission", permission));
        }

        return claims;
    }
}