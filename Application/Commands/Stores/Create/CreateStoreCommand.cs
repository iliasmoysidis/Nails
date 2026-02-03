namespace Application.Commands.Stores;

public sealed record CreateStoreCommand(
    string Name,
    string Street,
    string City,
    string PostalCode,
    string? State,
    string CountryCode,
    string TaxCountryCode,
    string TaxNumber,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber
);