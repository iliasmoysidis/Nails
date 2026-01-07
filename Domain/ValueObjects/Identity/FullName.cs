using Domain.Exceptions;

namespace Domain.ValueObjects.Identity;

public sealed record FullName
{
    private const int MaxLength = 100;

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
            throw new DomainException($"First name cannot be empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException($"Last name cannot be empty.");

        firstName = Normalize(firstName);
        lastName = Normalize(lastName);

        if (firstName.Length > MaxLength)
            throw new DomainException($"First name cannot exceed {MaxLength} characters.");

        if (lastName.Length > MaxLength)
            throw new DomainException($"Last name cannot exceed {MaxLength} characters.");

        return new(firstName, lastName);
    }

    public override string ToString() => $"{FirstName} {LastName}";
}