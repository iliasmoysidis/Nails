using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Offerings;

public sealed class UpdateOfferingLoader
    : IRequestContextLoader<UpdateOfferingCommand, UpdateOfferingContext>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;

    public UpdateOfferingLoader(
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo)
    {
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
    }

    public async Task PopulateAsync(
        UpdateOfferingCommand command,
        UpdateOfferingContext ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _catalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

        ctx.Staff = staff;
        ctx.Catalog = catalog;
    }
}