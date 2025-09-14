using MyShop.Domain.Shared.Exceptions.Validation;

namespace MyShop.xUnitTest.Domain.ValueObjects;

public class EmailTests
{
    #region Constructor Tests

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("test+tag@example.org")]
    [InlineData("user123@test-domain.com")]
    public void Constructor_WithValidEmail_ShouldCreateSuccessfully(string email)
    {
        // Act
        var emailValueObject = new Email(email);

        // Assert
        emailValueObject.Should().NotBeNull();
        emailValueObject.Value.Should().Be(email.ToLowerInvariant());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidEmail_ShouldThrowException(string email)
    {
        // Act & Assert
        var action = () => new Email(email);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*cannot be empty*");
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test.example.com")]
    [InlineData("test@.com")]
    [InlineData("test@example.")]
    public void Constructor_WithInvalidFormat_ShouldThrowException(string email)
    {
        // Act & Assert
        var action = () => new Email(email);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*Invalid email format*");
    }

    #endregion

    #region Email Type Detection Tests

    [Theory]
    [InlineData("test@gmail.com", false)]
    [InlineData("user@yahoo.com", false)]
    [InlineData("person@hotmail.com", false)]
    [InlineData("someone@outlook.com", false)]
    [InlineData("user@company.com", true)]
    [InlineData("employee@business.org", true)]
    public void IsBusinessEmail_ShouldReturnCorrectValue(string email, bool expected)
    {
        // Arrange
        var emailValueObject = new Email(email);

        // Act
        var result = emailValueObject.IsBusinessEmail();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("user@company.com", true)]
    [InlineData("employee@business.org", true)]
    [InlineData("test@corp.net", true)]
    [InlineData("user@inc.com", true)]
    [InlineData("person@gmail.com", false)]
    [InlineData("someone@yahoo.com", false)]
    public void IsCorporateEmail_ShouldReturnCorrectValue(string email, bool expected)
    {
        // Arrange
        var emailValueObject = new Email(email);

        // Act
        var result = emailValueObject.IsCorporateEmail();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("student@university.edu", true)]
    [InlineData("user@college.ac", true)]
    [InlineData("person@school.edu", true)]
    [InlineData("someone@institute.org", true)]
    [InlineData("test@gmail.com", false)]
    [InlineData("user@company.com", false)]
    public void IsEducationalEmail_ShouldReturnCorrectValue(string email, bool expected)
    {
        // Arrange
        var emailValueObject = new Email(email);

        // Act
        var result = emailValueObject.IsEducationalEmail();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("user@gov.com", true)]
    [InlineData("person@government.org", true)]
    [InlineData("someone@state.gov", true)]
    [InlineData("employee@municipal.org", true)]
    [InlineData("test@gmail.com", false)]
    [InlineData("user@company.com", false)]
    public void IsGovernmentEmail_ShouldReturnCorrectValue(string email, bool expected)
    {
        // Arrange
        var emailValueObject = new Email(email);

        // Act
        var result = emailValueObject.IsGovernmentEmail();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("test@10minutemail.com", true)]
    [InlineData("user@guerrillamail.com", true)]
    [InlineData("person@tempmail.org", true)]
    [InlineData("someone@gmail.com", false)]
    [InlineData("user@company.com", false)]
    public void IsDisposableEmail_ShouldReturnCorrectValue(string email, bool expected)
    {
        // Arrange
        var emailValueObject = new Email(email);

        // Act
        var result = emailValueObject.IsDisposableEmail();

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Email Parsing Tests

    [Fact]
    public void GetDomain_ShouldReturnCorrectDomain()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        var domain = email.GetDomain();

        // Assert
        domain.Should().Be("example.com");
    }

    [Fact]
    public void GetUsername_ShouldReturnCorrectUsername()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        var username = email.GetUsername();

        // Assert
        username.Should().Be("test");
    }

    [Fact]
    public void GetDisposableEmailString_ShouldReturnDisposableEmail()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        var disposableEmail = email.GetDisposableEmailString();

        // Assert
        disposableEmail.Should().Be("test+disposable@example.com");
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameEmail_ShouldReturnTrue()
    {
        // Arrange
        var email1 = new Email("test@example.com");
        var email2 = new Email("TEST@EXAMPLE.COM");

        // Act & Assert
        email1.Should().Be(email2);
        email1.GetHashCode().Should().Be(email2.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentEmail_ShouldReturnFalse()
    {
        // Arrange
        var email1 = new Email("test@example.com");
        var email2 = new Email("other@example.com");

        // Act & Assert
        email1.Should().NotBe(email2);
    }

    [Fact]
    public void ToString_ShouldReturnEmailValue()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        var result = email.ToString();

        // Assert
        result.Should().Be("test@example.com");
    }

    [Fact]
    public void ImplicitConversion_ShouldWorkCorrectly()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        string emailString = email;

        // Assert
        emailString.Should().Be("test@example.com");
    }

    #endregion
}