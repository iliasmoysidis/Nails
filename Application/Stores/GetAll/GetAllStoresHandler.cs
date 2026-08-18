using Application.Common.DTO;
using Application.Stores.Common.DTO;
using Application.Stores.Common.Queries;
using MediatR;

namespace Application.Stores.GetAll;

public sealed class GetAllStoresHandler
    : IRequestHandler<GetAllStoresQuery, PagedResult<StoreListItemDTO>>
{
    private readonly IStoreQueries _queries;

    public GetAllStoresHandler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<StoreListItemDTO>> Handle(GetAllStoresQuery query, CancellationToken ct)
    {
        return await _queries.GetStoresAsync(
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
