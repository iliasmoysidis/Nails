using Application.DTO.Professional;

namespace Application.Abstractions.Queries;

public interface IProfessionalQueries
{
    Task<ProfessionalDTO?> GetProfessionalDetailsAsync(int professionalId, CancellationToken ct);

    Task<IReadOnlyCollection<ProfessionalSearchResultDTO>> SearchProfessionalsAsync(string? name, int? offeringId, string? city, int? storeId, CancellationToken ct);
}