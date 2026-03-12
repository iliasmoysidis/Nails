using Application.Abstractions.Queries;
using Application.DTO.Professional;
using Application.Queries.Professionals;

namespace Application.Queries.Appointments;

public sealed class SearchProfessionalsHandler
{
    private readonly IProfessionalQueries _queries;

    public SearchProfessionalsHandler(IProfessionalQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<ProfessionalSearchResultDTO>> Handle(SearchProfessionalsQuery query, CancellationToken ct)
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