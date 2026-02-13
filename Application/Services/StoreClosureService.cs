using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public sealed class StoreClosureService : IStoreClosureService
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IProfessionalOfferingsRepository _assignmentsRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStoreRepository _storeRepo;
    private readonly IClock _clock;

    public StoreClosureService(
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo,
        IStoreCatalogRepository catalogRepo,
        IProfessionalOfferingsRepository assignmentsRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IStoreRepository storeRepo,
        IClock clock
    )
    {
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _storeRepo = storeRepo;
        _clock = clock;
    }

    public async Task CloseAsync(int storeId, CancellationToken ct)
    {
        var store = await _storeRepo.GetByStoreIdAsync(storeId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var staff = await _staffRepo.GetByStoreId(storeId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _catalogRepo.GetByStoreIdAsync(storeId, ct)
            ?? throw new ApplicationLayerNotFoundException("Catalog not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(storeId, ct)
            ?? throw new ApplicationLayerNotFoundException("Assignments not found.");

        var upcoming = await _appointmentRepo.GetUpcomingByStoreIdAsync(storeId, ct);

        foreach (var appointment in upcoming)
        {
            appointment.Cancel(_clock, "Store has been closed.");
        }

        staff.Clear(_clock);
        catalog.Clear(_clock);
        assignments.Clear();

        await _storeCalendarRepo.RemoveAsync(storeId, ct);
        await _staffCalendarRepo.RemoveAsync(storeId, ct);

        store.SoftDelete(_clock);
    }
}