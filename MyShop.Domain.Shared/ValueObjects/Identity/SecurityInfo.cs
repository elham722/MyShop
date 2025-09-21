namespace MyShop.Domain.Shared.ValueObjects.Identity;

public class SecurityInfo : BaseValueObject
{
    // حذف TwoFactorEnabled چون در IdentityUser موجود است
    public string? TwoFactorSecret { get; private init; }
    public DateTime? TwoFactorEnabledAt { get; private init; }
    public string? SecurityQuestion { get; private init; }
    public string? SecurityAnswer { get; private init; }
    public DateTime? LastSecurityUpdate { get; private init; }

    private SecurityInfo() { }

    private SecurityInfo(string? twoFactorSecret, DateTime? twoFactorEnabledAt, string? question, string? answer, DateTime? lastUpdate)
    {
        TwoFactorSecret = twoFactorSecret;
        TwoFactorEnabledAt = twoFactorEnabledAt;
        SecurityQuestion = question;
        SecurityAnswer = answer;
        LastSecurityUpdate = lastUpdate;
    }

    public static SecurityInfo Create() => new(null, null, null, null, null);

    public SecurityInfo EnableTwoFactor(string secret, IDateTimeService dateTimeService) =>
        new(secret, dateTimeService.UtcNow, SecurityQuestion, SecurityAnswer, dateTimeService.UtcNow);

    public SecurityInfo DisableTwoFactor(IDateTimeService dateTimeService) =>
        new(null, null, SecurityQuestion, SecurityAnswer, dateTimeService.UtcNow);

    public SecurityInfo UpdateTwoFactorSecret(string newSecret, IDateTimeService dateTimeService) =>
        new(newSecret, TwoFactorEnabledAt, SecurityQuestion, SecurityAnswer, dateTimeService.UtcNow);

    public SecurityInfo SetSecurityQuestion(string question, string answer, IDateTimeService dateTimeService) =>
        new(TwoFactorSecret, TwoFactorEnabledAt, question, answer, dateTimeService.UtcNow);

    public bool ValidateTwoFactorSecret(string secret) => TwoFactorSecret == secret;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return TwoFactorSecret;
        yield return TwoFactorEnabledAt;
        yield return SecurityQuestion;
        yield return SecurityAnswer;
        yield return LastSecurityUpdate;
    }
}
