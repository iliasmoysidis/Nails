using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Stores;

public sealed class CloseStoreLoader
    : IRequestContextLoader<CloseStoreCommand, CloseStoreContext>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public CloseStoreLoader(
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo,
        IAssignmentsRepository assignmentsRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IAppointmentRepository appointmentRepo)
    {
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task PopulateAsync(
        CloseStoreCommand command,
        CloseStoreContext ctx,
        CancellationToken ct)
    {
        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.Store = store;
        ctx.Staff = staff;
        ctx.Catalog = await _catalogRepo.GetByIdAsync(command.StoreId, ct);
        ctx.Assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct);
        ctx.StoreCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct);
        ctx.StaffCalendars = await _staffCalendarRepo.GetByStoreIdAsync(command.StoreId, ct);
        ctx.UpcomingAppointments =
            await _appointmentRepo.GetUpcomingByStoreIdAsync(command.StoreId, ct);
    }
}