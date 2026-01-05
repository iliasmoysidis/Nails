using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Policies;
using Domain.Repositories;
using Domain.ValueObjects.Time;

namespace Domain.Services;

public class BookingService
{
    private readonly IStoreCatalogRepository _storeServiceRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAppointmentAuthorizationPolicy _appointmentAuthorizationPolicy;
    private readonly IStaffRepository _staffRepository;
    private readonly AppointmentOverlapPolicy _appointmentOverlapPolicy;
    private readonly IAvailabilityService _availabilityService;
    private readonly IClock _clock;

    public BookingService(IStoreCatalogRepository storeServiceRepository, IAppointmentRepository appointmentRepository, IAppointmentAuthorizationPolicy appointmentAuthorizationPolicy, IStaffRepository staffRepository, IAvailabilityService availabilityService, AppointmentOverlapPolicy appointmentOverlapPolicy, IClock clock)
    {
        _storeServiceRepository = storeServiceRepository;
        _appointmentRepository = appointmentRepository;
        _appointmentAuthorizationPolicy = appointmentAuthorizationPolicy;
        _staffRepository = staffRepository;
        _availabilityService = availabilityService;
        _appointmentOverlapPolicy = appointmentOverlapPolicy;
        _clock = clock;
    }

    public async Task<Appointment> ScheduleAppointmentAsync(int userId, int offeringId, int professionalId, int storeId, UtcDateTime startAt, string? notes = null)
    {
        var catalog = await _storeServiceRepository.GetByStoreAsync(storeId);
        var offering = catalog.GetOffering(offeringId)
            ?? throw new DomainException("Service not found.");

        var duration = offering.Duration;
        var endAt = startAt.Add(duration.Value);

        await _availabilityService.EnsureStoreIsOpenAsync(storeId, startAt, endAt);
        await _availabilityService.EnsureProfessionalIsAvailableAsync(storeId, professionalId, startAt, endAt);

        await _appointmentOverlapPolicy.EnsureNoConflictAsync(professionalId, startAt, endAt);

        var appointment = Appointment.Create(userId, professionalId, offeringId, storeId, offering.Price, startAt, duration, _clock, notes);

        await _appointmentRepository.AddAsync(appointment);

        return appointment;
    }

    public async Task<Appointment> RescheduleAppointmentAsync(int agentId, int storeId, int professionalId, int appointmentId, UtcDateTime newStartAt)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId)
            ?? throw new DomainException("Appointment not found.");

        var staff = await _staffRepository.GetByStoreAsync(storeId);
        _appointmentAuthorizationPolicy.EnsureCanModify(agentId, appointment, staff, _clock.Now);

        var catalog = await _storeServiceRepository.GetByStoreAsync(storeId);
        var offering = catalog.GetOffering(appointment.OfferingId)
            ?? throw new DomainException("Service not found.");

        var newEndAt = newStartAt.Add(offering.Duration.Value);

        await _availabilityService.EnsureStoreIsOpenAsync(storeId, newStartAt, newEndAt);
        await _availabilityService.EnsureProfessionalIsAvailableAsync(storeId, professionalId, newStartAt, newEndAt);

        await _appointmentOverlapPolicy.EnsureNoConflictAsync(professionalId, newStartAt, newEndAt, appointment.Id);

        appointment.Reschedule(newStartAt, _clock);

        await _appointmentRepository.UpdateAsync(appointment);

        return appointment;
    }

    public async Task CancelAppointmentAsync(int agentId, int storeId, int professionalId, int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId)
            ?? throw new DomainException("Appointment not found.");

        var staff = await _staffRepository.GetByStoreAsync(storeId);
        _appointmentAuthorizationPolicy.EnsureCanModify(agentId, appointment, staff, _clock.Now);

        appointment.Cancel(_clock);

        await _appointmentRepository.UpdateAsync(appointment);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetAppointmentsForProfessionalAsync(int professionalId, UtcDateTime? date = null)
    {
        return await _appointmentRepository.GetByProfessionalAsync(professionalId, date);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetAppointmentsForStoreAsync(int storeId, UtcDateTime? date = null)
    {
        return await _appointmentRepository.GetByStoreAsync(storeId, date);
    }

    public async Task<IReadOnlyCollection<Appointment>> GetAppointmentsForUserAsync(int userId, UtcDateTime? date = null)
    {
        return await _appointmentRepository.GetByUserAsync(userId, date);
    }
}