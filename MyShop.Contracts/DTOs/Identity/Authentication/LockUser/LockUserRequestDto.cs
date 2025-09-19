using System.ComponentModel.DataAnnotations;

namespace MyShop.Contracts.DTOs.Identity.Authentication.LockUser;

public class LockUserRequestDto
{
    [Required(ErrorMessage = "User ID is required")]
    public string UserId { get; set; } = string.Empty;

    [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes (24 hours)")]
    public int? DurationMinutes { get; set; } = 15;

    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? Reason { get; set; }
}