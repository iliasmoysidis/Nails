namespace Api.Stores.Requests;

public sealed record CreateStoreRequest(
    int ProfessionalId,
    string Name,
    string Street,
    string City,
    string PostalCode,
    string State,
    string CountryCode,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber,
    string TaxCountryCode,
    string TaxNumber
);
