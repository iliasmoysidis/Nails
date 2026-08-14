using Application.Assignments.Common.Queries;
using Application.Common.DTO;

namespace Application.Assignments.GetOfferingsByProfessional;

public sealed class GetOfferingsByProfessionalHandler
{
    private readonly IAssignmentRegistryQueries _queries;

    public GetOfferingsByProfessionalHandler(IAssignmentRegistryQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<OfferingSummaryDTO>> Handle(GetOfferingsByProfessionalQuery query, CancellationToken ct)
    {
        return await _queries.GetOfferingsByProfessionalAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
