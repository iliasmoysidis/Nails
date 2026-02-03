namespace Application.Commands.Stores;

public sealed record UpdateDetailsCommand(
    int StoreId,
    string? Name,
    string? Street,
    string? City,
    string? PostalCode,
    string? State,
    string? CountryCode,
    string? PhoneCountryCode,
    string? PhoneNumber
);