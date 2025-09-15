using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MyShop.Contracts.DTOs.Identity;

namespace MyShop.Contracts.Identity.Services
{
    /// <summary>
    /// Service for managing JWT tokens
    /// </summary>
    public interface IJwtTokenService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUserDto user);
        Task<string> GenerateRefreshTokenAsync(ApplicationUserDto user);
        Task<ClaimsPrincipal?> ValidateTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<bool> RevokeAllUserTokensAsync(string userId);
        Task<IEnumerable<UserTokenDto>> GetUserTokensAsync(string userId);
        Task CleanupExpiredTokensAsync();
    }

}
