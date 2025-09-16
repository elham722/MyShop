using System.ComponentModel.DataAnnotations;

namespace MyShop.Contracts.DTOs.Identity.Authentication.TwoFactor;

/// <summary>
/// DTO for two-factor authentication request
/// </summary>
public class TwoFactorRequestDto
{
    [Required(ErrorMessage = "User ID is required")]
    public string UserId { get; set; } = string.Empty;
}