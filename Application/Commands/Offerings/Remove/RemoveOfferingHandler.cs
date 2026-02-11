using Application.Abstractions.Policies.Offerings;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Application.Commands.Offerings;

public sealed class RemoveOfferingHandler
{
    private readonly IRemoveOfferingPolicy _policy;
    private readonly IStoreCatalogRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RemoveOfferingHandler(
        IRemoveOfferingPolicy policy,
        IStoreCatalogRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(RemoveOfferingCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanRemoveAsync(command, ct);

        var catalog = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

        catalog.RemoveOffering(command.OfferingId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}