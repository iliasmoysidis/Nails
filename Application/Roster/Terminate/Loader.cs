using Application.Appointments.Common.Repositories;
using Application.Assignments.Common.Repositories;
using Application.Roster.Common.Repositories;
using Application.Schedule.Common.Repositories;
using Application.Stores.Common.Repositories;
using Domain.Roster;
using Domain.Professionals;
using Domain.Roster.Services;
using Domain.Stores;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Roster.Terminate;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IAssignmentRegistryRepository _assignmentRegistryRepo;
    private readonly IProfessionalScheduleRepository _professionalScheduleRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public Loader(
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IAssignmentRegistryRepository assignmentRegistryRepo,
        IProfessionalScheduleRepository professionalScheduleRepo,
        IAppointmentRepository appointmentRepo
    )
    {
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
        _assignmentRegistryRepo = assignmentRegistryRepo;
        _professionalScheduleRepo = professionalScheduleRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct)
    {
        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(
            command.StoreId,
            ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var assignments = await _assignmentRegistryRepo.GetByStoreIdAsync(
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
            store: store,
            staff: staff,
            assignments: assignments,
            professionalSchedule: professionalSchedule,
            appointments: appointments
        );
    }
}
