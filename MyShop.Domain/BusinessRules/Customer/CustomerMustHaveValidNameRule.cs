namespace MyShop.Domain.BusinessRules.Customer;
public class CustomerMustHaveValidNameRule : BaseBusinessRule
{
    private readonly string _firstName;
    private readonly string _lastName;

    public CustomerMustHaveValidNameRule(string firstName, string lastName)
    {
        Guard.AgainstNull(firstName, nameof(firstName));
        Guard.AgainstNull(lastName, nameof(lastName));
        _firstName = firstName;
        _lastName = lastName;
    }

    public override bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(_firstName) ||
               string.IsNullOrWhiteSpace(_lastName) ||
               _firstName.Length < 2 ||
               _lastName.Length < 2 ||
               _firstName.Length > 50 ||
               _lastName.Length > 50;
    }

    public override string Message => "Customer must have a valid first name and last name (2-50 characters each).";
}