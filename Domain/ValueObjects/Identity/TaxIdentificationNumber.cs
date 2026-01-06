using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.ValueObjects.Identity;

public sealed record TaxIdentificationNumber
{
    private const int MaxLength = 30;

    public string CountryCode { get; }
    public string Value { get; }

    private TaxIdentificationNumber(string countryCode, string value)
    {
        CountryCode = countryCode;
        Value = value;
    }

    public static TaxIdentificationNumber From(string countryCode, string value)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            throw new DomainException("Country code is required for tax identification number.");

        countryCode = countryCode.Trim().ToUpperInvariant();

        if (!Regex.IsMatch(countryCode, @"^[A-Z]{2}$"))
            throw new DomainException("Country code must be a valid ISO-3166 alpha-2 code.");

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Tax identification number is required.");

        value = value.Trim().ToUpperInvariant();

        if (countryCode.Length + value.Length > MaxLength)
            throw new DomainException("Tax identification number cannot exceed 30 characters.");

        if (!Regex.IsMatch(value, @"^[A-Z0-9\-]+$"))
            throw new DomainException("Tax identification number contains invalid characters.");

        return new TaxIdentificationNumber(countryCode, value);
    }

    public override string ToString() => $"{CountryCode}{Value}";
}