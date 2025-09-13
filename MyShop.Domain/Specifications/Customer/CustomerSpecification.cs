namespace MyShop.Domain.Specifications.Customer;
public class CustomerSpecification : BaseSpecification<Entities.Customer.Customer>
{
    public CustomerSpecification() { }

    public CustomerSpecification(Expression<Func<Entities.Customer.Customer, bool>> criteria) : base(criteria) { }

    #region Static Factory Methods

    public static CustomerSpecification ById(Guid id)
    {
        return new CustomerSpecification(c => c.Id == id);
    }

    public static CustomerSpecification ByEmail(string email)
    {
        return new CustomerSpecification(c => c.Email != null && c.Email.Value == email);
    }

    public static CustomerSpecification ByMobileNumber(string mobileNumber)
    {
        return new CustomerSpecification(c => c.MobileNumber != null && c.MobileNumber.Value == mobileNumber);
    }

    public static CustomerSpecification ByFirstName(string firstName)
    {
        return new CustomerSpecification(c => c.FirstName.Contains(firstName));
    }

    public static CustomerSpecification ByLastName(string lastName)
    {
        return new CustomerSpecification(c => c.LastName.Contains(lastName));
    }

    public static CustomerSpecification ByFullName(string fullName)
    {
        return new CustomerSpecification(c => c.FullName.Contains(fullName));
    }

    public static CustomerSpecification ActiveCustomers()
    {
        return new CustomerSpecification(c => c.Status == CustomerStatus.Active);
    }

    public static CustomerSpecification InactiveCustomers()
    {
        return new CustomerSpecification(c => c.Status == CustomerStatus.Inactive);
    }

    public static CustomerSpecification SuspendedCustomers()
    {
        return new CustomerSpecification(c => c.Status == CustomerStatus.Suspended);
    }

    public static CustomerSpecification VerifiedCustomers()
    {
        return new CustomerSpecification(c => c.IsEmailVerified && c.IsPhoneVerified);
    }

    public static CustomerSpecification EmailVerifiedCustomers()
    {
        return new CustomerSpecification(c => c.IsEmailVerified);
    }

    public static CustomerSpecification PhoneVerifiedCustomers()
    {
        return new CustomerSpecification(c => c.IsPhoneVerified);
    }

    public static CustomerSpecification WithCompleteProfile()
    {
        return new CustomerSpecification(c => c.HasCompleteProfile);
    }

    public static CustomerSpecification WithCompleteAddress()
    {
        return new CustomerSpecification(c => c.HasCompleteAddress);
    }

    public static CustomerSpecification ByApplicationUserId(string applicationUserId)
    {
        return new CustomerSpecification(c => c.ApplicationUserId == applicationUserId);
    }

    public static CustomerSpecification CreatedAfter(DateTime date)
    {
        return new CustomerSpecification(c => c.CreatedAt >= date);
    }

    public static CustomerSpecification CreatedBefore(DateTime date)
    {
        return new CustomerSpecification(c => c.CreatedAt <= date);
    }

    public static CustomerSpecification CreatedBetween(DateTime startDate, DateTime endDate)
    {
        return new CustomerSpecification(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate);
    }

    public static CustomerSpecification LastLoginAfter(DateTime date)
    {
        return new CustomerSpecification(c => c.LastLoginAt.HasValue && c.LastLoginAt.Value >= date);
    }

    public static CustomerSpecification NeverLoggedIn()
    {
        return new CustomerSpecification(c => !c.LastLoginAt.HasValue);
    }

    public static CustomerSpecification ByAgeRange(int minAge, int maxAge)
    {
        var minDate = DateTime.UtcNow.AddYears(-maxAge);
        var maxDate = DateTime.UtcNow.AddYears(-minAge);
        return new CustomerSpecification(c => c.DateOfBirth.HasValue &&
                                            c.DateOfBirth.Value >= minDate &&
                                            c.DateOfBirth.Value <= maxDate);
    }

    public static CustomerSpecification ByCity(string city)
    {
        return new CustomerSpecification(c => c.Address != null && c.Address.City == city);
    }

    public static CustomerSpecification ByProvince(string province)
    {
        return new CustomerSpecification(c => c.Address != null && c.Address.Province == province);
    }

    public static CustomerSpecification ByCountry(string country)
    {
        return new CustomerSpecification(c => c.Address != null && c.Address.Country == country);
    }

    #endregion

    #region Search Methods

    public static CustomerSpecification SearchByName(string searchTerm)
    {
        return new CustomerSpecification(c =>
            c.FirstName.Contains(searchTerm) ||
            c.LastName.Contains(searchTerm) ||
            c.FullName.Contains(searchTerm));
    }

