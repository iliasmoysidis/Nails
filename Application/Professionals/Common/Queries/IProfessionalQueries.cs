using Application.Common.DTO;
using Application.Professionals.GetDetails;
using Application.Professionals.Search;

namespace Application.Professionals.Common.Queries;

public interface IProfessionalQueries
{
    Task<ProfessionalDTO?> GetProfessionalDetailsAsync(int professionalId, CancellationToken ct);

    Task<PagedResult<ProfessionalSearchResultDTO>> SearchProfessionalsAsync(
        string? name,
        string? email,
        string? phone,
        int? page,
        int? pageSize,
        CancellationToken ct
    );
}
