namespace MyShop.Domain.Shared.Shared;
  public static class Guard
{
    public static void AgainstNullOrEmpty(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new CustomValidationException($"{name} cannot be empty.");
    }

    public static void Against(bool condition, string message)
    {
        if (condition) throw new DomainException(message);
    }

    public static void AgainstNull<T>(T? value, string name) where T : class
    {
        if (value == null) throw new CustomValidationException($"{name} cannot be null.");
    }

    public static void AgainstEmpty<T>(T value, string name) where T : struct
    {
        if (value.Equals(default(T))) throw new CustomValidationException($"{name} cannot be empty.");
    }

    public static void AgainstEmpty(Guid value, string name)
    {
        if (value == Guid.Empty) throw new CustomValidationException($"{name} cannot be empty.");
    }

    public static void AgainstNegative(int value, string name)
    {
        if (value < 0) throw new CustomValidationException($"{name} cannot be negative.");
    }

    public static void AgainstNegative(double value, string name)
    {
        if (value < 0) throw new CustomValidationException($"{name} cannot be negative.");
    }

    public static void AgainstNegative(TimeSpan value, string name)
    {
        if (value < TimeSpan.Zero) throw new CustomValidationException($"{name} cannot be negative.");
    }

    public static void AgainstOutOfRange(int value, int min, int max, string name)
    {
        if (value < min || value > max)
            throw new CustomValidationException($"{name} must be between {min} and {max}.");
    }

    public static void AgainstOutOfRange(double value, double min, double max, string name)
    {
        if (value < min || value > max)
            throw new CustomValidationException($"{name} must be between {min} and {max}.");
    }

    public static void AgainstInvalidFormat(string value, string pattern, string name)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(value, pattern))
            throw new CustomValidationException($"{name} has invalid format.");
    }

    public static void AgainstInvalidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
                throw new CustomValidationException("Invalid email format.");
        }
        catch
        {
            throw new CustomValidationException("Invalid email format.");
        }
    }

    public static void AgainstInvalidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 10)
            throw new CustomValidationException("Invalid phone number format.");
    }

    public static void AgainstInvalidDate(DateTime date, string name)
    {
        if (date == default)
            throw new CustomValidationException($"{name} cannot be default date.");
    }

    public static void AgainstFutureDate(DateTime date, string name)
    {
        if (date > DateTime.UtcNow)
            throw new CustomValidationException($"{name} cannot be in the future.");
    }

    public static void AgainstPastDate(DateTime date, string name)
    {
        if (date < DateTime.UtcNow)
            throw new CustomValidationException($"{name} cannot be in the past.");
    }

    #region Business Logic Guards

    public static void AgainstInvalidOperation(string operation, string reason)
    {
        throw new InvalidDomainOperationException($"Invalid operation '{operation}': {reason}", "Unknown", null, operation);
    }

    public static void AgainstInvalidOperation(string operation, string domainObjectType, string reason)
    {
        throw new InvalidDomainOperationException($"Invalid operation '{operation}' on {domainObjectType}: {reason}", domainObjectType, null, operation);
    }

    public static void AgainstEntityNotFound(string entityName, object key)
    {
        throw new NotFoundException(entityName, key);
    }

    public static void AgainstEntityNotFound(string message)
    {
        throw new NotFoundException(message);
    }

    #endregion

    #region Collection Guards

    public static void AgainstEmptyCollection<T>(IEnumerable<T> collection, string name)
    {
        if (collection == null || !collection.Any())
            throw new CustomValidationException($"{name} cannot be empty.");
    }

    public static void AgainstNullCollection<T>(IEnumerable<T> collection, string name)
    {
        if (collection == null)
            throw new CustomValidationException($"{name} cannot be null.");
    }

    #endregion

    #region String Guards

    public static void AgainstTooLong(string value, int maxLength, string name)
    {
        if (value != null && value.Length > maxLength)
            throw new CustomValidationException($"{name} cannot exceed {maxLength} characters.");
    }

    public static void AgainstTooShort(string value, int minLength, string name)
    {
        if (value != null && value.Length < minLength)
            throw new CustomValidationException($"{name} must be at least {minLength} characters long.");
    }

    #endregion
}

