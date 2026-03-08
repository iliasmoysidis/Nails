namespace Application.DTO.Store;

public sealed record StoreSummaryDTO(
    int Id,
    string Name,
    string City,
    string CountryCode
);