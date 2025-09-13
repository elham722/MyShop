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

        var today = DateTime.UtcNow.Date;
        var birthDate = _dateOfBirth.Value.Date;
        var age = today.Year - birthDate.Year;
        
        // Subtract a year if the birthday hasn't occurred this year
        if (birthDate > today.AddYears(-age))
            age--;

        return age < 13;
    }

    public override string Message => "Customer must be at least 13 years old.";
}
