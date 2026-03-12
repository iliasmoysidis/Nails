using Application.Abstractions.Queries;
using Application.DTO.Assignments;
using Application.DTO.Offering;

namespace Application.Queries.Assignments;

public sealed class GetProfessionalOfferingsHandler
{
    private readonly IAssignmentsQueries _queries;

    public GetProfessionalOfferingsHandler(IAssignmentsQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<ProfessionalOfferingDTO>> Handle(GetProfessionalOfferingsQuery query, CancellationToken ct)
    {
        return await _queries.GetProfessionalOfferingsAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            ct: ct
        );
    }
}