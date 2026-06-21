using Application.Appointments.Common.Repositories;
using Application.Assignments.Common.Repositories;
using Application.Professionals.Common.Repositories;
using Application.Roster.Common.Repositories;
using Application.Schedule.Common.Repositories;
using Application.Stores.Common.Repositories;
using Domain.Roster;
using Domain.Professionals.Services;
using Domain.Professionals;
using Domain.Roster.Services;
using Domain.Stores;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Professionals.Delete;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IProfessionalRepository _professionalRepo;
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IProfessionalScheduleRepository _professionalScheduleRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public Loader(
        IProfessionalRepository professionalRepo,
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IAssignmentsRepository assignmentsRepo,
        IProfessionalScheduleRepository professionalScheduleRepo,
        IAppointmentRepository appointmentRepo
    )
    {
        _professionalRepo = professionalRepo;
        _storeRepo = storeRepo;
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
        var professional = await _professionalRepo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        var ownedStores = await _storeRepo.GetOwnedStores(command.ProfessionalId, ct);

        var schedule = await _professionalScheduleRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional schedule not found.");

        var employments = new List<EmploymentTermination>();

        foreach (var calendar in schedule.Calendars)
        {
            var store = await _storeRepo.GetByIdAsync(calendar.StoreId, ct)
                ?? throw new ApplicationLayerNotFoundException("Store not found.");

            var staff = await _staffRepo.GetByStoreIdAsync(calendar.StoreId, ct)
                ?? throw new ApplicationLayerNotFoundException("Staff not found.");

            var assignments = await _assignmentsRepo.GetByStoreIdAsync(calendar.StoreId, ct)
                ?? throw new ApplicationLayerNotFoundException("Assignments not found.");

            var appointments = await _appointmentRepo.GetUpcomingByStoreIdAndProfessionalId(
                    storeId: calendar.StoreId,
                    professionalId: command.ProfessionalId,
                    ct: ct
                );

            var employment = new EmploymentTermination(
                store: store,
                staff: staff,
                assignments: assignments,
                professionalSchedule: schedule,
                appointments: appointments
            );

            employments.Add(employment);
        }

        ctx.ProfessionalDeletion = new ProfessionalDeletion(professional, ownedStores, employments);
    }
}
