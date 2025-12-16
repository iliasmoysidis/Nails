using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Repositories;
using Domain.ValueObjects.Time;

namespace Domain.Services;

public class BookingService
{
    private readonly IStoreCatalogRepository _storeServiceRepository;
    private readonly IProfessionalAgendaRepository _professionalAgendaRepository;
    private readonly IAppointmentReadRepository _appointmentReadRepository;
    private readonly IAppointmentAuthorizationPolicy _appointmentAuthorizationPolicy;
    private readonly IStaffRepository _staffRepository;
    private readonly IAvailabilityService _availabilityService;
    private readonly IClock _clock;

    public BookingService(IStoreCatalogRepository storeServiceRepository, IProfessionalAgendaRepository professionalAgendaRepository, IAppointmentReadRepository appointmentReadRepository, IAppointmentAuthorizationPolicy appointmentAuthorizationPolicy, IStaffRepository staffRepository, IAvailabilityService availabilityService, IClock clock)
    {
        _storeServiceRepository = storeServiceRepository;
        _professionalAgendaRepository = professionalAgendaRepository;
        _appointmentReadRepository = appointmentReadRepository;
        _appointmentAuthorizationPolicy = appointmentAuthorizationPolicy;
        _staffRepository = staffRepository;
        _availabilityService = availabilityService;
        _clock = clock;
    }

    public async Task<Appointment> ScheduleAppointmentAsync(int userId, int offeringId, int professionalId, int storeId, UtcDateTime startAt, string? notes = null)
    {
        var catalog = await _storeServiceRepository.GetByStoreAsync(storeId);
        var agenda = await _professionalAgendaRepository.GetAsync(professionalId);

        var offering = catalog.GetOffering(offeringId)
            ?? throw new DomainException("Service not found.");

        var endAt = startAt.Add(offering.Duration);

        await _availabilityService.EnsureStoreIsOpenAsync(storeId, startAt, endAt);
        await _availabilityService.EnsureProfessionalIsAvailableAsync(storeId, professionalId, startAt, endAt);

        var appointment = agenda.Schedule(userId, storeId, offeringId, offering.Price, startAt, endAt, _clock, notes);

        await _professionalAgendaRepository.SaveAsync(agenda);

        return appointment;
    }

    public async Task<Appointment> RescheduleAppointmentAsync(int agentId, int storeId, int professionalId, int appointmentId, UtcDateTime newStartAt)
    {
        var staff = await _staffRepository.GetByStoreAsync(storeId);
        var agenda = await _professionalAgendaRepository.GetAsync(professionalId);

        var appointment = agenda.GetActive(appointmentId);

        _appointmentAuthorizationPolicy.EnsureCanModify(agentId, appointment, staff, _clock.Now);

        var catalog = await _storeServiceRepository.GetByStoreAsync(storeId);
        var offering = catalog.GetOffering(appointment.OfferingId)
            ?? throw new DomainException("Service not found.");

        var newEndAt = newStartAt.Add(offering.Duration);

        await _availabilityService.EnsureStoreIsOpenAsync(storeId, newStartAt, newEndAt);
        await _availabilityService.EnsureProfessionalIsAvailableAsync(storeId, professionalId, newStartAt, newEndAt);

        agenda.Reschedule(appointment.Id, newStartAt, newEndAt, _clock);

        await _professionalAgendaRepository.SaveAsync(agenda);

        return appointment;
    }

    public async Task CancelAppointmentAsync(int agentId, int storeId, int professionalId, int appointmentId)
    {
        var staff = await _staffRepository.GetByStoreAsync(storeId);
        var agenda = await _professionalAgendaRepository.GetAsync(professionalId);

        var appointment = agenda.GetActive(appointmentId);
        _appointmentAuthorizationPolicy.EnsureCanModify(agentId, appointment, staff, _clock.Now);

        agenda.Cancel(appointmentId, _clock);

        await _professionalAgendaRepository.SaveAsync(agenda);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetAppointmentsForProfessionalAsync(int professionalId, UtcDateTime? date = null)
    {
        return await _appointmentReadRepository.GetByProfessionalAsync(professionalId, date);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetAppointmentsForStoreAsync(int storeId, UtcDateTime? date = null)
    {
        return await _appointmentReadRepository.GetByStoreAsync(storeId, date);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetAppointmentsForUserAsync(int userId, UtcDateTime? date = null)
    {
        return await _appointmentReadRepository.GetByUserAsync(userId, date);
    }
}