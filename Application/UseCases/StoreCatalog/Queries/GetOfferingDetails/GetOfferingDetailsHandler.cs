using Application.Abstractions;
using Application.DTO;
using Application.Repositories;

namespace Application.UseCases.StoreCatalog.Queries.GetOfferingDetails;

public sealed class GetOfferingDetailsHandler
    : IQueryHandler<GetOfferingDetailsQuery, OfferingDetailsDTO?>
{
    private readonly IStoreCatalogReadRepository _repo;

    public GetOfferingDetailsHandler(IStoreCatalogReadRepository repo)
    {
        _repo = repo;
    }

    public async Task<OfferingDetailsDTO?> Handle(
        GetOfferingDetailsQuery query,
        CancellationToken ct
    )
    {
        return await _repo.GetOfferingDetailsAsync(query.OfferingId, ct);
    }
}