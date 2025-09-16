using System.ComponentModel.DataAnnotations;

namespace MyShop.Contracts.DTOs.Identity.Authentication.LockUser;

/// <summary>
/// DTO for lock user request
/// </summary>
public class LockUserRequestDto
{
    [Required(ErrorMessage = "User ID is required")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Lockout duration in minutes (optional, defaults to 15 minutes)
    /// </summary>
    [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes (24 hours)")]
    public int? DurationMinutes { get; set; } = 15;

    /// <summary>
    /// Reason for locking the user
    /// </summary>
    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? Reason { get; set; }
}