namespace MyShop.xUnitTest.Domain.BusinessRules;

public class CustomerBusinessRulesTests
{
    #region CustomerMustHaveValidNameRule Tests

    [Fact]
    public void CustomerMustHaveValidNameRule_WithValidNames_ShouldNotBeBroken()
    {
        // Arrange
        var rule = new CustomerMustHaveValidNameRule("علی", "احمدی");

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    [Theory]
    [InlineData("", "احمدی")]
    [InlineData("علی", "")]
    [InlineData("a", "احمدی")] // Too short
    [InlineData("علی", "a")] // Too short
    public void CustomerMustHaveValidNameRule_WithInvalidNames_ShouldBeBroken(string firstName, string lastName)
    {
        // Arrange
        var rule = new CustomerMustHaveValidNameRule(firstName, lastName);

        // Act & Assert
        rule.IsBroken().Should().BeTrue();
        rule.IsSatisfied().Should().BeFalse();
        rule.Message.Should().Contain("Customer must have a valid first name and last name");
    }

    [Fact]
    public void CustomerMustHaveValidNameRule_WithTooLongNames_ShouldBeBroken()
    {
        // Arrange
        var longName = new string('ع', 51); // 51 characters
        var rule = new CustomerMustHaveValidNameRule(longName, "احمدی");

        // Act & Assert
        rule.IsBroken().Should().BeTrue();
        rule.Message.Should().Contain("Customer must have a valid first name and last name");
    }

    #endregion

    #region CustomerMustHaveContactInfoRule Tests

    [Fact]
    public void CustomerMustHaveContactInfoRule_WithEmail_ShouldNotBeBroken()
    {
        // Arrange
        var email = new Email("test@example.com");
        var rule = new CustomerMustHaveContactInfoRule(email, null);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    [Fact]
    public void CustomerMustHaveContactInfoRule_WithMobileNumber_ShouldNotBeBroken()
    {
        // Arrange
        var mobileNumber = new PhoneNumber("09123456789");
        var rule = new CustomerMustHaveContactInfoRule(null, mobileNumber);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    [Fact]
    public void CustomerMustHaveContactInfoRule_WithBothEmailAndMobile_ShouldNotBeBroken()
    {
        // Arrange
        var email = new Email("test@example.com");
        var mobileNumber = new PhoneNumber("09123456789");
        var rule = new CustomerMustHaveContactInfoRule(email, mobileNumber);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    [Fact]
    public void CustomerMustHaveContactInfoRule_WithoutAnyContact_ShouldBeBroken()
    {
        // Arrange
        var rule = new CustomerMustHaveContactInfoRule(null, null);

        // Act & Assert
        rule.IsBroken().Should().BeTrue();
        rule.IsSatisfied().Should().BeFalse();
        rule.Message.Should().Contain("Customer must have at least one contact method");
    }

    #endregion

    #region CustomerMustBeAtLeastThirteenYearsOldRule Tests

    [Fact]
    public void CustomerMustBeAtLeastThirteenYearsOldRule_WithValidAge_ShouldNotBeBroken()
    {
        // Arrange
        var dateOfBirth = DateTime.UtcNow.AddYears(-20); // 20 years old
        var rule = new CustomerMustBeAtLeastThirteenYearsOldRule(dateOfBirth);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    [Fact]
    public void CustomerMustBeAtLeastThirteenYearsOldRule_WithExactlyThirteenYears_ShouldNotBeBroken()
    {
        // Arrange
        var dateOfBirth = DateTime.UtcNow.AddYears(-13).AddDays(-1); // Just over 13 years
        var rule = new CustomerMustBeAtLeastThirteenYearsOldRule(dateOfBirth);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    [Fact]
    public void CustomerMustBeAtLeastThirteenYearsOldRule_WithTooYoungAge_ShouldBeBroken()
    {
        // Arrange
        var dateOfBirth = DateTime.UtcNow.AddYears(-10); // 10 years old
        var rule = new CustomerMustBeAtLeastThirteenYearsOldRule(dateOfBirth);

        // Act & Assert
        rule.IsBroken().Should().BeTrue();
        rule.IsSatisfied().Should().BeFalse();
        rule.Message.Should().Contain("Customer must be at least 13 years old");
    }

    [Fact]
    public void CustomerMustBeAtLeastThirteenYearsOldRule_WithNullDateOfBirth_ShouldNotBeBroken()
    {
        // Arrange
        var rule = new CustomerMustBeAtLeastThirteenYearsOldRule(null);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    #endregion

    #region CustomerMustBeActiveRule Tests

    [Fact]
    public void CustomerMustBeActiveRule_WithActiveStatus_ShouldNotBeBroken()
    {
        // Arrange
        var rule = new CustomerMustBeActiveRule(CustomerStatus.Active);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    [Theory]
    [InlineData(CustomerStatus.Inactive)]
    [InlineData(CustomerStatus.Suspended)]
    [InlineData(CustomerStatus.Deleted)]
    public void CustomerMustBeActiveRule_WithInactiveStatus_ShouldBeBroken(CustomerStatus status)
    {
        // Arrange
        var rule = new CustomerMustBeActiveRule(status);

        // Act & Assert
        rule.IsBroken().Should().BeTrue();
        rule.IsSatisfied().Should().BeFalse();
        rule.Message.Should().Contain("Customer must be active to perform this operation");
    }

    #endregion

    #region CustomerCannotBeSuspendedRule Tests

    [Theory]
    [InlineData(CustomerStatus.Active)]
    [InlineData(CustomerStatus.Inactive)]
    [InlineData(CustomerStatus.Deleted)]
    public void CustomerCannotBeSuspendedRule_WithNonSuspendedStatus_ShouldNotBeBroken(CustomerStatus status)
    {
        // Arrange
        var rule = new CustomerCannotBeSuspendedRule(status);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    [Fact]
    public void CustomerCannotBeSuspendedRule_WithSuspendedStatus_ShouldBeBroken()
    {
        // Arrange
        var rule = new CustomerCannotBeSuspendedRule(CustomerStatus.Suspended);

        // Act & Assert
        rule.IsBroken().Should().BeTrue();
        rule.IsSatisfied().Should().BeFalse();
        rule.Message.Should().Contain("This operation cannot be performed on a suspended customer");
    }

    #endregion

    #region CustomerMustHaveValidAddressRule Tests

    [Fact]
    public void CustomerMustHaveValidAddressRule_WithCompleteAddress_ShouldNotBeBroken()
    {
        // Arrange
        var address = new Address("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", "1234567890");
        var rule = new CustomerMustHaveValidAddressRule(address);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    [Fact]
    public void CustomerMustHaveValidAddressRule_WithNullAddress_ShouldNotBeBroken()
    {
        // Arrange
        var rule = new CustomerMustHaveValidAddressRule(null);

        // Act & Assert
        rule.IsBroken().Should().BeFalse();
        rule.IsSatisfied().Should().BeTrue();
    }

    #endregion

    #region BusinessRuleValidator Tests

    [Fact]
    public void BusinessRuleValidator_WithValidRules_ShouldNotThrow()
    {
        // Arrange
        var rules = new IBusinessRule[]
        {
            new CustomerMustHaveValidNameRule("علی", "احمدی"),
            new CustomerMustHaveContactInfoRule(new Email("test@example.com"), null),
            new CustomerMustBeAtLeastThirteenYearsOldRule(DateTime.UtcNow.AddYears(-20))
        };

        // Act & Assert
        var action = () => BusinessRuleValidator.Validate(rules);
        action.Should().NotThrow();
    }

    [Fact]
    public void BusinessRuleValidator_WithBrokenRules_ShouldThrowException()
    {
        // Arrange
        var rules = new IBusinessRule[]
        {
            new CustomerMustHaveValidNameRule("", "احمدی"), // Broken rule
            new CustomerMustHaveContactInfoRule(null, null), // Broken rule
            new CustomerMustBeAtLeastThirteenYearsOldRule(DateTime.UtcNow.AddYears(-20)) // Valid rule
        };

        // Act & Assert
        var action = () => BusinessRuleValidator.Validate(rules);
        action.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("*Customer must have a valid first name*")
            .WithMessage("*Customer must have at least one contact method*");
    }

    [Fact]
    public void BusinessRuleValidator_AreValid_WithValidRules_ShouldReturnTrue()
    {
        // Arrange
        var rules = new IBusinessRule[]
        {
            new CustomerMustHaveValidNameRule("علی", "احمدی"),
            new CustomerMustHaveContactInfoRule(new Email("test@example.com"), null)
        };

        // Act
        var result = BusinessRuleValidator.AreValid(rules);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void BusinessRuleValidator_AreValid_WithBrokenRules_ShouldReturnFalse()
    {
        // Arrange
        var rules = new IBusinessRule[]
        {
            new CustomerMustHaveValidNameRule("", "احمدی"), // Broken rule
            new CustomerMustHaveContactInfoRule(new Email("test@example.com"), null) // Valid rule
        };

        // Act
        var result = BusinessRuleValidator.AreValid(rules);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void BusinessRuleValidator_GetBrokenRuleMessages_ShouldReturnCorrectMessages()
    {
        // Arrange
        var rules = new IBusinessRule[]
        {
            new CustomerMustHaveValidNameRule("", "احمدی"), // Broken rule
            new CustomerMustHaveContactInfoRule(null, null), // Broken rule
            new CustomerMustBeAtLeastThirteenYearsOldRule(DateTime.UtcNow.AddYears(-20)) // Valid rule
        };

        // Act
        var messages = BusinessRuleValidator.GetBrokenRuleMessages(rules);

        // Assert
        messages.Should().HaveCount(2);
        messages.Should().Contain(m => m.Contains("Customer must have a valid first name"));
        messages.Should().Contain(m => m.Contains("Customer must have at least one contact method"));
    }

    #endregion

    #region Composite Business Rule Tests

    [Fact]
    public void CompositeBusinessRule_WithValidRules_ShouldNotBeBroken()
    {
        // Arrange
        var rules = new IBusinessRule[]
        {
            new CustomerMustHaveValidNameRule("علی", "احمدی"),
            new CustomerMustHaveContactInfoRule(new Email("test@example.com"), null)
        };
        var compositeRule = new CompositeBusinessRule(rules, "Customer validation failed");

        // Act & Assert
        compositeRule.IsBroken().Should().BeFalse();
        compositeRule.HasBrokenRules.Should().BeFalse();
        compositeRule.BrokenRulesCount.Should().Be(0);
        compositeRule.TotalRules.Should().Be(2);
    }

    [Fact]
    public void CompositeBusinessRule_WithBrokenRules_ShouldBeBroken()
    {
        // Arrange
        var rules = new IBusinessRule[]
        {
            new CustomerMustHaveValidNameRule("", "احمدی"), // Broken
            new CustomerMustHaveContactInfoRule(null, null) // Broken
        };
        var compositeRule = new CompositeBusinessRule(rules, "Customer validation failed");

        // Act & Assert
        compositeRule.IsBroken().Should().BeTrue();
        compositeRule.HasBrokenRules.Should().BeTrue();
        compositeRule.BrokenRulesCount.Should().Be(2);
        compositeRule.TotalRules.Should().Be(2);
        compositeRule.Message.Should().Contain("Customer validation failed");
    }

    #endregion
}