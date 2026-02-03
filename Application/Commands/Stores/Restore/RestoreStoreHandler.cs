using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Stores;

public sealed class RestoreStoreHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RestoreStoreHandler(
        IManageStorePolicy policy,
        IStoreRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(RestoreStoreCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.StoreId, ct);

        var store = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        store.Restore(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}