namespace Api.Users.Requests;

public sealed record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber
);
