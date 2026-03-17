using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffEmployeeHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStaffRepository _staffRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IClock _clock;

    public RemoveStaffEmployeeHandler(
        AuthorizationGuard auth,
        IStaffRepository staffRepo,
        IAssignmentsRepository assignmentsRepo,
        IClock clock
    )
    {
        _auth = auth;
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
        _clock = clock;
    }

    public async Task Handle(RemoveStaffEmployeeCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Professional offerings not found for store {command.StoreId}.");

        assignments.RemoveProfessionalAssignments(command.ProfessionalId);

        staff.RemoveEmployee(command.ProfessionalId, _clock);
    }
}