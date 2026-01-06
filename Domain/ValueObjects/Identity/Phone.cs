using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.ValueObjects.Identity;

public sealed record Phone
{
    public const int MaxLength = 20;

    public string CountryCode { get; }
    public string NationalNumber { get; }


    public string Value => $"{CountryCode}{NationalNumber}";

    private Phone(string countryCode, string nationalNumber)
    {
        CountryCode = countryCode;
        NationalNumber = nationalNumber;
    }

    public static Phone From(string countryCode, string nationalNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            throw new DomainException("Country code is required.");

        if (!countryCode.StartsWith("+"))
            throw new DomainException("Country code must start with '+'.");

        if (!Regex.IsMatch(countryCode, @"^\+\d+$"))
            throw new DomainException("Country code must contain digits only after '+'.");

        if (string.IsNullOrWhiteSpace(nationalNumber))
            throw new DomainException("Phone number is required.");

        if (!Regex.IsMatch(nationalNumber, @"^\d+$"))
            throw new DomainException("Phone number must contain digits only.");

        var combinedLength = countryCode.Length + nationalNumber.Length;

        if (combinedLength > MaxLength)
            throw new DomainException($"Phone number cannot exceed {MaxLength} characters.");

        return new Phone(countryCode, nationalNumber);
    }

    public static Phone Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Phone number is required.");

        value = value.Trim();

        if (!value.StartsWith("+"))
            throw new DomainException("Phone number must start with '+' and include the country code.");

        var match = Regex.Match(value, @"^(\+\d{1,4})(\d+)$");
        if (!match.Success)
            throw new DomainException("Invalid phone number format.");

        return From(
            countryCode: match.Groups[1].Value,
            nationalNumber: match.Groups[2].Value
        );
    }

    public override string ToString() => Value;
}