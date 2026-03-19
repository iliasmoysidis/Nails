using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Stores;

public sealed class CloseStoreLoader
    : IRequestContextLoader<CloseStoreCommand, CloseStoreContext>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;

    public CloseStoreLoader(
        IStoreRepository storeRepo,
        IStaffRepository staffRepo)
    {
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
    }

    public async Task PopulateAsync(
        CloseStoreCommand command,
        CloseStoreContext ctx,
        CancellationToken ct)
    {
        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.Store = store;
        ctx.Staff = staff;
    }
}