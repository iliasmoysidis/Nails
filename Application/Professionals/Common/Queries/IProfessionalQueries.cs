using Application.Professionals.GetDetails;
using Application.Professionals.Search;

namespace Application.Professionals.Common.Queries;

public interface IProfessionalQueries
{
    Task<ProfessionalDTO?> GetProfessionalDetailsAsync(int professionalId, CancellationToken ct);

    Task<IReadOnlyCollection<ProfessionalSearchResultDTO>> SearchProfessionalsAsync(string? name, int? offeringId, string? city, int? storeId, CancellationToken ct);
}