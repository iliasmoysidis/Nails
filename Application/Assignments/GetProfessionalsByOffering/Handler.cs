using Application.Assignments.Common.Queries;

namespace Application.Assignments.GetProfessionalsByOffering;

public sealed class Handler
{
    private readonly IAssignmentRegistryQueries _queries;

    public Handler(IAssignmentRegistryQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<ProfessionalSummaryDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetProfessionalsByOfferingAsync(storeId: query.StoreId, offeringId: query.OfferingId, ct: ct);
    }
}
