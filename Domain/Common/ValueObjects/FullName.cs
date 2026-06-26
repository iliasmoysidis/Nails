using Domain.Common.Exceptions;

namespace Domain.Common.ValueObjects;

public sealed record FullName
{
    public const int MaxLength = 100;

    public string FirstName { get; }
    public string LastName { get; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    private static string Normalize(string value)
        => value.Trim();

    public static FullName From(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ValidationException($"First name cannot be empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException($"Last name cannot be empty.");

        firstName = Normalize(firstName);
        lastName = Normalize(lastName);

        if (firstName.Length > MaxLength)
            throw new ValidationException($"First name cannot exceed {MaxLength} characters.");

        if (lastName.Length > MaxLength)
            throw new ValidationException($"Last name cannot exceed {MaxLength} characters.");

        return new(firstName, lastName);
    }

    public override string ToString() => $"{FirstName} {LastName}";
}
