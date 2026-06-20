using Domain.Common.Exceptions;

namespace Domain.Catalog.ValueObjects;

public sealed record Description
{
    public const int MaxLength = 500;

    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Description Empty()
        => new(string.Empty);

    public static Description From(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Empty();

        value = value.Trim();

        if (value.Length > MaxLength)
            throw new ValidationException($"Description cannot exceed {MaxLength} characters.");

        return new(value);
    }

    public override string ToString() => Value;
}