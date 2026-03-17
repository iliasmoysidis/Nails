using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Offerings;

public sealed class CreateOfferingLoader
    : IRequestContextLoader<CreateOfferingCommand, CreateOfferingContext>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;

    public CreateOfferingLoader(
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo)
    {
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
    }

    public async Task PopulateAsync(
        CreateOfferingCommand command,
        CreateOfferingContext ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _catalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found.");

        ctx.Staff = staff;
        ctx.Catalog = catalog;
    }
}