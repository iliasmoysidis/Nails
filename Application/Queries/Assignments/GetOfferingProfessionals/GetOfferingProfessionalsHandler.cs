using Application.Abstractions.Queries;
using Application.DTO.Assignments;

namespace Application.Queries.Assignments;

public sealed class GetOfferingProfessionalsHandler
{
    private readonly IAssignmentsQueries _queries;

    public GetOfferingProfessionalsHandler(IAssignmentsQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<OfferingProfessionalDTO>> Handle(GetOfferingProfessionalsQuery query, CancellationToken ct)
    {
        return await _queries.GetOfferingProfessionalsAsync(storeId: query.StoreId, offeringId: query.OfferingId, ct: ct);
    }
}