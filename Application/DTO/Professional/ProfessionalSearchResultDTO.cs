namespace Application.DTO.Professional;

public sealed record ProfessionalSearchResultDTO(
    int ProfessionalId,
    string FullName,
    string Email,
    int StoreId,
    string StoreName,
    string City,
    string CountryCode
);