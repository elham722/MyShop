using MyShop.Domain.Shared.Exceptions.Validation;
using MyShop.Domain.Shared.ValueObjects.Customer;

namespace MyShop.xUnitTest.Domain.ValueObjects;

public class AddressTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        var country = "ایران";
        var province = "تهران";
        var city = "تهران";
        var district = "منطقه 1";
        var street = "خیابان ولیعصر";
        var postalCode = "1234567890";
        var details = "پلاک 123";

        // Act
        var address = new Address(country, province, city, district, street, postalCode, details);

        // Assert
        address.Should().NotBeNull();
        address.Country.Should().Be(country);
        address.Province.Should().Be(province);
        address.City.Should().Be(city);
        address.District.Should().Be(district);
        address.Street.Should().Be(street);
        address.PostalCode.Should().Be(postalCode);
        address.Details.Should().Be(details);
        address.IsComplete.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", "1234567890")]
    [InlineData("ایران", "", "تهران", "منطقه 1", "خیابان ولیعصر", "1234567890")]
    [InlineData("ایران", "تهران", "", "منطقه 1", "خیابان ولیعصر", "1234567890")]
    [InlineData("ایران", "تهران", "تهران", "", "خیابان ولیعصر", "1234567890")]
    [InlineData("ایران", "تهران", "تهران", "منطقه 1", "", "1234567890")]
    [InlineData("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", "")]
    public void Constructor_WithEmptyRequiredFields_ShouldThrowException(
        string country, string province, string city, string district, string street, string postalCode)
    {
        // Act & Assert
        var action = () => new Address(country, province, city, district, street, postalCode);
        action.Should().Throw<CustomValidationException>()
            .WithMessage("*cannot be empty*");
    }

    [Fact]
    public void Constructor_WithInvalidPostalCodeLength_ShouldThrowException()
    {
        // Arrange
        var shortPostalCode = "123"; // Too short
        var longPostalCode = "12345678901"; // Too long

        // Act & Assert
        var shortAction = () => new Address("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", shortPostalCode);
        var longAction = () => new Address("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", longPostalCode);

        shortAction.Should().Throw<CustomValidationException>()
            .WithMessage("*Postal code must be between 5 and 10 characters*");
        longAction.Should().Throw<CustomValidationException>()
            .WithMessage("*Postal code must be between 5 and 10 characters*");
    }

    [Fact]
    public void Constructor_WithInvalidStreetLength_ShouldThrowException()
    {
        // Arrange
        var shortStreet = "abc"; // Too short
        var longStreet = new string('a', 201); // Too long

        // Act & Assert
        var shortAction = () => new Address("ایران", "تهران", "تهران", "منطقه 1", shortStreet, "1234567890");
        var longAction = () => new Address("ایران", "تهران", "تهران", "منطقه 1", longStreet, "1234567890");

        shortAction.Should().Throw<CustomValidationException>()
            .WithMessage("*Street must be between 5 and 200 characters*");
        longAction.Should().Throw<CustomValidationException>()
            .WithMessage("*Street must be between 5 and 200 characters*");
    }

    [Fact]
    public void Constructor_WithInvalidCityLength_ShouldThrowException()
    {
        // Arrange
        var shortCity = "a"; // Too short
        var longCity = new string('a', 101); // Too long

        // Act & Assert
        var shortAction = () => new Address("ایران", "تهران", shortCity, "منطقه 1", "خیابان ولیعصر", "1234567890");
        var longAction = () => new Address("ایران", "تهران", longCity, "منطقه 1", "خیابان ولیعصر", "1234567890");

        shortAction.Should().Throw<CustomValidationException>()
            .WithMessage("*City must be between 2 and 100 characters*");
        longAction.Should().Throw<CustomValidationException>()
            .WithMessage("*City must be between 2 and 100 characters*");
    }

    #endregion

    #region Comparison Tests

    [Fact]
    public void IsInSameCity_WithSameCity_ShouldReturnTrue()
    {
        // Arrange
        var address1 = CreateValidAddress();
        var address2 = new Address("ایران", "تهران", "تهران", "منطقه 2", "خیابان آزادی", "0987654321");

        // Act
        var result = address1.IsInSameCity(address2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInSameCity_WithDifferentCity_ShouldReturnFalse()
    {
        // Arrange
        var address1 = CreateValidAddress();
        var address2 = new Address("ایران", "تهران", "کرج", "منطقه 1", "خیابان طالقانی", "1234567890");

        // Act
        var result = address1.IsInSameCity(address2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsInSameProvince_WithSameProvince_ShouldReturnTrue()
    {
        // Arrange
        var address1 = CreateValidAddress();
        var address2 = new Address("ایران", "تهران", "کرج", "منطقه 1", "خیابان طالقانی", "1234567890");

        // Act
        var result = address1.IsInSameProvince(address2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInSameCountry_WithSameCountry_ShouldReturnTrue()
    {
        // Arrange
        var address1 = CreateValidAddress();
        var address2 = new Address("ایران", "اصفهان", "اصفهان", "منطقه 1", "خیابان چهارباغ", "1234567890");

        // Act
        var result = address1.IsInSameCountry(address2);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region Update Tests

    [Fact]
    public void UpdateDetails_ShouldReturnNewAddressWithUpdatedDetails()
    {
        // Arrange
        var address = CreateValidAddress();
        var newDetails = "پلاک 456، واحد 2";

        // Act
        var updatedAddress = address.UpdateDetails(newDetails);

        // Assert
        updatedAddress.Should().NotBeSameAs(address);
        updatedAddress.Details.Should().Be(newDetails);
        updatedAddress.Country.Should().Be(address.Country);
        updatedAddress.Province.Should().Be(address.Province);
        updatedAddress.City.Should().Be(address.City);
        updatedAddress.District.Should().Be(address.District);
        updatedAddress.Street.Should().Be(address.Street);
        updatedAddress.PostalCode.Should().Be(address.PostalCode);
    }

    [Fact]
    public void UpdateStreet_ShouldReturnNewAddressWithUpdatedStreet()
    {
        // Arrange
        var address = CreateValidAddress();
        var newStreet = "خیابان آزادی";

        // Act
        var updatedAddress = address.UpdateStreet(newStreet);

        // Assert
        updatedAddress.Should().NotBeSameAs(address);
        updatedAddress.Street.Should().Be(newStreet);
        updatedAddress.Details.Should().Be(address.Details);
    }

    #endregion

    #region String Representation Tests

    [Fact]
    public void FullAddress_ShouldReturnCorrectFormat()
    {
        // Arrange
        var address = CreateValidAddress();

        // Act
        var fullAddress = address.FullAddress;

        // Assert
        fullAddress.Should().Be("خیابان ولیعصر، منطقه 1، تهران، تهران، ایران");
    }

    [Fact]
    public void ShortAddress_ShouldReturnCorrectFormat()
    {
        // Arrange
        var address = CreateValidAddress();

        // Act
        var shortAddress = address.ShortAddress;

        // Assert
        shortAddress.Should().Be("خیابان ولیعصر، تهران، تهران");
    }

    [Fact]
    public void ToString_ShouldReturnFullAddress()
    {
        // Arrange
        var address = CreateValidAddress();

        // Act
        var result = address.ToString();

        // Assert
        result.Should().Be(address.FullAddress);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameAddressData_ShouldReturnTrue()
    {
        // Arrange
        var address1 = CreateValidAddress();
        var address2 = new Address("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", "1234567890", "پلاک 123");

        // Act & Assert
        address1.Should().Be(address2);
        address1.GetHashCode().Should().Be(address2.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentAddressData_ShouldReturnFalse()
    {
        // Arrange
        var address1 = CreateValidAddress();
        var address2 = new Address("ایران", "تهران", "تهران", "منطقه 2", "خیابان آزادی", "0987654321");

        // Act & Assert
        address1.Should().NotBe(address2);
    }

    #endregion

    #region Helper Methods

    private static Address CreateValidAddress()
    {
        return new Address("ایران", "تهران", "تهران", "منطقه 1", "خیابان ولیعصر", "1234567890", "پلاک 123");
    }

    #endregion
}