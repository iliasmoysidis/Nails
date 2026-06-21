using Application.Assignments.Common.Repositories;
using Application.Catalog.Common.Repositories;
using Application.Roster.Common.Repositories;
using Application.Stores.Common.Repositories;
using Domain.Roster;
using Domain.Catalog.Services;
using Domain.Stores;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Catalog.Remove;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IStoreRepository _storeRepo;

    public Loader(
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo,
        IAssignmentsRepository assignmentsRepo,
        IStoreRepository storeRepo
    )
    {
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
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
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found.");

        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        ctx.Staff = staff;
        ctx.StoreOfferingRemoval = new StoreOfferingRemoval(
            store: store,
            assignments: assignments,
            catalog: catalog
        );
    }
}
