using Application.Abstractions.Queries;
using Application.DTO.Store;

namespace Application.Features.Stores.GetSummary;

public sealed class Handler
{
    private readonly IStoreQueries _queries;

    public Handler(IStoreQueries queries)
    {
        _queries = queries;
    }

    public async Task<StoreSummaryDTO?> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetStoreSummaryAsync(query.StoreId, ct);
    }
}