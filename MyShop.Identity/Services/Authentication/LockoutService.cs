using Microsoft.AspNetCore.Identity;
using MyShop.Contracts.DTOs.Identity.Authentication;
using MyShop.Contracts.DTOs.Identity.Authentication.LockUser;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.Authentication;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services.Authentication;

public class LockoutService : ILockoutService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _auditService;

    public LockoutService(UserManager<ApplicationUser> userManager, IAuditService auditService)
    {
        _userManager = userManager;
        _auditService = auditService;
    }

    public async Task<OperationResponseDto> LockUserAsync(LockUserRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return CreateFailureResponse("User not found", "USER_NOT_FOUND");
            }

            // Check if user is already locked
            if (user.IsLocked)
            {
                return CreateFailureResponse("User is already locked", "USER_ALREADY_LOCKED");
            }

            // Calculate lockout duration
            var lockoutDuration = request.DurationMinutes.HasValue 
                ? TimeSpan.FromMinutes(request.DurationMinutes.Value)
                : TimeSpan.FromMinutes(15);

            var lockoutEnd = DateTime.UtcNow.Add(lockoutDuration);
            var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);

            if (result.Succeeded)
            {
                await _auditService.LogUserActionAsync(
                    request.UserId, 
                    "UserLocked", 
                    "User", 
                    request.UserId,
                    additionalData: $"DurationMinutes: {request.DurationMinutes ?? 15}, Reason: {request.Reason ?? "Manual lockout"}, LockoutEnd: {lockoutEnd:yyyy-MM-dd HH:mm:ss}",
                    isSuccess: true);

                return CreateSuccessResponse(
                    $"User locked successfully for {request.DurationMinutes ?? 15} minutes",
                    new Dictionary<string, object>
                    {
                        { "LockoutEnd", lockoutEnd },
                        { "DurationMinutes", request.DurationMinutes ?? 15 },
                        { "Reason", request.Reason }
                    });
            }

            await _auditService.LogUserActionAsync(
                request.UserId, 
                "UserLocked", 
                "User", 
                request.UserId,
                additionalData: $"DurationMinutes: {request.DurationMinutes ?? 15}, Reason: {request.Reason ?? "Manual lockout"}",
                errorMessage: string.Join(", ", result.Errors.Select(e => e.Description)),
                isSuccess: false);

            return CreateFailureResponse(
                "Failed to lock user: " + string.Join(", ", result.Errors.Select(e => e.Description)),
                "LOCKOUT_FAILED");
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                request.UserId, 
                "UserLocked", 
                "User", 
                request.UserId,
                additionalData: $"DurationMinutes: {request.DurationMinutes ?? 15}, Reason: {request.Reason ?? "Manual lockout"}",
                errorMessage: ex.Message,
                isSuccess: false);

            return CreateFailureResponse("An error occurred while locking the user", "INTERNAL_ERROR");
        }
    }

    public async Task<OperationResponseDto> UnlockUserAsync(UnlockUserRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return CreateFailureResponse("User not found", "USER_NOT_FOUND");
            }

            // Check if user is actually locked
            if (!user.IsLocked)
            {
                return CreateFailureResponse("User is not locked", "USER_NOT_LOCKED");
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null);

            if (result.Succeeded)
            {
                await _auditService.LogUserActionAsync(
                    request.UserId, 
                    "UserUnlocked", 
                    "User", 
                    request.UserId,
                    additionalData: $"Reason: {request.Reason ?? "Manual unlock"}",
                    isSuccess: true);

                return CreateSuccessResponse(
                    "User unlocked successfully",
                    new Dictionary<string, object>
                    {
                        { "Reason", request.Reason }
                    });
            }

            await _auditService.LogUserActionAsync(
                request.UserId, 
                "UserUnlocked", 
                "User", 
                request.UserId,
                additionalData: $"Reason: {request.Reason ?? "Manual unlock"}",
                errorMessage: string.Join(", ", result.Errors.Select(e => e.Description)),
                isSuccess: false);

            return CreateFailureResponse(
                "Failed to unlock user: " + string.Join(", ", result.Errors.Select(e => e.Description)),
                "UNLOCK_FAILED");
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                request.UserId, 
                "UserUnlocked", 
                "User", 
                request.UserId,
                additionalData: $"Reason: {request.Reason ?? "Manual unlock"}",
                errorMessage: ex.Message,
                isSuccess: false);

            return CreateFailureResponse("An error occurred while unlocking the user", "INTERNAL_ERROR");
        }
    }

    public async Task<LockoutStatusResponseDto> GetLockoutStatusAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new LockoutStatusResponseDto
                {
                    IsLocked = false,
                    FailedAttempts = 0,
                    MaxAttempts = 5, // Default max attempts
                    IsPermanent = false
                };
            }

            var remainingTime = GetRemainingLockoutTime(user);
            var isPermanent = user.LockoutEnd?.DateTime > DateTime.UtcNow.AddYears(1);

            return new LockoutStatusResponseDto
            {
                IsLocked = user.IsLocked,
                LockoutEnd = user.LockoutEnd?.DateTime,
                RemainingTime = remainingTime,
                FailedAttempts = user.AccessFailedCount,
                MaxAttempts = 5, // This should come from configuration
                IsPermanent = isPermanent,
                LockedAt = user.LockoutEnd?.DateTime,
                LockedBy = "System" // This should be tracked in the user entity
            };
        }
        catch (Exception)
        {
            return new LockoutStatusResponseDto
            {
                IsLocked = false,
                FailedAttempts = 0,
                MaxAttempts = 5,
                IsPermanent = false
            };
        }
    }

    public async Task<TimeSpan?> GetLockoutEndTimeAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user?.LockoutEnd == null) return null;

            var remaining = user.LockoutEnd.Value - DateTime.UtcNow;
            return remaining > TimeSpan.Zero ? remaining : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private TimeSpan? GetRemainingLockoutTime(ApplicationUser user)
    {
        if (user.LockoutEnd == null) return null;

        var remaining = user.LockoutEnd.Value - DateTime.UtcNow;
        return remaining > TimeSpan.Zero ? remaining : null;
    }

    private OperationResponseDto CreateSuccessResponse(string message, Dictionary<string, object>? additionalData = null)
    {
        return new OperationResponseDto
        {
            IsSuccess = true,
            Message = message,
            Timestamp = DateTime.UtcNow,
            AdditionalData = additionalData
        };
    }

    private OperationResponseDto CreateFailureResponse(string errorMessage, string errorCode)
    {
        return new OperationResponseDto
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode,
            Timestamp = DateTime.UtcNow
        };
    }
}