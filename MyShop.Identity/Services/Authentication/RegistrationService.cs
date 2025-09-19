using Mapster;
using Microsoft.AspNetCore.Identity;
using MyShop.Contracts.Common;
using MyShop.Contracts.DTOs.Identity;
using MyShop.Contracts.DTOs.Identity.Authentication.Email;
using MyShop.Contracts.DTOs.Identity.Authentication.Register;
using MyShop.Contracts.Identity.Services;
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
    private readonly IUserContextService _userContextService;

    public RegistrationService(
        UserManager<ApplicationUser> userManager, 
        IAuditService auditService,
        IUserContextService userContextService)
    {
        _userManager = userManager;
        _auditService = auditService;
        _userContextService = userContextService;
    }

    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request, string? ipAddress = null, string? userAgent = null)
    {
        try
        {
            // Get context information if not provided
            ipAddress ??= _userContextService.GetCurrentUserIpAddress();
            userAgent ??= _userContextService.GetCurrentUserAgent();

            // Validate request
            if (request.Password != request.ConfirmPassword)
            {
                return Result<RegisterResponseDto>.Failure("Password and confirm password do not match", "PASSWORD_MISMATCH");
            }

            if (!request.AcceptTerms)
            {
                return Result<RegisterResponseDto>.Failure("You must accept the terms and conditions", "TERMS_NOT_ACCEPTED");
            }

            // Check if email already exists
            var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserByEmail != null)
            {
                await _auditService.LogUserActionAsync(
                    "", 
                    "UserRegistration", 
                    "User", 
                    "",
                    additionalData: $"Email: {request.Email}, IP: {ipAddress}, UserAgent: {userAgent}",
                    isSuccess: false,
                    errorMessage: "Email already exists"
                );
                
                return Result<RegisterResponseDto>.Failure("Email is already registered", "EMAIL_EXISTS");
            }

            // Check if username already exists
            var existingUserByUsername = await _userManager.FindByNameAsync(request.UserName);
            if (existingUserByUsername != null)
            {
                await _auditService.LogUserActionAsync(
                    "", 
                    "UserRegistration", 
                    "User", 
                    "",
                    additionalData: $"Username: {request.UserName}, IP: {ipAddress}, UserAgent: {userAgent}",
                    isSuccess: false,
                    errorMessage: "Username already exists"
                );
                
                return Result<RegisterResponseDto>.Failure("Username is already taken", "USERNAME_EXISTS");
            }

            // Create user
            var user = ApplicationUser.Create(request.Email, request.UserName, request.CustomerId, "System");
            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                await _auditService.LogUserActionAsync(
                    "", 
                    "UserRegistration", 
                    "User", 
                    "",
                    additionalData: $"Email: {request.Email}, Username: {request.UserName}, IP: {ipAddress}, UserAgent: {userAgent}",
                    isSuccess: false,
                    errorMessage: errorMessage
                );
                
                return Result<RegisterResponseDto>.Failure(errorMessage, "REGISTRATION_FAILED");
            }

            // Assign default role
            var roleResult = await _userManager.AddToRoleAsync(user, RoleConstants.User.Customer);
            if (!roleResult.Succeeded)
            {
                // Log role assignment failure but don't fail registration
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "RoleAssignment", 
                    "User", 
                    user.Id,
                    additionalData: $"Role: {RoleConstants.User.Customer}, IP: {ipAddress}, UserAgent: {userAgent}",
                    isSuccess: false,
                    errorMessage: string.Join(", ", roleResult.Errors.Select(e => e.Description))
                );
            }

            // Generate email confirmation token
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            await _auditService.LogUserActionAsync(
                user.Id, 
                "UserRegistration", 
                "User", 
                user.Id,
                additionalData: $"Email: {request.Email}, Username: {request.UserName}, IP: {ipAddress}, UserAgent: {userAgent}, EmailTokenGenerated: true",
                isSuccess: true
            );

            // TODO: Send email confirmation email
            // await _emailService.SendEmailConfirmationAsync(user.Email, emailToken);

            var userDto = user.Adapt<ApplicationUserDto>();
            var response = new RegisterResponseDto
            {
                IsSuccess = true,
                User = userDto,
                RequiresEmailConfirmation = true,
                EmailConfirmationToken = emailToken, // In production, don't return token in response
                CreatedAt = DateTime.UtcNow
            };

            return Result<RegisterResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                "", 
                "UserRegistration", 
                "User", 
                "",
                additionalData: $"Email: {request.Email}, Username: {request.UserName}, IP: {ipAddress}, UserAgent: {userAgent}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result<RegisterResponseDto>.Failure("An error occurred during registration", "INTERNAL_ERROR");
        }
    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                await _auditService.LogUserActionAsync(
                    request.UserId, 
                    "EmailConfirmation", 
                    "User", 
                    request.UserId,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User not found"
                );
                
                return Result.Failure("User not found", "USER_NOT_FOUND");
            }

            // Check if email is already confirmed
            if (user.EmailConfirmed)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "EmailConfirmation", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Email already confirmed"
                );
                
                return Result.Failure("Email is already confirmed", "EMAIL_ALREADY_CONFIRMED");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "EmailConfirmation", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is inactive"
                );
                
                return Result.Failure("User account is inactive", "ACCOUNT_INACTIVE");
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            
            if (result.Succeeded)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "EmailConfirmation", 
                    "User", 
                    user.Id,
                    additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}, EmailConfirmed: true",
                    isSuccess: true
                );
                
                return Result.Success();
            }

            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            await _auditService.LogUserActionAsync(
                user.Id, 
                "EmailConfirmation", 
                "User", 
                user.Id,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: errorMessage
            );
            
            return Result.Failure(errorMessage, "EMAIL_CONFIRMATION_FAILED");
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                request.UserId, 
                "EmailConfirmation", 
                "User", 
                request.UserId,
                additionalData: $"IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result.Failure("An error occurred during email confirmation", "INTERNAL_ERROR");
        }
    }

    public async Task<Result> ResendEmailConfirmationAsync(ResendEmailConfirmationRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal if user exists or not for security reasons
                await _auditService.LogUserActionAsync(
                    "", 
                    "EmailConfirmationResent", 
                    "User", 
                    "",
                    additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User not found"
                );
                
                // Return success even if user doesn't exist for security
                return Result.Success();
            }

            // Check if email is already confirmed
            if (user.EmailConfirmed)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "EmailConfirmationResent", 
                    "User", 
                    user.Id,
                    additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "Email already confirmed"
                );
                
                return Result.Failure("Email is already confirmed", "EMAIL_ALREADY_CONFIRMED");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                await _auditService.LogUserActionAsync(
                    user.Id, 
                    "EmailConfirmationResent", 
                    "User", 
                    user.Id,
                    additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                    isSuccess: false,
                    errorMessage: "User account is inactive"
                );
                
                return Result.Failure("User account is inactive", "ACCOUNT_INACTIVE");
            }

            // Generate new email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            // TODO: Send email confirmation email
            // await _emailService.SendEmailConfirmationAsync(user.Email, token);
            
            await _auditService.LogUserActionAsync(
                user.Id, 
                "EmailConfirmationResent", 
                "User", 
                user.Id,
                additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}, TokenGenerated: true",
                isSuccess: true
            );
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            await _auditService.LogUserActionAsync(
                "", 
                "EmailConfirmationResent", 
                "User", 
                "",
                additionalData: $"Email: {request.Email}, IP: {_userContextService.GetCurrentUserIpAddress()}, UserAgent: {_userContextService.GetCurrentUserAgent()}",
                isSuccess: false,
                errorMessage: ex.Message
            );
            
            return Result.Failure("An error occurred while resending email confirmation", "INTERNAL_ERROR");
        }
    }
}