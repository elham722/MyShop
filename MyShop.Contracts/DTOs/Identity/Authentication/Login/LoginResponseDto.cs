namespace MyShop.Contracts.DTOs.Identity.Authentication.Login;

/// <summary>
/// DTO for login response
/// </summary>
public class LoginResponseDto
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
    public DateTime ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
}