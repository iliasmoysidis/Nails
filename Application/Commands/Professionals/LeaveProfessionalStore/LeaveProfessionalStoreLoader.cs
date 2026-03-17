using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Professionals;

public sealed class LeaveProfessionalStoreLoader
    : IRequestContextLoader<LeaveProfessionalStoreCommand, LeaveProfessionalStoreContext>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public LeaveProfessionalStoreLoader(
        IStaffRepository staffRepo,
        IAssignmentsRepository assignmentsRepo,
        IAppointmentRepository appointmentRepo)
    {
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task PopulateAsync(
        LeaveProfessionalStoreCommand command,
        LeaveProfessionalStoreContext ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct);

        var upcoming = await _appointmentRepo.GetUpcomingByStoreIdAndProfessionalId(
            command.StoreId,
            command.ProfessionalId,
            ct
        );

        ctx.Staff = staff;
        ctx.Assignments = assignments;
        ctx.UpcomingAppointments = upcoming;
    }
}