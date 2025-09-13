namespace MyShop.Domain.BusinessRules.Customer;
public class CustomerMustHaveContactInfoRule : BaseBusinessRule
{
    private readonly Email? _email;
    private readonly PhoneNumber? _mobileNumber;

    public CustomerMustHaveContactInfoRule(Email? email, PhoneNumber? mobileNumber)
    {
        _email = email;
        _mobileNumber = mobileNumber;
    }

    public override bool IsBroken()
    {
        return _email == null && _mobileNumber == null;
    }

    public override string Message => "Customer must have at least one contact method (email or mobile number).";
}