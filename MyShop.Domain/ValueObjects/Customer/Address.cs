﻿namespace MyShop.Domain.ValueObjects.Customer;
public class Address : BaseValueObject
{
    public string Country { get; private set; } = null!;
    public string Province { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string District { get; private set; } = null!;
    public string Street { get; private set; } = null!;
    public string PostalCode { get; private set; } = null!;
    public string? Details { get; private set; }

    public string FullAddress => $"{Street}, {District}, {City}, {Province}, {Country}".Trim();
    public string ShortAddress => $"{Street}, {City}, {Province}".Trim();
    public bool IsComplete => !string.IsNullOrWhiteSpace(Street) &&
                             !string.IsNullOrWhiteSpace(City) &&
                             !string.IsNullOrWhiteSpace(Province) &&
                             !string.IsNullOrWhiteSpace(Country);

    private Address() { }

    public Address(string country, string province, string city, string district, string street, string postalCode, string? details = null)
    {
        Guard.AgainstNullOrEmpty(country, nameof(country));
        Guard.AgainstNullOrEmpty(province, nameof(province));
        Guard.AgainstNullOrEmpty(city, nameof(city));
        Guard.AgainstNullOrEmpty(district, nameof(district));
        Guard.AgainstNullOrEmpty(street, nameof(street));
        Guard.AgainstNullOrEmpty(postalCode, nameof(postalCode));

        if (postalCode.Length < 5 || postalCode.Length > 10)
            throw new CustomValidationException("Postal code must be between 5 and 10 characters");

        if (street.Length < 5 || street.Length > 200)
            throw new CustomValidationException("Street must be between 5 and 200 characters");

        if (city.Length < 2 || city.Length > 100)
            throw new CustomValidationException("City must be between 2 and 100 characters");

        Country = country.Trim();
        Province = province.Trim();
        City = city.Trim();
        District = district.Trim();
        Street = street.Trim();
        PostalCode = postalCode.Trim();
        Details = details?.Trim();
    }

    public static Address Create(string country, string province, string city, string district, string street, string postalCode, string? details = null)
    {
        return new Address(country, province, city, district, street, postalCode, details);
    }

    public bool IsInSameCity(Address other)
    {
        return City.Equals(other.City, StringComparison.OrdinalIgnoreCase) &&
               Province.Equals(other.Province, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsInSameProvince(Address other)
    {
        return Province.Equals(other.Province, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsInSameCountry(Address other)
    {
        return Country.Equals(other.Country, StringComparison.OrdinalIgnoreCase);
    }

    public Address UpdateDetails(string? newDetails)
    {
        return new Address(Country, Province, City, District, Street, PostalCode, newDetails);
    }

    public Address UpdateStreet(string newStreet)
    {
        Guard.AgainstNullOrEmpty(newStreet, nameof(newStreet));
        return new Address(Country, Province, City, District, newStreet, PostalCode, Details);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Country;
        yield return Province;
        yield return City;
        yield return District;
        yield return Street;
        yield return PostalCode;
        yield return Details;
    }

    public override string ToString()
    {
        return FullAddress;
    }
}
