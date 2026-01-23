using Application.Abstractions;
using Application.Contexts;
using Application.DTO;
using Application.Policies;
using Application.Repositories;

namespace Application.UseCases.StoreCatalog.Queries.GetCatalogOverview;

public sealed class GetCatalogOverviewHandler
    : IQueryHandler<GetCatalogOverviewQuery, IReadOnlyCollection<OfferingOverviewDTO>>
{
    private readonly IStoreCatalogReadRepository _repo;
    private readonly ActorContextFactory _factory;
    private readonly AuthorizationPolicy _policy;

    public GetCatalogOverviewHandler(
        IStoreCatalogReadRepository repo,
        ActorContextFactory factory,
        AuthorizationPolicy policy
        )
    {
        _repo = repo;
        _factory = factory;
        _policy = policy;
    }

    public async Task<IReadOnlyCollection<OfferingOverviewDTO>> Handle(
        GetCatalogOverviewQuery query,
        CancellationToken ct
    )
    {
        var actor = await _factory.CreateAsync(query.StoreId, ct);
        _policy.EnsureIsStoreOwner(actor);

        return await _repo.GetCatalogOverviewAsync(query.StoreId, ct);
    }
}