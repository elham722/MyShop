namespace MyShop.Contracts.DTOs.Identity.Authentication.LockUser;

/// <summary>
/// DTO for lockout status response
/// </summary>
public class LockoutStatusResponseDto
{
    public bool IsLocked { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public TimeSpan? RemainingTime { get; set; }
    public string? Reason { get; set; }
    public int FailedAttempts { get; set; }
    public int MaxAttempts { get; set; }
}