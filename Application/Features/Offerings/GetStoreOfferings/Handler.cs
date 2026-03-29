using Application.Abstractions.Queries;
using Application.DTO.Offering;

namespace Application.Features.Offerings.GetStoreOfferings;

public sealed class Handler
{
    private readonly IOfferingQueries _queries;

    public Handler(IOfferingQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<OfferingDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetStoreOfferingsAsync(query.StoreId, ct);
    }
}