namespace Application.DTO.Staff;

public sealed record ProfessionalStoreDTO(
    int StoreId,
    string StoreName,
    string City,
    string CountryCode,
    bool IsOwner,
    bool IsEmployee
);