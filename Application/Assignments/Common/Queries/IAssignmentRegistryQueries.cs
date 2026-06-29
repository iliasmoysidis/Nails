using Application.Assignments.GetProfessionalsByOffering;
using Application.Assignments.GetOfferingsByProfessional;

namespace Application.Assignments.Common.Queries;

public interface IAssignmentRegistryQueries
{
    Task<IReadOnlyCollection<OfferingSummaryDTO>> GetOfferingsByProfessionalAsync(
        int storeId,
        int professionalId,
        CancellationToken ct
    );

    Task<IReadOnlyCollection<ProfessionalSummaryDTO>> GetProfessionalsByOfferingAsync(
        int storeId,
        int offeringId,
        CancellationToken ct
    );
}
