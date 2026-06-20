using Domain.Roster;
using Domain.Stores.Services;
using Domain.Stores;
using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Features.Stores.Close;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IProfessionalScheduleRepository _professionalScheduleRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public Loader(
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IAssignmentsRepository assignmentsRepo,
        IProfessionalScheduleRepository professionalScheduleRepo,
        IAppointmentRepository appointmentRepo
    )
    {
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
        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found");

        var professionalSchedules = await _professionalScheduleRepo.GetByStoreIdAsync(command.StoreId, ct);

        var appointments = await _appointmentRepo.GetUpcomingByStoreIdAsync(command.StoreId, ct);

        ctx.StoreClosure = new StoreClosure(
            store,
            staff,
            assignments,
            professionalSchedules,
            appointments
        );

        ctx.Staff = staff;
    }
}
