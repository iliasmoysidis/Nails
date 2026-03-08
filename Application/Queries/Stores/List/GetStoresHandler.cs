using Application.Abstractions.Queries;
using Application.DTO.Store;

namespace Application.Queries.Stores;

public sealed class GetStoresHandler
{
    private readonly IStoreQueries _queries;

    public GetStoresHandler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StoreListItemDTO>> Handle(GetStoresQuery query, CancellationToken ct)
    {
        return await _queries.GetAllStoresAsync(ct);
    }
}