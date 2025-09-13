namespace MyShop.Domain.BusinessRules.Customer;
public class CustomerMustBeAtLeastThirteenYearsOldRule : BaseBusinessRule
{
    private readonly DateTime? _dateOfBirth;

    public CustomerMustBeAtLeastThirteenYearsOldRule(DateTime? dateOfBirth)
    {
        _dateOfBirth = dateOfBirth;
    }

    public override bool IsBroken()
    {
        if (!_dateOfBirth.HasValue)
            return false;

        var age = DateTime.UtcNow.Year - _dateOfBirth.Value.Year;
        if (_dateOfBirth.Value.Date > DateTime.UtcNow.AddYears(-age))
            age--;

        return age < 13;
    }

    public override string Message => "Customer must be at least 13 years old.";
}
