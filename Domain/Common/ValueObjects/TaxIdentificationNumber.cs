using System.Text.RegularExpressions;
using Domain.Common.Exceptions;

namespace Domain.Common.ValueObjects;

public sealed record TaxIdentificationNumber
{
    public const int CountryCodeMaxLength = 30;
    public const int TaxIdNumberMaxLength = 50;

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
            throw new ValidationException("Country code is required for tax identification number.");

        countryCode = countryCode.Trim().ToUpperInvariant();

        if (countryCode.Length > CountryCodeMaxLength)
            throw new ValidationException($"Country code cannot exceed {CountryCodeMaxLength} characters.");

        if (!Regex.IsMatch(countryCode, @"^[A-Z]{2}$"))
            throw new ValidationException("Country code must be a valid ISO-3166 alpha-2 code.");

        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException("Tax identification number is required.");

        value = value.Trim().ToUpperInvariant();

        if (value.Length > TaxIdNumberMaxLength)
            throw new ValidationException($"Tax id number cannot exceed {TaxIdNumberMaxLength} digits.");

        if (!Regex.IsMatch(value, @"^[A-Z0-9\-]+$"))
            throw new ValidationException("Tax identification number contains invalid characters.");

        return new TaxIdentificationNumber(countryCode, value);
    }

    public override string ToString() => $"{CountryCode}{Value}";
}
