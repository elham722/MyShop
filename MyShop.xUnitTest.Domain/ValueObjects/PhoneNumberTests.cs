using MyShop.Domain.Shared.Exceptions.Validation;

namespace MyShop.xUnitTest.Domain.ValueObjects;

public class PhoneNumberTests
{
    #region Constructor Tests

    [Theory]
    [InlineData("09123456789", "+989123456789")]
    [InlineData("+989123456789", "+989123456789")]
    [InlineData("00989123456789", "+989123456789")]
    public void Constructor_WithValidPhoneNumber_ShouldCreateSuccessfully(string input, string expected)
    {
        // Act
        var phoneNumber = new PhoneNumber(input);

        // Assert
        phoneNumber.Should().NotBeNull();
        phoneNumber.Value.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidPhoneNumber_ShouldThrowException(string phoneNumber)
    {
        // Act & Assert
        var action = () => new PhoneNumber(phoneNumber);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*cannot be empty*");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("123456789")]
    [InlineData("invalid")]
    [InlineData("123456789012345")]
    public void Constructor_WithInvalidFormat_ShouldThrowException(string phoneNumber)
    {
        // Act & Assert
        var action = () => new PhoneNumber(phoneNumber);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*Invalid phone number format*");
    }

    #endregion

    #region Phone Type Detection Tests

    [Theory]
    [InlineData("09123456789", true)]
    [InlineData("02112345678", false)]
    [InlineData("+989123456789", true)]
    [InlineData("+982112345678", false)]
    public void IsMobile_ShouldReturnCorrectValue(string phoneNumber, bool expected)
    {
        // Arrange
        var phoneValueObject = new PhoneNumber(phoneNumber);

        // Act
        var result = phoneValueObject.IsMobile();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("09123456789", false)]
    [InlineData("02112345678", true)]
    [InlineData("+989123456789", false)]
    [InlineData("+982112345678", true)]
    public void IsLandline_ShouldReturnCorrectValue(string phoneNumber, bool expected)
    {
        // Arrange
        var phoneValueObject = new PhoneNumber(phoneNumber);

        // Act
        var result = phoneValueObject.IsLandline();

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Format Tests

    [Theory]
    [InlineData("09123456789", "09123456789")]
    [InlineData("+989123456789", "09123456789")]
    public void GetLocalFormat_ShouldReturnCorrectFormat(string input, string expected)
    {
        // Arrange
        var phoneNumber = new PhoneNumber(input);

        // Act
        var result = phoneNumber.GetLocalFormat();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("09123456789", "+989123456789")]
    [InlineData("+989123456789", "+989123456789")]
    public void GetInternationalFormat_ShouldReturnCorrectFormat(string input, string expected)
    {
        // Arrange
        var phoneNumber = new PhoneNumber(input);

        // Act
        var result = phoneNumber.GetInternationalFormat();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("09123456789", "+98 9123-456-789")]
    [InlineData("+989123456789", "+98 9123-456-789")]
    public void GetDisplayFormat_ShouldReturnCorrectFormat(string input, string expected)
    {
        // Arrange
        var phoneNumber = new PhoneNumber(input);

        // Act
        var result = phoneNumber.GetDisplayFormat();

        // Assert
        result.Should().Be(expected);
    }

    #endregion

 

    #region Alternative Format Tests

    [Fact]
    public void GetAlternativeFormat_ShouldReturnAlternativeFormat()
    {
        // Arrange
        var phoneNumber = new PhoneNumber("09123456789");

        // Act
        var alternativeFormat = phoneNumber.GetAlternativeFormat();

        // Assert
        alternativeFormat.Value.Should().Be("+989123456789");
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSamePhoneNumber_ShouldReturnTrue()
    {
        // Arrange
        var phone1 = new PhoneNumber("09123456789");
        var phone2 = new PhoneNumber("+989123456789");

        // Act & Assert
        phone1.Should().Be(phone2);
        phone1.GetHashCode().Should().Be(phone2.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentPhoneNumber_ShouldReturnFalse()
    {
        // Arrange
        var phone1 = new PhoneNumber("09123456789");
        var phone2 = new PhoneNumber("09129876543");

        // Act & Assert
        phone1.Should().NotBe(phone2);
    }

    [Fact]
    public void ToString_ShouldReturnPhoneValue()
    {
        // Arrange
        var phoneNumber = new PhoneNumber("09123456789");

        // Act
        var result = phoneNumber.ToString();

        // Assert
        result.Should().Be("+989123456789");
    }

    [Fact]
    public void ImplicitConversion_ShouldWorkCorrectly()
    {
        // Arrange
        var phoneNumber = new PhoneNumber("09123456789");

        // Act
        string phoneString = phoneNumber;

        // Assert
        phoneString.Should().Be("+989123456789");
    }

    #endregion
}