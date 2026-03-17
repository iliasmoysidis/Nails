using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Stores;

public sealed class UpdateStoreLoader
    : IRequestContextLoader<UpdateStoreCommand, UpdateStoreContext>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;

    public UpdateStoreLoader(
        IStoreRepository storeRepo,
        IStaffRepository staffRepo)
    {
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
    }

    public async Task PopulateAsync(
        UpdateStoreCommand command,
        UpdateStoreContext ctx,
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