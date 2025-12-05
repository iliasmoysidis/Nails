using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Domain.Services;

public class BookingService
{
    private readonly IStoreCalendarRepository _storeScheduleRepository;
    private readonly IStaffCalendarRepository _storeStaffScheduleRepository;
    private readonly IStoreCatalogRepository _storeServiceRepository;
    private readonly IProfessionalAppointmentRepository _professionalAppointmentRepository;
    private readonly IAppointmentReadRepository _appointmentReadRepository;

    public BookingService(IStoreCalendarRepository storeScheduleRepository, IStaffCalendarRepository storeStaffScheduleRepository, IStoreCatalogRepository storeServiceRepository, IProfessionalAppointmentRepository professionalAppointmentRepository, IAppointmentReadRepository appointmentReadRepository)
    {
        _storeScheduleRepository = storeScheduleRepository;
        _storeStaffScheduleRepository = storeStaffScheduleRepository;
        _storeServiceRepository = storeServiceRepository;
        _professionalAppointmentRepository = professionalAppointmentRepository;
        _appointmentReadRepository = appointmentReadRepository;
    }

    public async Task<Appointment> ScheduleAppointmentAsync(int userId, int serviceId, int professionalId, int storeId, DateTime startAt, string? notes = null)
    {
        var storeCalendar = await _storeScheduleRepository.GetByStoreAsync(storeId);
        var storeStaffScheduleManager = await _storeStaffScheduleRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);
        var storeCatalog = await _storeServiceRepository.GetByStoreAsync(storeId);
        var professionalAppointments = await _professionalAppointmentRepository.GetByProfessionalAsync(professionalId);

        var service = storeCatalog.Services.FirstOrDefault(s => s.Id == serviceId && !s.IsDeleted);
        if (service == null)
        {
            throw new DomainException("Service not found.");
        }

        decimal price = service.Price;
        DateTime endAt = startAt.Add(service.Duration);

        if (!storeCalendar.IsOpenAt(startAt))
        {
            throw new DomainException("Store is closed at the requested time.");
        }

        if (!storeStaffScheduleManager.IsProfessionalAvailable(startAt, endAt))
        {
            throw new DomainException("Professional is unavailable at the requested time.");
        }

        if (!storeCatalog.ServiceIsProvidedByProfessional(professionalId, serviceId))
        {
            throw new DomainException("Service is not offered by the professional.");
        }

        if (!storeCatalog.ServiceIsProvidedByTheStore(serviceId))
        {
            throw new DomainException("Service is not offered by the store.");
        }

        var appointment = professionalAppointments.ScheduleAppointment(userId, storeId, serviceId, price, startAt, endAt, notes);

        await _professionalAppointmentRepository.SaveProfessionalAppointmentsAsync(professionalAppointments);

        return appointment;
    }

    public async Task<Appointment> RescheduleAppointmentAsync(int professionalId, int appointmentId, DateTime newStartAt)
    {
        var appointmentManager = await _professionalAppointmentRepository.GetByProfessionalAsync(professionalId);

        var appointment = appointmentManager.Appointments.FirstOrDefault(a => a.Id == appointmentId);
        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        var storeCatalog = await _storeServiceRepository.GetByStoreAsync(appointment.StoreId);
        var storeCalendar = await _storeScheduleRepository.GetByStoreAsync(appointment.StoreId);
        var storeStaffScheduleManager = await _storeStaffScheduleRepository.GetByStoreAndProfessionalAsync(appointment.StoreId, appointment.ProfessionalId);

        var service = storeCatalog.Services.First(s => s.Id == appointment.ServiceId);
        DateTime newEndAt = newStartAt.Add(service.Duration);

        if (!storeCalendar.IsOpenAt(newStartAt))
        {
            throw new DomainException("Store is closed at the requested time.");
        }

        if (!storeStaffScheduleManager.IsProfessionalAvailable(newStartAt, newEndAt))
        {
            throw new DomainException("Professional is unavailable at the requested time.");
        }

        appointmentManager.RescheduleAppointment(appointment.Id, newStartAt, newEndAt);
        await _professionalAppointmentRepository.SaveProfessionalAppointmentsAsync(appointmentManager);
        return appointment;
    }

    public async Task CancelAppointmentAsync(int professionalId, int appointmentId)
    {
        var appointmentManager = await _professionalAppointmentRepository.GetByProfessionalAsync(professionalId);

        appointmentManager.CancelAppointment(appointmentId);

        await _professionalAppointmentRepository.SaveProfessionalAppointmentsAsync(appointmentManager);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetAppointmentsForProfessionalAsync(int professionalId, DateTime? date = null)
    {
        return await _appointmentReadRepository.GetByProfessionalAsync(professionalId, date);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetAppointmentsForStoreAsync(int storeId, DateTime? date = null)
    {
        return await _appointmentReadRepository.GetByStoreAsync(storeId, date);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetAppointmentsForUserAsync(int userId, DateTime? date = null)
    {
        return await _appointmentReadRepository.GetByUserAsync(userId, date);
    }

    public async Task<bool> IsProfessionalAvailableAsync(int professionalId, int storeId, DateTime startAt, TimeSpan duration)
    {
        DateTime endAt = startAt.Add(duration);

        var staffSchedule = await _storeStaffScheduleRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);
        return staffSchedule.IsProfessionalAvailable(startAt, endAt);
    }
}