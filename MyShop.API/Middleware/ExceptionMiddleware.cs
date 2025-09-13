using System.Net;
using System.Text.Json;
using MyShop.API.Common;
using MyShop.Contracts.Common;
using MyShop.Domain.Exceptions.Validation;
using MyShop.Domain.Exceptions.Bussiness;
using MyShop.Domain.Exceptions.Common;
using MyShop.Domain.Exceptions.Persistence;

namespace MyShop.API.Middleware;

/// <summary>
/// Middleware for handling exceptions globally
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log domain exceptions with appropriate level
            if (ex is DomainException or CustomValidationException or BusinessRuleViolationException or 
                NotFoundException or InvalidDomainOperationException or ConcurrencyException)
            {
                LogDomainException(ex);
            }
            else
            {
                _logger.LogError(ex, "An unhandled exception occurred");
            }
            
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var traceId = context.TraceIdentifier;
        var response = new ApiResponse();

        switch (exception)
        {
            // Domain Exceptions (specific first, then base)
            case CustomValidationException validationEx:
                response = ApiResponse.Failure(validationEx.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case BusinessRuleViolationException businessEx:
                response = ApiResponse.Failure(businessEx.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case NotFoundException notFoundEx:
                response = ApiResponse.Failure(notFoundEx.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            case InvalidDomainOperationException invalidOpEx:
                response = ApiResponse.Failure(invalidOpEx.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case ConcurrencyException concurrencyEx:
                response = ApiResponse.Failure(concurrencyEx.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;

            case DomainException domainEx:
                response = ApiResponse.Failure(domainEx.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            // System Exceptions
            case ArgumentNullException:
            case ArgumentException:
                response = ApiResponse.Failure($"Invalid argument: {exception.Message}", traceId);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case UnauthorizedAccessException:
                response = ApiResponse.Failure("Unauthorized access", traceId);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;

            case KeyNotFoundException:
                response = ApiResponse.Failure("Resource not found", traceId);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            case InvalidOperationException:
                response = ApiResponse.Failure($"Invalid operation: {exception.Message}", traceId);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case TimeoutException:
                response = ApiResponse.Failure("Request timeout", traceId);
                context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                break;

            case NotImplementedException:
                response = ApiResponse.Failure("Feature not implemented", traceId);
                context.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
                break;

            default:
                var message = _environment.IsDevelopment() 
                    ? exception.Message 
                    : "An internal server error occurred";
                
                response = ApiResponse.Failure(message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    /// <summary>
    /// Logs domain exceptions with appropriate level
    /// </summary>
    /// <param name="exception">The domain exception</param>
    private void LogDomainException(Exception exception)
    {
        switch (exception)
        {
            case CustomValidationException:
            case BusinessRuleViolationException:
            case InvalidDomainOperationException:
                _logger.LogWarning(exception, "Domain exception occurred: {Message}", exception.Message);
                break;
            case NotFoundException:
                _logger.LogInformation(exception, "Resource not found: {Message}", exception.Message);
                break;
            case ConcurrencyException:
                _logger.LogWarning(exception, "Concurrency conflict: {Message}", exception.Message);
                break;
            case DomainException:
                _logger.LogWarning(exception, "General domain exception: {Message}", exception.Message);
                break;
            default:
                _logger.LogError(exception, "Unexpected domain exception: {Message}", exception.Message);
                break;
        }
    }
}

/// <summary>
/// Extension methods for registering the exception middleware
/// </summary>
public static class ExceptionMiddlewareExtensions
{
    /// <summary>
    /// Registers the exception handling middleware
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder</returns>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}