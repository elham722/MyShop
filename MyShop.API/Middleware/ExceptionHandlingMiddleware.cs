using System.Net;
using System.Text.Json;
using MyShop.Contracts.DTOs.Responses;
using MyShop.API.Services;

namespace MyShop.API.Middleware;

/// <summary>
/// Global exception handling middleware
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IExceptionMappingService _exceptionMappingService;

    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlingMiddleware> logger,
        IExceptionMappingService exceptionMappingService)
    {
        _next = next;
        _logger = logger;
        _exceptionMappingService = exceptionMappingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = GetCorrelationId(context);
        
        _logger.LogError(exception, 
            "Unhandled exception occurred for request {Method} {Path} with correlation ID {CorrelationId}", 
            context.Request.Method, 
            context.Request.Path, 
            correlationId);

        var (statusCode, apiResponse) = _exceptionMappingService.MapException(exception);
        
        // Add correlation ID to response
        apiResponse.WithCorrelationId(correlationId);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonResponse = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private static string GetCorrelationId(HttpContext context)
    {
        // Try to get correlation ID from headers
        if (context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            return correlationId.ToString();
        }

        // Generate new correlation ID
        return Guid.NewGuid().ToString();
    }
}

/// <summary>
/// Extension methods for exception handling middleware
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    /// <summary>
    /// Adds global exception handling middleware
    /// </summary>
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}