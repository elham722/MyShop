namespace MyShop.Domain.BusinessRules.Customer;
public class CustomerPhoneMustBeUniqueRule : BaseBusinessRule
{
    private readonly string _phoneNumber;
    private readonly Func<string, Task<bool>> _phoneExistsChecker;

    public CustomerPhoneMustBeUniqueRule(string phoneNumber, Func<string, Task<bool>> phoneExistsChecker)
    {
        Guard.AgainstNull(phoneNumber, nameof(phoneNumber));
        Guard.AgainstNull(phoneExistsChecker, nameof(phoneExistsChecker));
        _phoneNumber = phoneNumber;
        _phoneExistsChecker = phoneExistsChecker;
    }

    public override bool IsBroken()
    {
        return false;
    }

    public override async Task<bool> IsBrokenAsync()
    {
        var phoneExists = await _phoneExistsChecker(_phoneNumber);
        return phoneExists;
    }

    public override string Message => $"Phone number '{_phoneNumber}' is already registered.";
}
