namespace MyShop.Domain.ValueObjects.Customer;
public class Email : BaseValueObject
{

    private static readonly Regex EmailRegex = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);


    private static readonly string[] PersonalDomains = { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "live.com", "msn.com" };
    private static readonly string[] CorporateKeywords = { "corp", "company", "business", "enterprise", "inc", "ltd", "llc" };
    private static readonly string[] EducationalKeywords = { "edu", "ac", "school", "university", "college", "institute" };
    private static readonly string[] GovernmentKeywords = { "gov", "government", "state", "municipal", "city" };
    private static readonly string[] DisposableDomains = { "10minutemail.com", "guerrillamail.com", "tempmail.org" };

    public string Value { get; private set; } = null!;

    private Email() { }

    public Email(string value)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));

        if (!IsValidEmail(value))
            throw new CustomValidationException("Invalid email format");

        Value = value.Trim().ToLowerInvariant();
    }

    public static Email Create(string value)
    {
        return new Email(value);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            return EmailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }


    public string GetDomain()
    {
        var parts = Value.Split('@');
        return parts.Length == 2 ? parts[1] : string.Empty;
    }

    public string GetUsername()
    {
        var parts = Value.Split('@');
        return parts.Length == 2 ? parts[0] : string.Empty;
    }

    public bool IsBusinessEmail()
    {
        var domain = GetDomain();
        return !PersonalDomains.Contains(domain.ToLowerInvariant());
    }

    public bool IsCorporateEmail()
    {
        var domain = GetDomain();
        return CorporateKeywords.Any(keyword => domain.ToLowerInvariant().Contains(keyword));
    }

    public bool IsEducationalEmail()
    {
        var domain = GetDomain();
        return EducationalKeywords.Any(keyword => domain.ToLowerInvariant().Contains(keyword));
    }

    public bool IsGovernmentEmail()
    {
        var domain = GetDomain();
        return GovernmentKeywords.Any(keyword => domain.ToLowerInvariant().Contains(keyword));
    }

    public Email GetDisposableEmail()
    {
        var username = GetUsername();
        return new Email($"{username}+disposable@{GetDomain()}");
    }

    public string GetDisposableEmailString()
    {
        var username = GetUsername();
        return $"{username}+disposable@{GetDomain()}";
    }

    public bool IsDisposableEmail()
    {
        var domain = GetDomain();
        return DisposableDomains.Contains(domain.ToLowerInvariant());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}