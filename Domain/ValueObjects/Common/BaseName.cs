using Domain.Exceptions;

namespace Domain.ValueObjects.Common;

public abstract record BaseName
{
    public string Value { get; }

    protected BaseName(string value, int maxLength, string subject)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{subject} name cannot be empty.");

        value = Normalize(value);

        if (value.Length > maxLength)
            throw new ValidationException($"{subject} name cannot exceed {maxLength} characters.");

        Value = value;
    }

    protected static string Normalize(string value)
        => value.Trim();

    public override string ToString()
        => Value;
}