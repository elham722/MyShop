namespace MyShop.Contracts.DTOs.Options;

/// <summary>
/// Configuration options for user lockout functionality
/// </summary>
public class LockoutOptionsDto
{
    /// <summary>
    /// Default lockout duration in minutes
    /// </summary>
    public int DefaultDurationMinutes { get; set; } = 15;

    /// <summary>
    /// Maximum failed login attempts before lockout
    /// </summary>
    public int MaxFailedAttempts { get; set; } = 5;

    /// <summary>
    /// Threshold in days to consider a lockout as permanent
    /// </summary>
    public int PermanentLockThresholdDays { get; set; } = 365;

    /// <summary>
    /// Maximum lockout duration in minutes (24 hours)
    /// </summary>
    public int MaxDurationMinutes { get; set; } = 1440;

    /// <summary>
    /// Minimum lockout duration in minutes
    /// </summary>
    public int MinDurationMinutes { get; set; } = 1;

    /// <summary>
    /// Whether to enable automatic unlock after duration expires
    /// </summary>
    public bool EnableAutoUnlock { get; set; } = true;

    /// <summary>
    /// Whether to enable idempotent unlock behavior (unlock already unlocked user)
    /// </summary>
    public bool EnableIdempotentUnlock { get; set; } = true;
}