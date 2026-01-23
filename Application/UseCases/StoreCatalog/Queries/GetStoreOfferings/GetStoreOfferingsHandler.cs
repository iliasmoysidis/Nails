using Application.Abstractions;
using Application.DTO;
using Application.Repositories;

namespace Application.UseCases.StoreCatalog.Queries.GetStoreOfferings;

public sealed class GetStoreOfferingsHandler
    : IQueryHandler<GetStoreOfferingsQuery, IReadOnlyCollection<OfferingDetailsDTO>>
{
    private readonly IStoreCatalogReadRepository _repo;

    public GetStoreOfferingsHandler(IStoreCatalogReadRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyCollection<OfferingDetailsDTO>> Handle(
        GetStoreOfferingsQuery query,
        CancellationToken ct
    )
    {
        return await _repo.GetStoreOfferingsAsync(query.StoreId, ct);
    }
}