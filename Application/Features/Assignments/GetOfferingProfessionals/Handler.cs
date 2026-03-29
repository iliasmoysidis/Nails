using Application.Abstractions.Queries;
using Application.DTO.Assignments;

namespace Application.Features.Assignments.GetOfferingProfessionals;

public sealed class Handler
{
    private readonly IAssignmentsQueries _queries;

    public Handler(IAssignmentsQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<OfferingProfessionalDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetOfferingProfessionalsAsync(storeId: query.StoreId, offeringId: query.OfferingId, ct: ct);
    }
}