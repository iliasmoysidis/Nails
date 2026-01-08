using Domain.Repositories;
using Domain.ValueObjects.Time;

namespace Domain.Services.Booking;

public sealed class ContextFactory
{
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public ContextFactory(
        IStoreCatalogRepository storeCatalogRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IStaffRepository staffRepo,
        IAppointmentRepository appointmentRepo
    )
    {
        _storeCatalogRepo = storeCatalogRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _staffRepo = staffRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task<Context> CreateAsync(
        int storeId,
        int professionalId,
        UtcDateTime? date = null
    )
    {
        var catalog = await _storeCatalogRepo.GetByStoreAsync(storeId);
        var storeCalendar = await _storeCalendarRepo.GetByStoreAsync(storeId);
        var staffCalendar = await _staffCalendarRepo.GetByStoreAndProfessionalAsync(storeId, professionalId);
        var staff = await _staffRepo.GetByStoreAsync(storeId);
        var appointments = await _appointmentRepo.GetByProfessionalAsync(professionalId, date);

        return new Context(catalog, storeCalendar, staffCalendar, staff, appointments);
    }
}