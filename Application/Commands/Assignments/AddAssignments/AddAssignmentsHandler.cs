using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;

namespace Application.Commands.Assignments;

public sealed class AddAssignmentsHandler
{
    private readonly ValidationGuard _val;
    private readonly AuthorizationGuard _auth;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public AddAssignmentsHandler(
        ValidationGuard val,
        AuthorizationGuard auth,
        IStoreCatalogRepository storeCatalogRepo,
        IAssignmentsRepository assignmentsRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _val = val;
        _auth = auth;
        _storeCatalogRepo = storeCatalogRepo;
        _assignmentsRepo = assignmentsRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(AddAssignmentsCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _storeCatalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found.");

        _val.EnsureProfessionalWorksForStore(staff, command.ProfessionalId);
        foreach (var offeringId in command.OfferingIds)
        {
            _val.EnsureStoreOffersService(catalog, offeringId);
        }

        _auth.EnsureOwner(staff);

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional offerings not found.");

        foreach (var offeringId in command.OfferingIds)
        {
            assignments.Add(command.ProfessionalId, offeringId);
        }

        await _uow.SaveChangesAsync(ct);
    }
}