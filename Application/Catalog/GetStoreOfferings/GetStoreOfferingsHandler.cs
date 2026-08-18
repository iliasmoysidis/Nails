using Application.Catalog.Common.DTO;
using Application.Catalog.Common.Queries;
using MediatR;

namespace Application.Catalog.GetStoreOfferings;

public sealed class GetStoreOfferingsHandler
    : IRequestHandler<GetStoreOfferingsQuery, IReadOnlyCollection<OfferingDTO>>
{
    private readonly IStoreCatalogQueries _queries;

    public GetStoreOfferingsHandler(IStoreCatalogQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<OfferingDTO>> Handle(GetStoreOfferingsQuery query, CancellationToken ct)
    {
        return await _queries.GetStoreOfferingsAsync(query.StoreId, ct);
    }
}
