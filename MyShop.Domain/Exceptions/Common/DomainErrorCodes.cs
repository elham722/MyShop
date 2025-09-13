namespace MyShop.Domain.Exceptions.Common;
public static class DomainErrorCodes
{
    public const string BusinessRuleViolation = "BUSINESS_RULE_VIOLATION";

    public const string EntityNotFound = "ENTITY_NOT_FOUND";

    public const string ValidationFailed = "VALIDATION_FAILED";

    public const string ConcurrencyConflict = "CONCURRENCY_CONFLICT";

    public const string InvalidDomainOperation = "INVALID_DOMAIN_OPERATION";

    public const string DomainError = "DOMAIN_ERROR";
}