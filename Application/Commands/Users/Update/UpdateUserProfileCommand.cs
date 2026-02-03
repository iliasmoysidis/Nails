namespace Application.Commands.Users;

public sealed record UpdateUserProfileCommand(
    int UserId,
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
);