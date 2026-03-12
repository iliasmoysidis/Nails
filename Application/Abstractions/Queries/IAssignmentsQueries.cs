using Application.DTO.Assignments;

namespace Application.Abstractions.Queries;

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