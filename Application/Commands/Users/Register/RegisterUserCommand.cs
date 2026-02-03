namespace Application.Commands.Users;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber
);