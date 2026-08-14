namespace Api.Users.Requests;

public sealed record UpdateUserRequest(
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
);
