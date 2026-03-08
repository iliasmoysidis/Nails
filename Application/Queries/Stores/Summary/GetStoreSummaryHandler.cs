using Application.Abstractions.Queries;
using Application.DTO.Store;

namespace Application.Queries.Stores;

public sealed class GetStoreSummaryHandler
{
    private readonly IStoreQueries _queries;

    public GetStoreSummaryHandler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<StoreSummaryDTO?> Handle(GetStoreSummaryQuery query, CancellationToken ct)
    {
        return await _queries.GetStoreSummaryAsync(query.StoreId, ct);
    }
}