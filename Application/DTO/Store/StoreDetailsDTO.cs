namespace Application.DTO.Store;

public sealed record StoreDetailsDTO(
    int Id,
    string Name,
    string Email,
    string Phone,
    string Street,
    string City,
    string PostalCode,
    string State,
    string CountryCode,
    string TaxId
);