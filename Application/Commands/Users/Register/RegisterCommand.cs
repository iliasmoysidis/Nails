namespace Application.Commands.Users;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber
);