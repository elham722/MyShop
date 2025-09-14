using System.Net;
using MyShop.Contracts.DTOs.Responses;
using MyShop.Domain.Exceptions.Common;
using MyShop.Domain.Exceptions.Validation;
using MyShop.Domain.Exceptions.Bussiness;
using MyShop.Domain.Exceptions.Persistence;

namespace MyShop.API.Services;

/// <summary>
/// Service for mapping exceptions to appropriate HTTP responses
/// </summary>
public interface IExceptionMappingService
{
    /// <summary>
    /// Maps exception to HTTP status code and API response
    /// </summary>
    (HttpStatusCode statusCode, ApiResponse response) MapException(Exception exception);
}

/// <summary>
/// Implementation of exception mapping service
/// </summary>
public class ExceptionMappingService : IExceptionMappingService
{
    private readonly ILogger<ExceptionMappingService> _logger;

    public ExceptionMappingService(ILogger<ExceptionMappingService> logger)
    {
        _logger = logger;
    }

    public (HttpStatusCode statusCode, ApiResponse response) MapException(Exception exception)
    {
        _logger.LogDebug("Mapping exception of type {ExceptionType}: {Message}", 
            exception.GetType().Name, exception.Message);

        // Domain Exceptions
        if (exception is DomainException domainEx)
            return MapDomainException(domainEx);

        // Validation Exceptions
        if (exception is CustomValidationException validationEx)
            return MapValidationException(validationEx);

        // Business Rule Exceptions
        if (exception is BusinessRuleViolationException businessEx)
            return MapBusinessRuleException(businessEx);

        // Not Found Exceptions
        if (exception is NotFoundException notFoundEx)
            return MapNotFoundException(notFoundEx);

        // Invalid Operation Exceptions
        if (exception is InvalidDomainOperationException invalidOpEx)
            return MapInvalidOperationException(invalidOpEx);

        // System Exceptions
        if (exception is UnauthorizedAccessException)
            return MapUnauthorizedException();

        if (exception is ArgumentException argEx)
            return MapArgumentException(argEx);

        if (exception is ArgumentNullException argNullEx)
            return MapArgumentNullException(argNullEx);

        if (exception is KeyNotFoundException keyEx)
            return MapKeyNotFoundException(keyEx);

        if (exception is NotImplementedException notImplEx)
            return MapNotImplementedException(notImplEx);

        if (exception is TimeoutException timeoutEx)
            return MapTimeoutException(timeoutEx);

        // Default case
        return MapGenericException(exception);
    }

    private (HttpStatusCode, ApiResponse) MapDomainException(DomainException ex)
    {
        return (
            HttpStatusCode.BadRequest,
            ApiResponse.Fail(ex.Message, ex.ErrorCode)
        );
    }

    private (HttpStatusCode, ApiResponse) MapValidationException(CustomValidationException ex)
    {
        return (
            HttpStatusCode.BadRequest,
            ApiResponse.Fail(ex.Message, "VALIDATION_ERROR")
        );
    }

    private (HttpStatusCode, ApiResponse) MapBusinessRuleException(BusinessRuleViolationException ex)
    {
        return (
            HttpStatusCode.BadRequest,
            ApiResponse.Fail(ex.Message, "BUSINESS_RULE_VIOLATION")
        );
    }

    private (HttpStatusCode, ApiResponse) MapNotFoundException(NotFoundException ex)
    {
        return (
            HttpStatusCode.NotFound,
            ApiResponse.Fail(ex.Message, "NOT_FOUND")
        );
    }

    private (HttpStatusCode, ApiResponse) MapInvalidOperationException(InvalidDomainOperationException ex)
    {
        return (
            HttpStatusCode.BadRequest,
            ApiResponse.Fail(ex.Message, "INVALID_OPERATION")
        );
    }


    private (HttpStatusCode, ApiResponse) MapUnauthorizedException()
    {
        return (
            HttpStatusCode.Unauthorized,
            ApiResponse.Fail("Access denied", "UNAUTHORIZED")
        );
    }

    private (HttpStatusCode, ApiResponse) MapArgumentException(ArgumentException ex)
    {
        return (
            HttpStatusCode.BadRequest,
            ApiResponse.Fail(ex.Message, "INVALID_ARGUMENT")
        );
    }

    private (HttpStatusCode, ApiResponse) MapArgumentNullException(ArgumentNullException ex)
    {
        return (
            HttpStatusCode.BadRequest,
            ApiResponse.Fail(ex.Message, "NULL_ARGUMENT")
        );
    }

    private (HttpStatusCode, ApiResponse) MapKeyNotFoundException(KeyNotFoundException ex)
    {
        return (
            HttpStatusCode.NotFound,
            ApiResponse.Fail(ex.Message, "KEY_NOT_FOUND")
        );
    }

    private (HttpStatusCode, ApiResponse) MapNotImplementedException(NotImplementedException ex)
    {
        return (
            HttpStatusCode.NotImplemented,
            ApiResponse.Fail(ex.Message, "NOT_IMPLEMENTED")
        );
    }

    private (HttpStatusCode, ApiResponse) MapTimeoutException(TimeoutException ex)
    {
        return (
            HttpStatusCode.RequestTimeout,
            ApiResponse.Fail(ex.Message, "REQUEST_TIMEOUT")
        );
    }

    private (HttpStatusCode, ApiResponse) MapGenericException(Exception ex)
    {
        _logger.LogWarning("Unmapped exception type: {ExceptionType}", ex.GetType().Name);
        
        return (
            HttpStatusCode.InternalServerError,
            ApiResponse.Fail("An unexpected error occurred", "INTERNAL_SERVER_ERROR")
        );
    }
}