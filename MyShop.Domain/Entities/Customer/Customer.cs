namespace MyShop.Domain.Entities.Customer;
public class Customer : BaseAggregateRoot<Guid>
{
    // Personal Information
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateTime? DateOfBirth { get; private set; }
    public Address? Address { get; private set; }

    // Contact Information
    public Email? Email { get; private set; }
    public PhoneNumber? MobileNumber { get; private set; }

    // Status Information
    public CustomerStatus Status { get; private set; } = CustomerStatus.Active;
    public DateTime? LastLoginAt { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public bool IsPhoneVerified { get; private set; }

    // External Integration
    public string? ApplicationUserId { get; private set; }

    // Computed Properties
    public string FullName => $"{FirstName} {LastName}".Trim();
    public bool IsActive => Status == CustomerStatus.Active;
    public bool IsVerified => IsEmailVerified && IsPhoneVerified;
    public bool HasCompleteProfile => !string.IsNullOrEmpty(FirstName) &&
                                     !string.IsNullOrEmpty(LastName) &&
                                     Email != null &&
                                     MobileNumber != null &&
                                     Address != null;
    public bool HasCompleteAddress => Address?.IsComplete ?? false;
    public int Age => DateOfBirth.HasValue
        ? DateTime.UtcNow.Year - DateOfBirth.Value.Year -
          (DateTime.UtcNow.Date < DateOfBirth.Value.AddYears(DateTime.UtcNow.Year - DateOfBirth.Value.Year) ? 1 : 0)
        : 0;

    #region Constructors

    private Customer() { } // For EF Core

    public Customer(string firstName, string lastName, string createdBy) : base()
    {
        SetPersonalInfo(firstName, lastName);
        MarkAsCreated(createdBy);
        AddDomainEvent(new CustomerCreatedEvent(Id, FirstName, LastName));
    }

    public Customer(Guid id, string firstName, string lastName, string createdBy) : base(id)
    {
        SetPersonalInfo(firstName, lastName);
        MarkAsCreated(createdBy);
        AddDomainEvent(new CustomerCreatedEvent(Id, FirstName, LastName));
    }

    #endregion

    #region Personal Information Methods

    public void UpdatePersonalInfo(string firstName, string lastName)
    {
        Guard.AgainstNullOrEmpty(firstName, nameof(firstName));
        Guard.AgainstNullOrEmpty(lastName, nameof(lastName));
        Guard.AgainstTooLong(firstName, 50, nameof(firstName));
        Guard.AgainstTooLong(lastName, 50, nameof(lastName));

        var oldFirstName = FirstName;
        var oldLastName = LastName;

        FirstName = firstName.Trim();
        LastName = lastName.Trim();

        MarkAsUpdated();
        AddDomainEvent(new CustomerPersonalInfoUpdatedEvent(Id, oldFirstName, oldLastName, FirstName, LastName));
    }

    public void SetDateOfBirth(DateTime dateOfBirth)
    {
        Guard.AgainstFutureDate(dateOfBirth, nameof(dateOfBirth));

        var minDate = DateTime.UtcNow.AddYears(-120);
        var maxDate = DateTime.UtcNow.AddYears(-13);

        if (dateOfBirth < minDate || dateOfBirth > maxDate)
            throw new CustomValidationException($"Date of birth must be between {minDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd}");

        UpdateWithEvent(() => DateOfBirth = dateOfBirth, new CustomerDateOfBirthUpdatedEvent(Id, dateOfBirth));
    }

    #endregion

    #region Contact Information Methods

    public void SetEmail(Email email)
    {
        Guard.AgainstNull(email, nameof(email));

        var oldEmail = Email?.Value;
        UpdateWithEvent(() => Email = email, new CustomerEmailUpdatedEvent(Id, oldEmail, email.Value),
            () => IsEmailVerified = false); // Reset verification when email changes
    }

    public void SetMobileNumber(PhoneNumber mobileNumber)
    {
        Guard.AgainstNull(mobileNumber, nameof(mobileNumber));

        var oldMobileNumber = MobileNumber?.Value;
        UpdateWithEvent(() => MobileNumber = mobileNumber, new CustomerMobileNumberUpdatedEvent(Id, oldMobileNumber, mobileNumber.Value),
            () => IsPhoneVerified = false); // Reset verification when phone changes
    }

    public void VerifyEmail()
    {
        if (Email == null)
            throw new InvalidDomainOperationException("Cannot verify email when no email is set", "Customer", Id.ToString(), "VerifyEmail");

        UpdateWithEvent(() => IsEmailVerified = true, new CustomerEmailVerifiedEvent(Id, Email.Value));
    }

    public void VerifyPhone()
    {
        if (MobileNumber == null)
            throw new InvalidDomainOperationException("Cannot verify phone when no phone is set", "Customer", Id.ToString(), "VerifyPhone");

        UpdateWithEvent(() => IsPhoneVerified = true, new CustomerPhoneVerifiedEvent(Id, MobileNumber.Value));
    }

    #endregion

    #region Address Methods

    public void SetAddress(Address address)
    {
        Guard.AgainstNull(address, nameof(address));

        var oldAddress = Address?.ToString();
        UpdateWithEvent(() => Address = address, new CustomerAddressUpdatedEvent(Id, oldAddress, address.ToString()));
    }

    public void UpdateAddressDetails(string? details)
    {
        if (Address == null)
            throw new InvalidDomainOperationException("Cannot update address details when no address is set", "Customer", Id.ToString(), "UpdateAddressDetails");

        var oldAddress = Address.ToString();
        var newAddress = Address.UpdateDetails(details);
        UpdateWithEvent(() => Address = newAddress, new CustomerAddressUpdatedEvent(Id, oldAddress, newAddress.ToString()));
    }

    #endregion

    #region Status Methods

    private void ChangeStatus(CustomerStatus newStatus, BaseDomainEvent? domainEvent = null)
    {
        if (Status == newStatus) return;

        var oldStatus = Status;
        Status = newStatus;
        MarkAsUpdatedInternal();

        if (domainEvent == null)
            AddDomainEvent(new CustomerStatusChangedEvent(Id, oldStatus, newStatus));
        else
            AddDomainEvent(domainEvent);
    }

    public void Activate() => ChangeStatus(CustomerStatus.Active);

    public void Deactivate() => ChangeStatus(CustomerStatus.Inactive);

    public void Suspend(string reason)
    {
        Guard.AgainstNullOrEmpty(reason, nameof(reason));
        ChangeStatus(CustomerStatus.Suspended, new CustomerSuspendedEvent(Id, reason, Status));
    }

    public void RecordLogin()
    {
        var loginTime = DateTime.UtcNow;
        UpdateWithEvent(() => LastLoginAt = loginTime, new CustomerLoggedInEvent(Id, loginTime));
    }

    #endregion

    #region External Integration Methods

    public void LinkToApplicationUser(string applicationUserId)
    {
        Guard.AgainstNullOrEmpty(applicationUserId, nameof(applicationUserId));

        var oldApplicationUserId = ApplicationUserId;
        UpdateWithEvent(() => ApplicationUserId = applicationUserId, new CustomerLinkedToApplicationUserEvent(Id, oldApplicationUserId, applicationUserId));
    }

    public void UnlinkFromApplicationUser()
    {
        if (ApplicationUserId == null)
            return;

        var oldApplicationUserId = ApplicationUserId;
        UpdateWithEvent(() => ApplicationUserId = null, new CustomerUnlinkedFromApplicationUserEvent(Id, oldApplicationUserId));
    }

    #endregion

    #region Business Rules Validation

    public void ValidateBusinessRules()
    {
        var rules = new List<IBusinessRule>
        {
            new CustomerMustHaveValidNameRule(FirstName, LastName),
            new CustomerMustHaveContactInfoRule(Email, MobileNumber),
            new CustomerMustBeAtLeastThirteenYearsOldRule(DateOfBirth)
        };

        BusinessRuleValidator.Validate(rules.ToArray());
    }

    public async Task ValidateBusinessRulesAsync()
    {
        var rules = new List<IBusinessRule>
        {
            new CustomerMustHaveValidNameRule(FirstName, LastName),
            new CustomerMustHaveContactInfoRule(Email, MobileNumber),
            new CustomerMustBeAtLeastThirteenYearsOldRule(DateOfBirth)
        };

        await BusinessRuleValidator.ValidateAsync(rules.ToArray());
    }

    #endregion

    #region Private Helper Methods

    private void SetPersonalInfo(string firstName, string lastName)
    {
        Guard.AgainstNullOrEmpty(firstName, nameof(firstName));
        Guard.AgainstNullOrEmpty(lastName, nameof(lastName));
        Guard.AgainstTooLong(firstName, 50, nameof(firstName));
        Guard.AgainstTooLong(lastName, 50, nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    #endregion
}


