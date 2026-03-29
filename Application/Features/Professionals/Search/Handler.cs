using Application.Abstractions.Queries;
using Application.DTO.Professional;

namespace Application.Features.Professionals.Search;

public sealed class Handler
{
    private readonly IProfessionalQueries _queries;

    public Handler(IProfessionalQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<ProfessionalSearchResultDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.SearchProfessionalsAsync(
            name: query.Name,
            offeringId: query.OfferingId,
            city: query.City,
            storeId: query.StoreId,
            ct: ct
        );
    }
}