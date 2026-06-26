using Domain.Common.Exceptions;

namespace Domain.Common.ValueObjects;

public sealed record Email
{
    public const int MaxLength = 320;

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

        if (value.Length > MaxLength)
            throw new ValidationException($"Email cannot exceed {MaxLength} characters.");

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
