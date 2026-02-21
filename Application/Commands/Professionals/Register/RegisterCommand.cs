namespace Application.Commands.Professionals;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber,
    string TaxCountryCode,
    string TaxIdNumber
);