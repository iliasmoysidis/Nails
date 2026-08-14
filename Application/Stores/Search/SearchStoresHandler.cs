using Application.Common.DTO;
using Application.Stores.Common.DTO;
using Application.Stores.Common.Queries;

namespace Application.Stores.Search;

public sealed class SearchStoresHandler
{
    private readonly IStoreQueries _queries;

    public SearchStoresHandler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<StoreListItemDTO>> Handle(SearchStoresQuery query, CancellationToken ct)
    {
        return await _queries.SearchStoresAsync(
            name: query.Name,
            city: query.City,
            countryCode: query.CountryCode,
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
