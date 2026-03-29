using Application.Abstractions.Queries;
using Application.DTO.Store;

namespace Application.Features.Stores.GetAll;

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