using Application.Catalog.Common.DTO;
using Application.Catalog.Common.Queries;
using Application.Catalog;

namespace Application.Catalog.GetStoreOfferings;

public sealed class Handler
{
    private readonly IStoreCatalogQueries _queries;

    public Handler(IStoreCatalogQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<OfferingDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetStoreOfferingsAsync(query.StoreId, ct);
    }
}