namespace Application.Commands.Users;

public sealed record UpdateUserCommand(
    int UserId,
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
);