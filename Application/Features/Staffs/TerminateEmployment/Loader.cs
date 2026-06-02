using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Services;

namespace Application.Features.Staffs.TerminateEmployment;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IProfessionalScheduleRepository _professionalScheduleRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public Loader(
        IStaffRepository staffRepo,
        IAssignmentsRepository assignmentsRepo,
        IProfessionalScheduleRepository professionalScheduleRepo,
        IAppointmentRepository appointmentRepo)
    {
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
        _professionalScheduleRepo = professionalScheduleRepo;
        _appointmentRepo = appointmentRepo;
    }


    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(
            command.StoreId,
            ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(
            command.StoreId,
            ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found.");

        var professionalSchedule =
            await _professionalScheduleRepo.GetByProfessionalIdAsync(
                command.ProfessionalId,
                ct)
            ?? throw new ApplicationLayerNotFoundException(
                "Professional schedule not found.");

        var appointments =
            await _appointmentRepo.GetUpcomingByStoreIdAndProfessionalId(
                storeId: command.StoreId,
                professionalId: command.ProfessionalId,
                ct: ct);

        ctx.Staff = staff;

        ctx.EmploymentTermination = new EmploymentTermination(
            storeId: command.StoreId,
            professionalId: command.ProfessionalId,
            staff: staff,
            assignments: assignments,
            professionalSchedule: professionalSchedule,
            appointments: appointments
        );
    }
}
