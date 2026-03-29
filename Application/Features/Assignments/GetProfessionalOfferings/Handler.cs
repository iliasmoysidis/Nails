using Application.Abstractions.Queries;
using Application.DTO.Assignments;

namespace Application.Features.Assignments.GetProfessionalOfferings;

public sealed class Handler
{
    private readonly IAssignmentsQueries _queries;

    public Handler(IAssignmentsQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<ProfessionalOfferingDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetProfessionalOfferingsAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            ct: ct
        );
    }
}