using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Stores;

public sealed class DeleteStoreHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreRepository _storeRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IProfessionalOfferingsRepository _assignmentsRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IAppointmentCancellationService _service;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public DeleteStoreHandler(
        IManageStorePolicy policy,
        IStoreRepository storeRepo,
        IStoreCatalogRepository catalogRepo,
        IProfessionalOfferingsRepository assignmentsRepo,
        IStaffRepository staffRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IAppointmentCancellationService service,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _storeRepo = storeRepo;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
        _staffRepo = staffRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _service = service;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(DeleteStoreCommand command, CancellationToken ct)
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

        await _service.CancelUpcomingForStoreAsync(command.StoreId, _clock, ct);
        staff.Clear(_clock);
        assignments.Clear();
        catalog.Clear(_clock);
        await _storeCalendarRepo.RemoveAsync(command.StoreId, ct);
        await _staffCalendarRepo.RemoveAsync(command.StoreId, ct);
        store.SoftDelete(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}