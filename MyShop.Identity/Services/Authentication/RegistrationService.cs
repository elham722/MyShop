using Mapster;
using Microsoft.AspNetCore.Identity;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.Authentication;
using MyShop.Identity.Constants;
using MyShop.Identity.Models;

namespace MyShop.Identity.Services.Authentication;

/// <summary>
/// Implementation of registration service
/// </summary>
public class RegistrationService : IRegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _auditService;

    public RegistrationService(UserManager<ApplicationUser> userManager, IAuditService auditService)
    {
        _userManager = userManager;
        _auditService = auditService;
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string userName, string password, 
        string? customerId = null, string? ipAddress = null, string? userAgent = null)
    {
        var user = ApplicationUser.Create(email, userName, customerId, "System");
        
        var result = await _userManager.CreateAsync(user, password);
        
        if (!result.Succeeded)
        {
            await _auditService.LogUserActionAsync("", "UserRegistration", "User", "", 
                ipAddress: ipAddress, userAgent: userAgent, isSuccess: false, 
                errorMessage: string.Join(", ", result.Errors.Select(e => e.Description)));
            
            return new AuthenticationResult 
            { 
                IsSuccess = false, 
                ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)) 
            };
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, RoleConstants.User.Customer);

        // Generate email confirmation token
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        await _auditService.LogUserActionAsync(user.Id, "UserRegistration", "User", user.Id, 
            ipAddress: ipAddress, userAgent: userAgent, isSuccess: true);

        return new AuthenticationResult
        {
            IsSuccess = true,
            User = user.Adapt<ApplicationUserDto>(),
            RequiresEmailConfirmation = true
        };
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        
        await _auditService.LogUserActionAsync(userId, "EmailConfirmation", "User", userId, 
            isSuccess: result.Succeeded, errorMessage: result.Succeeded ? null : "Invalid token");
        
        return result.Succeeded;
    }

    public async Task<bool> ResendEmailConfirmationAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.EmailConfirmed) return false;

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        await _auditService.LogUserActionAsync(user.Id, "EmailConfirmationResent", "User", user.Id, 
            isSuccess: true);
        
        return true;
    }
}