namespace MyShop.Contracts.DTOs.Identity.Authentication.Register;

/// <summary>
/// DTO for registration response
/// </summary>
public class RegisterResponseDto
{
    public bool IsSuccess { get; set; }
    public ApplicationUserDto? User { get; set; }
    public string? ErrorMessage { get; set; }
    public bool RequiresEmailConfirmation { get; set; }
    public string? EmailConfirmationToken { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}