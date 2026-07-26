using Application.Common.DTO;
using Application.Stores.Common.DTO;
using Application.Stores.Common.Queries;

namespace Application.Stores.GetAll;

public sealed class Handler
{
    private readonly IStoreQueries _queries;

    public Handler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<StoreListItemDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetStoresAsync(
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
