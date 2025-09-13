namespace MyShop.xUnitTest.Domain.Aggregates.Customer;

public class CustomerTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidData_ShouldCreateCustomerSuccessfully()
    {
        // Arrange
        var firstName = "علی";
        var lastName = "احمدی";
        var createdBy = "system";

        // Act
        var customer = new MyShop.Domain.Entities.Customer.Customer(firstName, lastName, createdBy);

        // Assert
        customer.Should().NotBeNull();
        customer.FirstName.Should().Be(firstName);
        customer.LastName.Should().Be(lastName);
        customer.FullName.Should().Be($"{firstName} {lastName}");
        customer.Status.Should().Be(CustomerStatus.Active);
        customer.IsActive.Should().BeTrue();
        customer.IsEmailVerified.Should().BeFalse();
        customer.IsPhoneVerified.Should().BeFalse();
        customer.IsVerified.Should().BeFalse();
        customer.HasCompleteProfile.Should().BeFalse();
        customer.CreatedBy.Should().Be(createdBy);
        customer.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        customer.IsDeleted.Should().BeFalse();
        customer.Version.Should().Be(1);
    }

    [Fact]
    public void Constructor_WithId_ShouldCreateCustomerWithSpecificId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var firstName = "علی";
        var lastName = "احمدی";
        var createdBy = "system";

        // Act
        var customer = new MyShop.Domain.Entities.Customer.Customer(id, firstName, lastName, createdBy);

        // Assert
        customer.Id.Should().Be(id);
        customer.FirstName.Should().Be(firstName);
        customer.LastName.Should().Be(lastName);
    }

    [Fact]
    public void Constructor_WithEmptyFirstName_ShouldThrowException()
    {
        // Arrange
        var firstName = "";
        var lastName = "احمدی";
        var createdBy = "system";

        // Act & Assert
        var action = () => new MyShop.Domain.Entities.Customer.Customer(firstName, lastName, createdBy);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*cannot be empty*");
    }

    [Fact]
    public void Constructor_WithNullLastName_ShouldThrowException()
    {
        // Arrange
        var firstName = "علی";
        string lastName = null!;
        var createdBy = "system";

        // Act & Assert
        var action = () => new MyShop.Domain.Entities.Customer.Customer(firstName, lastName, createdBy);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*cannot be empty*");
    }

    [Fact]
    public void Constructor_WithTooLongFirstName_ShouldThrowException()
    {
        // Arrange
        var firstName = new string('ع', 51); // 51 characters
        var lastName = "احمدی";
        var createdBy = "system";

        // Act & Assert
        var action = () => new MyShop.Domain.Entities.Customer.Customer(firstName, lastName, createdBy);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*cannot exceed 50 characters*");
    }

    [Fact]
    public void Constructor_ShouldRaiseCustomerCreatedEvent()
    {
        // Arrange
        var firstName = "علی";
        var lastName = "احمدی";
        var createdBy = "system";

        // Act
        var customer = new MyShop.Domain.Entities.Customer.Customer(firstName, lastName, createdBy);

        // Assert
        customer.DomainEvents.Should().HaveCount(1);
        var domainEvent = customer.DomainEvents.First();
        domainEvent.Should().BeOfType<CustomerCreatedEvent>();
        
        var createdEvent = (CustomerCreatedEvent)domainEvent;
        createdEvent.AggregateId.Should().Be(customer.Id);
        createdEvent.FirstName.Should().Be(firstName);
        createdEvent.LastName.Should().Be(lastName);
        createdEvent.FullName.Should().Be($"{firstName} {lastName}");
    }

    #endregion

    #region Personal Information Tests

    [Fact]
    public void UpdatePersonalInfo_WithValidData_ShouldUpdateSuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var newFirstName = "محمد";
        var newLastName = "رضایی";

        // Act
        customer.UpdatePersonalInfo(newFirstName, newLastName);

        // Assert
        customer.FirstName.Should().Be(newFirstName);
        customer.LastName.Should().Be(newLastName);
        customer.FullName.Should().Be($"{newFirstName} {newLastName}");
        customer.Version.Should().Be(2);
        customer.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void UpdatePersonalInfo_ShouldRaisePersonalInfoUpdatedEvent()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var oldFirstName = customer.FirstName;
        var oldLastName = customer.LastName;
        var newFirstName = "محمد";
        var newLastName = "رضایی";

        // Act
        customer.UpdatePersonalInfo(newFirstName, newLastName);

        // Assert
        customer.DomainEvents.Should().HaveCount(2); // Created + Updated
        var lastEvent = customer.DomainEvents.Last();
        lastEvent.Should().BeOfType<CustomerPersonalInfoUpdatedEvent>();
        
        var updatedEvent = (CustomerPersonalInfoUpdatedEvent)lastEvent;
        updatedEvent.AggregateId.Should().Be(customer.Id);
        updatedEvent.OldFirstName.Should().Be(oldFirstName);
        updatedEvent.OldLastName.Should().Be(oldLastName);
        updatedEvent.NewFirstName.Should().Be(newFirstName);
        updatedEvent.NewLastName.Should().Be(newLastName);
    }

    [Fact]
    public void SetDateOfBirth_WithValidDate_ShouldSetSuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var dateOfBirth = new DateTime(1990, 5, 15);

        // Act
        customer.SetDateOfBirth(dateOfBirth);

        // Assert
        customer.DateOfBirth.Should().Be(dateOfBirth);
        customer.Age.Should().Be(DateTime.UtcNow.Year - 1990);
        customer.Version.Should().Be(2);
    }

    [Fact]
    public void SetDateOfBirth_WithFutureDate_ShouldThrowException()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act & Assert
        var action = () => customer.SetDateOfBirth(futureDate);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*cannot be in the future*");
    }

    [Fact]
    public void SetDateOfBirth_WithTooOldDate_ShouldThrowException()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var tooOldDate = DateTime.UtcNow.AddYears(-121);

        // Act & Assert
        var action = () => customer.SetDateOfBirth(tooOldDate);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*must be between*");
    }

    [Fact]
    public void SetDateOfBirth_WithTooYoungDate_ShouldThrowException()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var tooYoungDate = DateTime.UtcNow.AddYears(-12);

        // Act & Assert
        var action = () => customer.SetDateOfBirth(tooYoungDate);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*must be between*");
    }

    #endregion

    #region Contact Information Tests

    [Fact]
    public void SetEmail_WithValidEmail_ShouldSetSuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var email = new Email("test@example.com");

        // Act
        customer.SetEmail(email);

        // Assert
        customer.Email.Should().Be(email);
        customer.Email!.Value.Should().Be("test@example.com");
        customer.IsEmailVerified.Should().BeFalse(); // Should be reset when email changes
        customer.Version.Should().Be(2);
    }

    [Fact]
    public void SetEmail_ShouldRaiseEmailUpdatedEvent()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var email = new Email("test@example.com");

        // Act
        customer.SetEmail(email);

        // Assert
        var lastEvent = customer.DomainEvents.Last();
        lastEvent.Should().BeOfType<CustomerEmailUpdatedEvent>();
        
        var emailEvent = (CustomerEmailUpdatedEvent)lastEvent;
        emailEvent.AggregateId.Should().Be(customer.Id);
        emailEvent.OldEmail.Should().BeNull();
        emailEvent.NewEmail.Should().Be("test@example.com");
    }

    [Fact]
    public void SetMobileNumber_WithValidNumber_ShouldSetSuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var mobileNumber = new PhoneNumber("09123456789");

        // Act
        customer.SetMobileNumber(mobileNumber);

        // Assert
        customer.MobileNumber.Should().Be(mobileNumber);
        customer.MobileNumber!.Value.Should().Be("+989123456789");
        customer.IsPhoneVerified.Should().BeFalse(); // Should be reset when phone changes
        customer.Version.Should().Be(2);
    }

    [Fact]
    public void VerifyEmail_WithValidEmail_ShouldVerifySuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var email = new Email("test@example.com");
        customer.SetEmail(email);

        // Act
        customer.VerifyEmail();

        // Assert
        customer.IsEmailVerified.Should().BeTrue();
        customer.Version.Should().Be(3);
    }

    [Fact]
    public void VerifyEmail_WithoutEmail_ShouldThrowException()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act & Assert
        var action = () => customer.VerifyEmail();
        action.Should().Throw<InvalidDomainOperationException>()
            .WithMessage("*Cannot verify email when no email is set*");
    }

    [Fact]
    public void VerifyPhone_WithValidPhone_ShouldVerifySuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var mobileNumber = new PhoneNumber("09123456789");
        customer.SetMobileNumber(mobileNumber);

        // Act
        customer.VerifyPhone();

        // Assert
        customer.IsPhoneVerified.Should().BeTrue();
        customer.IsVerified.Should().BeFalse(); // Email not verified yet
        customer.Version.Should().Be(3);
    }

    [Fact]
    public void VerifyPhone_WithoutPhone_ShouldThrowException()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act & Assert
        var action = () => customer.VerifyPhone();
        action.Should().Throw<InvalidDomainOperationException>()
            .WithMessage("*Cannot verify phone when no phone is set*");
    }

    #endregion

    #region Address Tests

    [Fact]
    public void SetAddress_WithValidAddress_ShouldSetSuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var address = new Address("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", "1234567890");

        // Act
        customer.SetAddress(address);

        // Assert
        customer.Address.Should().Be(address);
        customer.HasCompleteAddress.Should().BeTrue();
        customer.Version.Should().Be(2);
    }

    [Fact]
    public void SetAddress_ShouldRaiseAddressUpdatedEvent()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var address = new Address("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", "1234567890");

        // Act
        customer.SetAddress(address);

        // Assert
        var lastEvent = customer.DomainEvents.Last();
        lastEvent.Should().BeOfType<CustomerAddressUpdatedEvent>();
        
        var addressEvent = (CustomerAddressUpdatedEvent)lastEvent;
        addressEvent.AggregateId.Should().Be(customer.Id);
        addressEvent.OldAddress.Should().BeNull();
        addressEvent.NewAddress.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void UpdateAddressDetails_WithValidDetails_ShouldUpdateSuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var address = new Address("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", "1234567890");
        customer.SetAddress(address);
        var newDetails = "پلاک 123، طبقه 2";

        // Act
        customer.UpdateAddressDetails(newDetails);

        // Assert
        customer.Address!.Details.Should().Be(newDetails);
        customer.Version.Should().Be(3);
    }

    [Fact]
    public void UpdateAddressDetails_WithoutAddress_ShouldThrowException()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act & Assert
        var action = () => customer.UpdateAddressDetails("details");
        action.Should().Throw<InvalidDomainOperationException>()
            .WithMessage("*Cannot update address details when no address is set*");
    }

    #endregion

    #region Status Tests

    [Fact]
    public void Activate_ShouldChangeStatusToActive()
    {
        // Arrange
        var customer = CreateValidCustomer();
        customer.Deactivate(); // First deactivate

        // Act
        customer.Activate();

        // Assert
        customer.Status.Should().Be(CustomerStatus.Active);
        customer.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_ShouldChangeStatusToInactive()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act
        customer.Deactivate();

        // Assert
        customer.Status.Should().Be(CustomerStatus.Inactive);
        customer.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Suspend_WithValidReason_ShouldSuspendCustomer()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var reason = "نقض قوانین";

        // Act
        customer.Suspend(reason);

        // Assert
        customer.Status.Should().Be(CustomerStatus.Suspended);
        customer.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Suspend_ShouldRaiseSuspendedEvent()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var reason = "نقض قوانین";

        // Act
        customer.Suspend(reason);

        // Assert
        var lastEvent = customer.DomainEvents.Last();
        lastEvent.Should().BeOfType<CustomerSuspendedEvent>();
        
        var suspendedEvent = (CustomerSuspendedEvent)lastEvent;
        suspendedEvent.AggregateId.Should().Be(customer.Id);
        suspendedEvent.Reason.Should().Be(reason);
        suspendedEvent.PreviousStatus.Should().Be(CustomerStatus.Active);
    }

    [Fact]
    public void RecordLogin_ShouldUpdateLastLoginAt()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var beforeLogin = DateTime.UtcNow;

        // Act
        customer.RecordLogin();

        // Assert
        customer.LastLoginAt.Should().BeCloseTo(beforeLogin, TimeSpan.FromSeconds(5));
        customer.Version.Should().Be(2);
    }

    [Fact]
    public void RecordLogin_ShouldRaiseLoggedInEvent()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act
        customer.RecordLogin();

        // Assert
        var lastEvent = customer.DomainEvents.Last();
        lastEvent.Should().BeOfType<CustomerLoggedInEvent>();
        
        var loginEvent = (CustomerLoggedInEvent)lastEvent;
        loginEvent.AggregateId.Should().Be(customer.Id);
        loginEvent.LoginTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    #endregion

    #region External Integration Tests

    [Fact]
    public void LinkToApplicationUser_WithValidId_ShouldLinkSuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var applicationUserId = "user123";

        // Act
        customer.LinkToApplicationUser(applicationUserId);

        // Assert
        customer.ApplicationUserId.Should().Be(applicationUserId);
        customer.Version.Should().Be(2);
    }

    [Fact]
    public void LinkToApplicationUser_ShouldRaiseLinkedEvent()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var applicationUserId = "user123";

        // Act
        customer.LinkToApplicationUser(applicationUserId);

        // Assert
        var lastEvent = customer.DomainEvents.Last();
        lastEvent.Should().BeOfType<CustomerLinkedToApplicationUserEvent>();
        
        var linkedEvent = (CustomerLinkedToApplicationUserEvent)lastEvent;
        linkedEvent.AggregateId.Should().Be(customer.Id);
        linkedEvent.OldApplicationUserId.Should().BeNull();
        linkedEvent.NewApplicationUserId.Should().Be(applicationUserId);
    }

    [Fact]
    public void UnlinkFromApplicationUser_ShouldUnlinkSuccessfully()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var applicationUserId = "user123";
        customer.LinkToApplicationUser(applicationUserId);

        // Act
        customer.UnlinkFromApplicationUser();

        // Assert
        customer.ApplicationUserId.Should().BeNull();
        customer.Version.Should().Be(3);
    }

    [Fact]
    public void UnlinkFromApplicationUser_ShouldRaiseUnlinkedEvent()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var applicationUserId = "user123";
        customer.LinkToApplicationUser(applicationUserId);

        // Act
        customer.UnlinkFromApplicationUser();

        // Assert
        var lastEvent = customer.DomainEvents.Last();
        lastEvent.Should().BeOfType<CustomerUnlinkedFromApplicationUserEvent>();
        
        var unlinkedEvent = (CustomerUnlinkedFromApplicationUserEvent)lastEvent;
        unlinkedEvent.AggregateId.Should().Be(customer.Id);
        unlinkedEvent.ApplicationUserId.Should().Be(applicationUserId);
    }

    #endregion

    #region Business Rules Tests

    [Fact]
    public void ValidateBusinessRules_WithValidCustomer_ShouldPass()
    {
        // Arrange
        var customer = CreateValidCustomer();
        customer.SetEmail(new Email("test@example.com"));
        customer.SetMobileNumber(new PhoneNumber("09123456789"));

        // Act & Assert
        var action = () => customer.ValidateBusinessRules();
        action.Should().NotThrow();
    }

    [Fact]
    public void ValidateBusinessRules_WithInvalidName_ShouldThrowException()
    {
        // Arrange
        var customer = CreateValidCustomer();
        customer.UpdatePersonalInfo("", "احمدی"); // Invalid first name

        // Act & Assert
        var action = () => customer.ValidateBusinessRules();
        action.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("*Customer must have a valid first name*");
    }

    [Fact]
    public void ValidateBusinessRules_WithoutContactInfo_ShouldThrowException()
    {
        // Arrange
        var customer = CreateValidCustomer();
        // No email or phone set

        // Act & Assert
        var action = () => customer.ValidateBusinessRules();
        action.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("*Customer must have at least one contact method*");
    }

    [Fact]
    public void ValidateBusinessRules_WithTooYoungCustomer_ShouldThrowException()
    {
        // Arrange
        var customer = CreateValidCustomer();
        customer.SetDateOfBirth(DateTime.UtcNow.AddYears(-10)); // Too young

        // Act & Assert
        var action = () => customer.ValidateBusinessRules();
        action.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("*Customer must be at least 13 years old*");
    }

    #endregion

    #region Computed Properties Tests

    [Fact]
    public void FullName_ShouldReturnConcatenatedName()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act & Assert
        customer.FullName.Should().Be($"{customer.FirstName} {customer.LastName}");
    }

    [Fact]
    public void IsActive_ShouldReturnCorrectStatus()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act & Assert
        customer.IsActive.Should().BeTrue();
        
        customer.Deactivate();
        customer.IsActive.Should().BeFalse();
    }

    [Fact]
    public void IsVerified_ShouldReturnTrueWhenBothEmailAndPhoneVerified()
    {
        // Arrange
        var customer = CreateValidCustomer();
        customer.SetEmail(new Email("test@example.com"));
        customer.SetMobileNumber(new PhoneNumber("09123456789"));

        // Act
        customer.VerifyEmail();
        customer.VerifyPhone();

        // Assert
        customer.IsVerified.Should().BeTrue();
    }

    [Fact]
    public void HasCompleteProfile_ShouldReturnTrueWhenAllFieldsSet()
    {
        // Arrange
        var customer = CreateValidCustomer();
        customer.SetEmail(new Email("test@example.com"));
        customer.SetMobileNumber(new PhoneNumber("09123456789"));
        customer.SetAddress(new Address("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", "1234567890"));

        // Act & Assert
        customer.HasCompleteProfile.Should().BeTrue();
    }

    [Fact]
    public void Age_ShouldCalculateCorrectly()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var birthYear = 1990;
        customer.SetDateOfBirth(new DateTime(birthYear, 5, 15));

        // Act & Assert
        var expectedAge = DateTime.UtcNow.Year - birthYear;
        customer.Age.Should().Be(expectedAge);
    }

    #endregion

    #region Helper Methods

    private static MyShop.Domain.Entities.Customer.Customer CreateValidCustomer()
    {
        return new MyShop.Domain.Entities.Customer.Customer("علی", "احمدی", "system");
    }

    #endregion
}