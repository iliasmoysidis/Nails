using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;

namespace Application.Commands.Assignments;

public sealed class RemoveAssignmentsHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IAssignmentsRepository _professionalRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public RemoveAssignmentsHandler(
        AuthorizationGuard auth,
        IAssignmentsRepository professionalRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _professionalRepo = professionalRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(RemoveAssignmentsCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        var assignments = await _professionalRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional offerings not found.");

        foreach (var offeringId in command.OfferingIds)
        {
            assignments.Remove(command.ProfessionalId, offeringId);
        }

        await _uow.SaveChangesAsync(ct);
    }
}