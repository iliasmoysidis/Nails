using Domain.Common.Exceptions;

namespace Domain.Appointments.ValueObjects;

public sealed record Notes
{
    public const int MaxLength = 500;

    public string Value { get; }

    private Notes(string value)
    {
        Value = value;
    }

    public static Notes Empty()
        => new(string.Empty);

    public static Notes From(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Empty();

        value = value.Trim();

        if (value.Length > MaxLength)
            throw new ValidationException($"Notes cannot exceed {MaxLength} characters.");

        return new(value);
    }

    public Notes Append(string label, string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return this;

        var line = $"{label}: {content.Trim()}";

        var combined = string.IsNullOrEmpty(Value) ? line : $"{Value}\n{line}";

        if (combined.Length > MaxLength)
            throw new ValidationException($"Notes cannot exceed {MaxLength} characters.");

        return new(combined);
    }

    public override string ToString() => Value;

}