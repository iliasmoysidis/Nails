using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Stores;

public sealed class CloseStoreHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStoreClosureService _service;
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IAssignmentsRepository _assignmentsRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CloseStoreHandler(
        AuthorizationGuard auth,
        IStoreClosureService service,
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo,
        IAssignmentsRepository assignmentsRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IAppointmentRepository appointmentRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _service = service;
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _appointmentRepo = appointmentRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(CloseStoreCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var catalog = await _catalogRepo.GetByIdAsync(command.StoreId, ct);

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct);

        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct);

        var staffCalendars =
            await _staffCalendarRepo.GetByStoreIdAsync(command.StoreId, ct);

        var upcoming =
            await _appointmentRepo.GetUpcomingByStoreIdAsync(command.StoreId, ct);

        _service.CloseStore(
            store,
            staff,
            catalog,
            assignments,
            storeCalendar,
            staffCalendars,
            upcoming,
            _clock
        );

        await _storeCalendarRepo.RemoveAsync(command.StoreId, ct);
        await _staffCalendarRepo.RemoveAsync(command.StoreId, ct);


        await _uow.SaveChangesAsync(ct);
    }
}