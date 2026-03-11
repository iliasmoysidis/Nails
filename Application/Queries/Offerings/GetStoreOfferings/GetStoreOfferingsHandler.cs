using Application.Abstractions.Queries;
using Application.DTO.Offering;

namespace Application.Queries.Offerings;

public sealed class GetStoreOfferingsHandler
{
    private readonly IOfferingQueries _queries;

    public GetStoreOfferingsHandler(IOfferingQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<OfferingDTO>> Handle(GetStoreOfferingsQuery query, CancellationToken ct)
    {
        return await _queries.GetStoreOfferingsAsync(query.StoreId, ct);
    }
}