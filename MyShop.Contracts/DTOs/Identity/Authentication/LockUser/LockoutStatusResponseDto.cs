namespace MyShop.Contracts.DTOs.Identity.Authentication.LockUser;

public class LockoutStatusResponseDto
{
    public bool IsLocked { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public TimeSpan? RemainingTime { get; set; }
    public string? Reason { get; set; }
    public int FailedAttempts { get; set; }
    public int MaxAttempts { get; set; }
    public bool IsPermanent { get; set; }
    public DateTime? LockedAt { get; set; }
    public string? LockedBy { get; set; }
}