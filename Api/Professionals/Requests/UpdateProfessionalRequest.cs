namespace Api.Professionals.Requests;

public sealed record UpdateProfessionalRequest(
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
);
