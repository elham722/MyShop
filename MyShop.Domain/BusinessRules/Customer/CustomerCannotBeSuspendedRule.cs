namespace MyShop.Domain.BusinessRules.Customer;
public class CustomerCannotBeSuspendedRule : BaseBusinessRule
{
    private readonly CustomerStatus _status;

    public CustomerCannotBeSuspendedRule(CustomerStatus status)
    {
        _status = status;
    }

    public override bool IsBroken()
    {
        return _status == CustomerStatus.Suspended;
    }

    public override string Message => "This operation cannot be performed on a suspended customer.";
}
