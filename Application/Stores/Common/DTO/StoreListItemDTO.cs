namespace Application.Stores.Common.DTO;

public sealed record StoreListItemDTO(
    int Id,
    string Name,
    string City,
    string CountryCode
);