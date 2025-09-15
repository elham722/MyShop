using MyShop.Contracts.DTOs.Identity;

namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Result of authentication operations
    /// </summary>
    public class AuthenticationResult
    {
        public bool IsSuccess { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public ApplicationUserDto? User { get; set; }
        public string? ErrorMessage { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public bool RequiresEmailConfirmation { get; set; }
        public bool IsAccountLocked { get; set; }
        public DateTime? LockoutEnd { get; set; }
    }
}