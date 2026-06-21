using Application.Stores.Common.DTO;
using Application.Stores.Common.Queries;
using Application.Stores;

namespace Application.Stores.GetAll;

public sealed class Handler
{
    private readonly IStoreQueries _queries;

    public Handler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StoreListItemDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetAllStoresAsync(ct);
    }
}