using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Stores;

public sealed class CloseHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreClosureService _service;
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IProfessionalOfferingsRepository _assignmentsRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CloseHandler(
        IManageStorePolicy policy,
        IStoreClosureService service,
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo,
        IProfessionalOfferingsRepository assignmentsRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IAppointmentRepository appointmentRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
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

    public async Task Handle(CloseCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.StoreId, ct);

        var store = await _storeRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _catalogRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Catalog not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found.");

        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct);

        var staffCalendars =
            await _staffCalendarRepo.GetByStoreId(command.StoreId, ct);

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