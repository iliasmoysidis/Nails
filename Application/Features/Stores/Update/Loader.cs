using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Features.Stores.Update;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;

    public Loader(
        IStoreRepository storeRepo,
        IStaffRepository staffRepo)
    {
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        ctx.Staff = staff;
        ctx.Store = store;
    }
}