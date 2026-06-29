using Application.Assignments.Common.Queries;

namespace Application.Assignments.GetOfferingsByProfessional;

public sealed class Handler
{
    private readonly IAssignmentRegistryQueries _queries;

    public Handler(IAssignmentRegistryQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<OfferingSummaryDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetOfferingsByProfessionalAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            ct: ct
        );
    }
}
