using Application.Assignments.GetOfferingProfessionals;
using Application.Assignments.GetProfessionalOfferings;

namespace Application.Assignments.Common.Queries;

public interface IAssignmentsQueries
{
    Task<IReadOnlyCollection<ProfessionalOfferingDTO>> GetProfessionalOfferingsAsync(
        int storeId,
        int professionalId,
        CancellationToken ct
    );

    Task<IReadOnlyCollection<OfferingProfessionalDTO>> GetOfferingProfessionalsAsync(
        int storeId,
        int offeringId,
        CancellationToken ct
    );
}