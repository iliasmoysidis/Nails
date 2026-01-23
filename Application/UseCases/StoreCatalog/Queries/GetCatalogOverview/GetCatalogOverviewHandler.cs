using Application.Abstractions;
using Application.DTO;
using Application.Policies.Interfaces;
using Application.Repositories;

namespace Application.UseCases.StoreCatalog.Queries.GetCatalogOverview;

public sealed class GetCatalogOverviewHandler
    : IQueryHandler<GetCatalogOverviewQuery, IReadOnlyCollection<OfferingOverviewDTO>>
{
    private readonly IStoreCatalogReadRepository _repo;
    private readonly IStoreOwnerAccessPolicy _policy;
    private readonly ICurrentUser _currentUser;

    public GetCatalogOverviewHandler(
        IStoreCatalogReadRepository repo,
        IStoreOwnerAccessPolicy policy,
        ICurrentUser currentUser
        )
    {
        _repo = repo;
        _policy = policy;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<OfferingOverviewDTO>> Handle(
        GetCatalogOverviewQuery query,
        CancellationToken ct
    )
    {
        await _policy.EnsureIsOwnerAsync(_currentUser.UserId, query.StoreId, ct);
        return await _repo.GetCatalogOverviewAsync(query.StoreId, ct);
    }
}