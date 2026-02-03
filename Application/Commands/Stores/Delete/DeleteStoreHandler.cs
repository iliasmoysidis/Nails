using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Stores;

public sealed class DeleteStoreHandler
{
    private readonly IStoreRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public DeleteStoreHandler(
        IStoreRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(DeleteStoreCommand command, CancellationToken ct)
    {
        var store = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        store.SoftDelete(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}