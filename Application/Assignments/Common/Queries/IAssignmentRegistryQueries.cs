using Application.Assignments.GetProfessionalsByOffering;
using Application.Assignments.GetOfferingsByProfessional;
using Application.Common.DTO;

namespace Application.Assignments.Common.Queries;

public interface IAssignmentRegistryQueries
{
    Task<PagedResult<OfferingSummaryDTO>> GetOfferingsByProfessionalAsync(
        int storeId,
        int professionalId,
        int? page,
        int? limit,
        CancellationToken ct
    );

    Task<PagedResult<ProfessionalSummaryDTO>> GetProfessionalsByOfferingAsync(
        int storeId,
        int offeringId,
        int? page,
        int? limit,
        CancellationToken ct
    );
}
