namespace MyShop.Domain.BusinessRules.Customer;
public class CustomerMustBeActiveRule : BaseBusinessRule
{
    private readonly CustomerStatus _status;

    public CustomerMustBeActiveRule(CustomerStatus status)
    {
        _status = status;
    }

    public override bool IsBroken()
    {
        return _status != CustomerStatus.Active;
    }

    public override string Message => "Customer must be active to perform this operation.";
}
