using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Assignments;

public sealed class AddAssignmentsLoader
    : IRequestContextLoader<AddAssignmentsCommand, AddAssignmentsContext>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;

    public AddAssignmentsLoader(
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo,
        IAssignmentsRepository assignmentsRepo)
    {
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
    }

    public async Task PopulateAsync(
        AddAssignmentsCommand command,
        AddAssignmentsContext ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _catalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found.");

        ctx.Staff = staff;
        ctx.Catalog = catalog;
        ctx.Assignments = assignments;
    }
}