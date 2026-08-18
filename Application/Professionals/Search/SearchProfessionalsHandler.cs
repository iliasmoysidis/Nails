using Application.Professionals.Common.Queries;
using MediatR;

namespace Application.Professionals.Search;

public sealed class SearchProfessionalsHandler
    : IRequestHandler<SearchProfessionalsQuery, IReadOnlyCollection<ProfessionalSearchResultDTO>>
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