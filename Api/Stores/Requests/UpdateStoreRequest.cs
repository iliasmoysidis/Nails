namespace Api.Stores.Requests;

public sealed record UpdateStoreRequest(
    string? Name,
    string? Street,
    string? City,
    string? PostalCode,
    string? State,
    string? CountryCode,
    string? PhoneCountryCode,
    string? PhoneNumber
);
