using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Offerings;

public sealed class DeleteOfferingLoader
    : IRequestContextLoader<DeleteOfferingCommand, DeleteOfferingContext>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;

    public DeleteOfferingLoader(
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo,
        IAssignmentsRepository assignmentsRepo)
    {
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
    }

    public async Task PopulateAsync(
        DeleteOfferingCommand command,
        DeleteOfferingContext ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _catalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Assignments not found for store {command.StoreId}.");

        ctx.Staff = staff;
        ctx.Catalog = catalog;
        ctx.Assignments = assignments;
    }
}