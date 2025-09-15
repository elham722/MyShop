namespace MyShop.Domain.Shared.ValueObjects.Identity;

public class SecurityInfo : BaseValueObject
{
    public bool TwoFactorEnabled { get; private init; }
    public string? TwoFactorSecret { get; private init; }
    public DateTime? TwoFactorEnabledAt { get; private init; }
    public string? SecurityQuestion { get; private init; }
    public string? SecurityAnswer { get; private init; }
    public DateTime? LastSecurityUpdate { get; private init; }

    private SecurityInfo() { }

    private SecurityInfo(bool twoFactorEnabled, string? twoFactorSecret, DateTime? twoFactorEnabledAt, string? question, string? answer, DateTime? lastUpdate)
    {
        TwoFactorEnabled = twoFactorEnabled;
        TwoFactorSecret = twoFactorSecret;
        TwoFactorEnabledAt = twoFactorEnabledAt;
        SecurityQuestion = question;
        SecurityAnswer = answer;
        LastSecurityUpdate = lastUpdate;
    }

    public static SecurityInfo Create() => new(false, null, null, null, null, null);

    public SecurityInfo EnableTwoFactor(string secret, IDateTimeService dateTimeService) =>
        new(true, secret, dateTimeService.UtcNow, SecurityQuestion, SecurityAnswer, dateTimeService.UtcNow);

    public SecurityInfo DisableTwoFactor(IDateTimeService dateTimeService) =>
        new(false, null, null, SecurityQuestion, SecurityAnswer, dateTimeService.UtcNow);

    public SecurityInfo UpdateTwoFactorSecret(string newSecret, IDateTimeService dateTimeService)
    {
        if (!TwoFactorEnabled) throw new CustomValidationException("Two-factor is not enabled");
        return new(TwoFactorEnabled, newSecret, TwoFactorEnabledAt, SecurityQuestion, SecurityAnswer, dateTimeService.UtcNow);
    }

    public bool ValidateTwoFactorSecret(string secret) => TwoFactorEnabled && TwoFactorSecret == secret;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return TwoFactorEnabled;
        yield return TwoFactorSecret;
        yield return TwoFactorEnabledAt;
        yield return SecurityQuestion;
        yield return SecurityAnswer;
        yield return LastSecurityUpdate;
    }
}
