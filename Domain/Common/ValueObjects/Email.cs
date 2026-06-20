using Domain.Common.Exceptions;

namespace Domain.Common.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException("Email cannot be empty.");

        value = value.Trim().ToLowerInvariant();

        if (!IsValid(value))
            throw new ValidationException("Invalid email address.");

        return new Email(value);
    }

    private static bool IsValid(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public override string ToString() => Value;
}