namespace Application.DTO.Store;

public sealed record StoreListItemDTO(
    int Id,
    string Name,
    string City,
    string CountryCode
);