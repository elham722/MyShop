namespace MyShop.Domain.BusinessRules.Customer;
public class CustomerMustHaveValidAddressRule : BaseBusinessRule
{
    private readonly Address? _address;

    public CustomerMustHaveValidAddressRule(Address? address)
    {
        _address = address;
    }

    public override bool IsBroken()
    {
        return _address != null && !_address.IsComplete;
    }

    public override string Message => "Customer address must be complete (country, province, city, district, street, and postal code).";
}