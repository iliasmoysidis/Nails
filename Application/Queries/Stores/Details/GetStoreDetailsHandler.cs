using Application.Abstractions.Queries;
using Application.DTO.Store;
using Application.Exceptions;

namespace Application.Queries.Stores;

public sealed class GetStoreDetailsHandler
{
    private readonly IStoreQueries _queries;

    public GetStoreDetailsHandler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<StoreDetailsDTO> Handle(GetStoreDetailsQuery query, CancellationToken ct)
    {
        var store = await _queries.GetStoreDetailsAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        return store;
    }
}