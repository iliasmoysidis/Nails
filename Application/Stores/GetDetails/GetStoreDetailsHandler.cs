using Application.Stores.Common.Queries;
using Application.Common.Exceptions;
using MediatR;

namespace Application.Stores.GetDetails;

public sealed class GetStoreDetailsHandler
    : IRequestHandler<GetStoreDetailsQuery, StoreDetailsDTO>
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