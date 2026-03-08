using Application.Abstractions.Queries;
using Application.DTO.Store;

namespace Application.Queries.Stores;

public sealed class SearchStoresHandler
{
    private readonly IStoreQueries _queries;

    public SearchStoresHandler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StoreListItemDTO>> Handle(SearchStoresQuery query, CancellationToken ct)
    {
        return await _queries.SearchStoresAsync(query.Name, query.City, query.CountryCode, ct);
    }
}