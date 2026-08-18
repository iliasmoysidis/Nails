using Application.Assignments.Common.Queries;
using Application.Common.DTO;
using MediatR;

namespace Application.Assignments.GetProfessionalsByOffering;

public sealed class GetProfessionalsByOfferingHandler
    : IRequestHandler<GetProfessionalsByOfferingQuery, PagedResult<ProfessionalSummaryDTO>>
{
    private readonly IAssignmentRegistryQueries _queries;

    public GetProfessionalsByOfferingHandler(IAssignmentRegistryQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<ProfessionalSummaryDTO>> Handle(GetProfessionalsByOfferingQuery query, CancellationToken ct)
    {
        return await _queries.GetProfessionalsByOfferingAsync(
            storeId: query.StoreId,
            offeringId: query.OfferingId,
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
