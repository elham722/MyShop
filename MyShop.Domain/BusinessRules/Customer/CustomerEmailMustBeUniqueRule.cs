namespace MyShop.Domain.BusinessRules.Customer;
public class CustomerEmailMustBeUniqueRule : BaseBusinessRule
{
    private readonly string _email;
    private readonly Func<string, Task<bool>> _emailExistsChecker;

    public CustomerEmailMustBeUniqueRule(string email, Func<string, Task<bool>> emailExistsChecker)
    {
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNull(emailExistsChecker, nameof(emailExistsChecker));
        _email = email;
        _emailExistsChecker = emailExistsChecker;
    }

    public override bool IsBroken()
    {
        return false;
    }

    public override async Task<bool> IsBrokenAsync()
    {
        var emailExists = await _emailExistsChecker(_email);
        return emailExists;
    }

    public override string Message => $"Email '{_email}' is already registered.";
}