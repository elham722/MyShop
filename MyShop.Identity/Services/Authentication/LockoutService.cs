using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyShop.Contracts.Common;
using MyShop.Contracts.DTOs.Identity.Authentication.LockUser;
using MyShop.Contracts.DTOs.Options;
using MyShop.Contracts.Identity.Services;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.Authentication;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services.Authentication;

public class LockoutService : ILockoutService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _auditService;
    private readonly LockoutOptionsDto _lockoutOptions;
    private readonly IUserContextService _userContextService;

    public LockoutService(
        UserManager<ApplicationUser> userManager,
        IAuditService auditService,
        IUserContextService userContextService,
        IOptions<LockoutOptionsDto> lockoutOptions)
    {
        _userManager = userManager;
        _auditService = auditService;
        _userContextService = userContextService;
        _lockoutOptions = lockoutOptions.Value;
    }

    public async Task<Result<LockUserResponseDto>> LockUserAsync(LockUserRequestDto request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return Result<LockUserResponseDto>.Failure("User not found");

        if (user.IsLocked)
            return Result<LockUserResponseDto>.Failure("User is already locked");

        var lockoutDuration = request.DurationMinutes ?? _lockoutOptions.DefaultDurationMinutes;
        if (lockoutDuration > _lockoutOptions.MaxDurationMinutes)
            lockoutDuration = _lockoutOptions.MaxDurationMinutes;
        if (lockoutDuration < _lockoutOptions.MinDurationMinutes)
            lockoutDuration = _lockoutOptions.MinDurationMinutes;

        var lockoutEnd = DateTime.UtcNow.AddMinutes(lockoutDuration);

        var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        var lockedBy = _userContextService.GetCurrentUserName() ?? "System";
        var userIp = _userContextService.GetCurrentUserIpAddress();
        var deviceInfo = _userContextService.GetCurrentDeviceInfo();

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            await _auditService.LogUserActionAsync(
                request.UserId,
                "UserLocked",
                "User",
                request.UserId,
                additionalData: $"Reason: {request.Reason}, LockedBy: {lockedBy}, IP: {userIp}, Device: {deviceInfo}",
                errorMessage: errorMessage,
                isSuccess: false
            );

            return Result<LockUserResponseDto>.Failure(errorMessage);
        }

        await _auditService.LogUserActionAsync(
            request.UserId,
            "UserLocked",
            "User",
            request.UserId,
            additionalData: $"DurationMinutes: {lockoutDuration}, Reason: {request.Reason}, LockedBy: {lockedBy}, IP: {userIp}, Device: {deviceInfo}",
            isSuccess: true
        );

        return Result<LockUserResponseDto>.Success(new LockUserResponseDto
        {
            LockoutEnd = lockoutEnd,
            DurationMinutes = lockoutDuration,
            Reason = request.Reason
        });
    }

    public async Task<Result<UnlockUserResponseDto>> UnlockUserAsync(UnlockUserRequestDto request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return Result<UnlockUserResponseDto>.Failure("User not found");

        if (!user.IsLocked && !_lockoutOptions.EnableIdempotentUnlock)
            return Result<UnlockUserResponseDto>.Failure("User is not locked");

        var result = await _userManager.SetLockoutEndDateAsync(user, null);
        var unlockedBy = _userContextService.GetCurrentUserName() ?? "System";
        var userIp = _userContextService.GetCurrentUserIpAddress();
        var deviceInfo = _userContextService.GetCurrentDeviceInfo();

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            await _auditService.LogUserActionAsync(
                request.UserId,
                "UserUnlocked",
                "User",
                request.UserId,
                additionalData: $"Reason: {request.Reason}, UnlockedBy: {unlockedBy}, IP: {userIp}, Device: {deviceInfo}",
                errorMessage: errorMessage,
                isSuccess: false
            );

            return Result<UnlockUserResponseDto>.Failure(errorMessage);
        }

        await _auditService.LogUserActionAsync(
            request.UserId,
            "UserUnlocked",
            "User",
            request.UserId,
            additionalData: $"Reason: {request.Reason}, UnlockedBy: {unlockedBy}, IP: {userIp}, Device: {deviceInfo}",
            isSuccess: true
        );

        return Result<UnlockUserResponseDto>.Success(new UnlockUserResponseDto
        {
            Reason = request.Reason,
            UnlockedAt = DateTime.UtcNow
        });
    }

    public async Task<Result<LockoutStatusResponseDto>> GetLockoutStatusAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result<LockoutStatusResponseDto>.Failure("User not found");

        var remainingTime = GetRemainingLockoutTime(user);
        var isPermanent = user.LockoutEnd?.DateTime > DateTime.UtcNow.AddDays(_lockoutOptions.PermanentLockThresholdDays);
        var lockedBy = _userContextService.GetCurrentUserName() ?? "System";

        var status = new LockoutStatusResponseDto
        {
            IsLocked = user.IsLocked,
            LockoutEnd = user.LockoutEnd?.DateTime,
            RemainingTime = remainingTime,
            FailedAttempts = user.AccessFailedCount,
            MaxAttempts = _lockoutOptions.MaxFailedAttempts,
            IsPermanent = isPermanent,
            LockedAt = user.LockoutEnd?.DateTime,
            LockedBy = lockedBy
        };

        return Result<LockoutStatusResponseDto>.Success(status);
    }

    public async Task<Result<TimeSpan?>> GetLockoutEndTimeAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user?.LockoutEnd == null)
            return Result<TimeSpan?>.Success(null);

        var remaining = user.LockoutEnd.Value - DateTime.UtcNow;
        return Result<TimeSpan?>.Success(remaining > TimeSpan.Zero ? remaining : null);
    }

    private TimeSpan? GetRemainingLockoutTime(ApplicationUser user)
    {
        if (user.LockoutEnd == null) return null;
        var remaining = user.LockoutEnd.Value - DateTime.UtcNow;
        return remaining > TimeSpan.Zero ? remaining : null;
    }
}