    public static CustomerSpecification SearchByContact(string searchTerm)
    {
        return new CustomerSpecification(c =>
            (c.Email != null && c.Email.Value.Contains(searchTerm)) ||
            (c.MobileNumber != null && c.MobileNumber.Value.Contains(searchTerm)));
    }

    public static CustomerSpecification SearchByAddress(string searchTerm)
    {
        return new CustomerSpecification(c =>
            c.Address != null && (
                c.Address.City.Contains(searchTerm) ||
                c.Address.Province.Contains(searchTerm) ||
                c.Address.Country.Contains(searchTerm) ||
                c.Address.Street.Contains(searchTerm) ||
                c.Address.PostalCode.Contains(searchTerm)
            ));
    }

    public static CustomerSpecification SearchByAll(string searchTerm)
    {
        return new CustomerSpecification(c =>
            c.FirstName.Contains(searchTerm) ||
            c.LastName.Contains(searchTerm) ||
            c.FullName.Contains(searchTerm) ||
            (c.Email != null && c.Email.Value.Contains(searchTerm)) ||
            (c.MobileNumber != null && c.MobileNumber.Value.Contains(searchTerm)) ||
            (c.Address != null && (
                c.Address.City.Contains(searchTerm) ||
                c.Address.Province.Contains(searchTerm) ||
                c.Address.Country.Contains(searchTerm) ||
                c.Address.Street.Contains(searchTerm) ||
                c.Address.PostalCode.Contains(searchTerm)
            )));
    }

    #endregion

    #region Composite Specifications

    public static CustomerSpecification ActiveVerifiedCustomers()
    {
        return (CustomerSpecification)ActiveCustomers().And(VerifiedCustomers());
    }

    public static CustomerSpecification ActiveCustomersWithCompleteProfile()
    {
        return (CustomerSpecification)ActiveCustomers().And(WithCompleteProfile());
    }

    public static CustomerSpecification RecentlyCreatedCustomers(int days = 30)
    {
        var date = DateTime.UtcNow.AddDays(-days);
        return CreatedAfter(date);
    }

    public static CustomerSpecification InactiveCustomersOlderThan(int days)
    {
        var date = DateTime.UtcNow.AddDays(-days);
        return (CustomerSpecification)InactiveCustomers().And(CreatedBefore(date));
    }

    #endregion

    #region Ordering Specifications

    public CustomerSpecification OrderByFirstName()
    {
        return (CustomerSpecification)AddOrderBy(c => c.FirstName);
    }

    public CustomerSpecification OrderByLastName()
    {
        return (CustomerSpecification)AddOrderBy(c => c.LastName);
    }

    public CustomerSpecification OrderByFullName()
    {
        return (CustomerSpecification)AddOrderBy(c => c.FullName);
    }

    public CustomerSpecification OrderByCreatedAt()
    {
        return (CustomerSpecification)AddOrderBy(c => c.CreatedAt);
    }

    public CustomerSpecification OrderByLastLoginAt()
    {
        return (CustomerSpecification)AddOrderBy(c => c.LastLoginAt ?? DateTime.MinValue);
    }

    public CustomerSpecification OrderByFirstNameDescending()
    {
        return (CustomerSpecification)AddOrderByDescending(c => c.FirstName);
    }

    public CustomerSpecification OrderByLastNameDescending()
    {
        return (CustomerSpecification)AddOrderByDescending(c => c.LastName);
    }

    public CustomerSpecification OrderByCreatedAtDescending()
    {
        return (CustomerSpecification)AddOrderByDescending(c => c.CreatedAt);
    }

    public CustomerSpecification OrderByLastLoginAtDescending()
    {
        return (CustomerSpecification)AddOrderByDescending(c => c.LastLoginAt ?? DateTime.MinValue);
    }

    #endregion

    #region Paging Specifications

    public CustomerSpecification WithPaging(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        return (CustomerSpecification)ApplyPaging(skip, pageSize);
    }

    #endregion

    #region Include Specifications

    public CustomerSpecification IncludeAddress()
    {
        return (CustomerSpecification)AddInclude(c => c.Address!);
    }

    public CustomerSpecification IncludeEmail()
    {
        return (CustomerSpecification)AddInclude(c => c.Email!);
    }

    public CustomerSpecification IncludeMobileNumber()
    {
        return (CustomerSpecification)AddInclude(c => c.MobileNumber!);
    }

    public CustomerSpecification IncludeAll()
    {
        return IncludeAddress().IncludeEmail().IncludeMobileNumber();
    }

    #endregion

    public override string Description => $"Customer specification with {CriteriaCount} criteria(s)";
}