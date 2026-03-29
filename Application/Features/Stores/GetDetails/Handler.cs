using Application.Abstractions.Queries;
using Application.DTO.Store;
using Application.Exceptions;

namespace Application.Features.Stores.GetDetails;

public sealed class Handler
{
    private readonly IStoreQueries _queries;

    public Handler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<StoreDetailsDTO> Handle(Query query, CancellationToken ct)
    {
        var store = await _queries.GetStoreDetailsAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        return store;
    }
}