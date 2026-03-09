using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;

namespace Application.Commands.Assignments;

public sealed class AddAssignmentsHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public AddAssignmentsHandler(
        AuthorizationGuard auth,
        IAssignmentsRepository assignmentsRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _assignmentsRepo = assignmentsRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(AddAssignmentsCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

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