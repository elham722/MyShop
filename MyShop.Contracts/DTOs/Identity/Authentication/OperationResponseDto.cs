namespace MyShop.Contracts.DTOs.Identity.Authentication;

/// <summary>
/// Generic DTO for operation responses (success/failure)
/// </summary>
public class OperationResponseDto
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? ErrorCode { get; set; }
    public Dictionary<string, object>? AdditionalData { get; set; }
}