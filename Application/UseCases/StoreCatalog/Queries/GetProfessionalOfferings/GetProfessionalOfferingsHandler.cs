using Application.Abstractions;
using Application.DTO;
using Application.Repositories;

namespace Application.UseCases.StoreCatalog.Queries.GetProfessionalOfferings;

public sealed class GetProfessionalOfferingsHandler
    : IQueryHandler<GetProfessionalOfferingsQuery, IReadOnlyCollection<StoreOfferingDTO>>
{
    private readonly IStoreCatalogReadRepository _repo;

    public GetProfessionalOfferingsHandler(IStoreCatalogReadRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyCollection<StoreOfferingDTO>> Handle(
        GetProfessionalOfferingsQuery query,
        CancellationToken ct
    )
    {
        return await _repo.GetProfessionalOfferingsAsync(
            query.StoreId,
            query.ProfessionalId,
            ct);
    }
}