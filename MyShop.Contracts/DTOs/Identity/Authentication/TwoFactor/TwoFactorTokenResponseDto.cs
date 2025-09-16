namespace MyShop.Contracts.DTOs.Identity.Authentication.TwoFactor;

/// <summary>
/// DTO for two-factor token response
/// </summary>
public class TwoFactorTokenResponseDto
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int ExpiresInSeconds { get; set; }
    public string TokenType { get; set; } = "TOTP";
}