using System.ComponentModel.DataAnnotations;

namespace MyShop.Contracts.DTOs.Identity.Authentication.LockUser;

/// <summary>
/// DTO for unlock user request
/// </summary>
public class UnlockUserRequestDto
{
    [Required(ErrorMessage = "User ID is required")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Reason for unlocking the user
    /// </summary>
    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? Reason { get; set; }
}