using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.Services;

namespace Application.Features.Offerings.Create;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IStoreRepository _storeRepo;

    public Loader(
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo,
        IStoreRepository storeRepo
    )
    {
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
        _storeRepo = storeRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _catalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found.");

        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new InvariantException("Store not found.");

        ctx.Staff = staff;
        ctx.StoreOfferings = new StoreOfferings(store, catalog);
    }
}
