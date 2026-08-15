namespace Api.Professionals.Requests;

public sealed record RegisterProfessionalRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber,
    string TaxCountryCode,
    string TaxIdNumber
);
