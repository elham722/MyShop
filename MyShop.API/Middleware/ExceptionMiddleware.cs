using System.Net;
using System.Text.Json;
using MyShop.API.Common;
using MyShop.Contracts.Common;

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
            _logger.LogError(ex, "An unhandled exception occurred");
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