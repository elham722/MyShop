using System.ComponentModel.DataAnnotations;

namespace MyShop.Contracts.DTOs.Identity.Authentication.TwoFactor;

/// <summary>
/// DTO for verify two-factor authentication request
/// </summary>
public class VerifyTwoFactorRequestDto
{
    [Required(ErrorMessage = "User ID is required")]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Token is required")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Token must be exactly 6 digits")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Token must contain only digits")]
    public string Token { get; set; } = string.Empty;
}