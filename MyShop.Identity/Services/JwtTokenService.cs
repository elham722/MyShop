using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyShop.Contracts.Identity.Services;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.DTOs.Identity.JwtSettings;
using MyShop.Identity.Models;
using MyShop.Identity.Context;

namespace MyShop.Identity.Services;

/// <summary>
/// Implementation of JWT token service
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MyShopIdentityDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IAuditService _auditService;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(UserManager<ApplicationUser> userManager, MyShopIdentityDbContext context, 
        IConfiguration configuration, IAuditService auditService)
    {
        _userManager = userManager;
        _context = context;
        _configuration = configuration;
        _auditService = auditService;
        _jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
    }

    public async Task<string> GenerateAccessTokenAsync(ApplicationUserDto user)
    {
        var userEntity = await _userManager.FindByIdAsync(user.Id);
        if (userEntity == null)
            throw new InvalidOperationException("User not found");

        var claims = await GetUserClaimsAsync(userEntity);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // Store token in database
        await StoreTokenAsync(user.Id, "Access", tokenString, DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes));

        return tokenString;
    }

    public async Task<string> GenerateRefreshTokenAsync(ApplicationUserDto user)
    {
        var refreshToken = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
        var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        // Store refresh token in database
        await StoreTokenAsync(user.Id, "Refresh", refreshToken, expiresAt);

        return refreshToken;
    }

    public async Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            
            // Check if token exists in database and is not revoked
            var tokenExists = await _context.UserTokens
                .AnyAsync(ut => ut.Value == token && ut.IsActive && !ut.IsRevoked && 
                               (!ut.ExpiresAt.HasValue || ut.ExpiresAt > DateTime.UtcNow));

            if (!tokenExists)
                return null;

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
            .FirstOrDefaultAsync(ut => ut.Value == token);

        if (userToken == null)
            return false;

        userToken.Revoke("System", "Manual revocation");
        await _context.SaveChangesAsync();

        //await _auditService.LogAsync("Token Revoked", $"Token revoked for user {userToken.UserId}", userToken.UserId);
        return true;
    }

    public async Task<bool> RevokeAllUserTokensAsync(string userId)
    {
        var userTokens = await _context.UserTokens
            .Where(ut => ut.UserId == userId && ut.IsActive && !ut.IsRevoked)
            .ToListAsync();

        foreach (var token in userTokens)
        {
            token.Revoke("System", "Bulk revocation");
        }

        await _context.SaveChangesAsync();

       // await _auditService.LogAsync("All Tokens Revoked", $"All tokens revoked for user {userId}", userId);
        return true;
    }

    public async Task<IEnumerable<UserTokenDto>> GetUserTokensAsync(string userId)
    {
        var tokens = await _context.UserTokens
            .Where(ut => ut.UserId == userId)
            .OrderByDescending(ut => ut.CreatedAt)
            .ToListAsync();
        var tokenDto = MappingService.MapToDto(tokens);
        return tokenDto;
    }

    public async Task CleanupExpiredTokensAsync()
    {
        var expiredTokens = await _context.UserTokens
            .Where(ut => ut.ExpiresAt.HasValue && ut.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        foreach (var token in expiredTokens)
        {
            token.Deactivate("Expired", "System");
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
            new("CustomerId", user.CustomerId ?? "")
        };

        // Add roles
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Add permissions
        var permissions = await GetUserPermissionsAsync(user.Id);
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("Permission", permission));
        }

        return claims;
    }

    private async Task<List<string>> GetUserPermissionsAsync(string userId)
    {
        return null;
    }

    private async Task StoreTokenAsync(string userId, string tokenType, string tokenValue, DateTime expiresAt)
    {
        var userToken = UserToken.Create(
            userId: userId,
            loginProvider: "JWT",
            name: tokenType,
            value: tokenValue,
            expiresAt: expiresAt,
            tokenType: "Bearer",
            tokenPurpose: tokenType,
            createdBy: "System"
        );

        _context.UserTokens.Add(userToken);
        await _context.SaveChangesAsync();
    }

  
}