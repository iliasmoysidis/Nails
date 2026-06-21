using Application.Assignments.Common.Repositories;
using Application.Catalog.Common.Repositories;
using Application.Professionals.Common.Repositories;
using Application.Roster.Common.Repositories;
using Application.Stores.Common.Repositories;
using Domain.Roster;
using Domain.Professionals;
using Domain.Stores.Services;
using Domain.Stores;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Assignments.Remove;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IProfessionalRepository _professionalRepo;
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;

    public Loader(
        IProfessionalRepository professionalRepo,
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo,
        IAssignmentsRepository assignmentsRepo
    )
    {
        _professionalRepo = professionalRepo;
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct
    )
    {
        var professional = await _professionalRepo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _catalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found.");

        ctx.StoreAssignments = new StoreAssignments(
            professional: professional,
            store: store,
            staff: staff,
            storeCatalog: catalog,
            assignments: assignments
        );
    }
}
