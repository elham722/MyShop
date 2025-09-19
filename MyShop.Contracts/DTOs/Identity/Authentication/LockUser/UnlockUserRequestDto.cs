using System.ComponentModel.DataAnnotations;

namespace MyShop.Contracts.DTOs.Identity.Authentication.LockUser;

public class UnlockUserRequestDto
{
    [Required(ErrorMessage = "User ID is required")]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? Reason { get; set; }
}