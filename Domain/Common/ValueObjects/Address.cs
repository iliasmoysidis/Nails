using System.Text.RegularExpressions;
using Domain.Common.Exceptions;

namespace Domain.Common.ValueObjects;

public sealed record Address
{
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string State { get; }
    public string CountryCode { get; }

    private Address(
        string street,
        string city,
        string postalCode,
        string state,
        string countryCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        State = state;
        CountryCode = countryCode;
    }

    public static Address From(
        string street,
        string city,
        string postalCode,
        string state,
        string countryCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ValidationException("Street is required.");

        if (string.IsNullOrWhiteSpace(city))
            throw new ValidationException("City is required.");

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ValidationException("Postal code is required.");

        if (string.IsNullOrWhiteSpace(state))
            throw new ValidationException("State is required.");

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ValidationException("Country code is required.");

        street = street.Trim();
        city = city.Trim();
        postalCode = postalCode.Trim();
        countryCode = countryCode.Trim().ToUpperInvariant();
        state = state.Trim();

        if (street.Length > 200)
            throw new ValidationException("Street cannot exceed 200 characters.");

        if (city.Length > 100)
            throw new ValidationException("City cannot exceed 100 characters.");

        if (postalCode.Length > 20)
            throw new ValidationException("Postal code cannot exceed 20 characters.");

        if (!Regex.IsMatch(countryCode, @"^[A-Z]{2}$"))
            throw new ValidationException("Country code must be a valid ISO-3166 alpha-2 code.");

        if (state.Length > 100)
            throw new ValidationException("State cannot exceed 100 characters.");

        return new Address(
            street,
            city,
            postalCode,
            state,
            countryCode
        );
    }

    public override string ToString()
        => $"{Street}, {PostalCode} {City}, {CountryCode}";
}