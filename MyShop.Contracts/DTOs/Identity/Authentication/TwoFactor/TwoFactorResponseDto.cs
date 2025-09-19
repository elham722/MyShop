namespace MyShop.Contracts.DTOs.Identity.Authentication.TwoFactor;

/// <summary>
/// DTO for generic two-factor operations (enable, disable, verify)
/// </summary>
public class TwoFactorResponseDto
{
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// The method used (e.g., "Email", "SMS", "AuthenticatorApp")
    /// </summary>
    public string Method { get; set; } = "AuthenticatorApp";

    /// <summary>
    /// Optional message or extra info
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// When the 2FA was enabled/disabled/verified
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}