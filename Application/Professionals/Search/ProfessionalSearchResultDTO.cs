namespace Application.Professionals.Search;

public sealed record ProfessionalSearchResultDTO(
    int ProfessionalId,
    string FullName,
    string Email,
    string phone
);
