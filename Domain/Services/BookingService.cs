using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Repositories;
using Domain.ValueObjects;
using Domain.ValueObjects.Time;

namespace Domain.Services;

public class BookingService
{
    private readonly IStoreCatalogRepository _storeServiceRepository;
    private readonly IProfessionalAppointmentRepository _professionalAppointmentRepository;
    private readonly IAppointmentReadRepository _appointmentReadRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IAvailabilityService _availabilityService;
    private readonly IClock _clock;

    public BookingService(IStoreCatalogRepository storeServiceRepository, IProfessionalAppointmentRepository professionalAppointmentRepository, IAppointmentReadRepository appointmentReadRepository, IStaffRepository staffRepository, IAvailabilityService availabilityService, IClock clock)
    {
        _storeServiceRepository = storeServiceRepository;
        _professionalAppointmentRepository = professionalAppointmentRepository;
        _appointmentReadRepository = appointmentReadRepository;
        _staffRepository = staffRepository;
        _availabilityService = availabilityService;
        _clock = clock;
    }

    public async Task<Appointment> ScheduleAppointmentAsync(int userId, int offeringId, int professionalId, int storeId, UtcDateTime startAt, string? notes = null)
    {
        var catalog = await _storeServiceRepository.GetByStoreAsync(storeId);
        var appointments = await _professionalAppointmentRepository.GetByProfessionalAsync(professionalId);

        var service = catalog.GetOffering(offeringId)
            ?? throw new DomainException("Service not found.");

        var endAt = startAt.Add(service.Duration);

        await _availabilityService.EnsureStoreIsOpenAsync(storeId, startAt, endAt);
        await _availabilityService.EnsureProfessionalIsAvailableAsync(storeId, professionalId, startAt, endAt);

        var appointment = appointments.ScheduleAppointment(userId, storeId, offeringId, service.Price, startAt, endAt, _clock, notes);

        await _professionalAppointmentRepository.SaveProfessionalAppointmentsAsync(appointments);

        return appointment;
    }

    public async Task<Appointment> RescheduleAppointmentAsync(int agentId, int storeId, int professionalId, int appointmentId, UtcDateTime newStartAt)
    {
        var staff = await _staffRepository.GetByStoreAsync(storeId);
        var appointments = await _professionalAppointmentRepository.GetByProfessionalAsync(professionalId);

        var appointment = FindAppointment(appointments, appointmentId, storeId, professionalId);

        EnsureAgentCanModifyAppointment(agentId, appointment, staff, _clock.Now);

        var catalog = await _storeServiceRepository.GetByStoreAsync(storeId);
        var service = catalog.GetOffering(appointment.OfferingId)
            ?? throw new DomainException("Service not found.");

        var newEndAt = newStartAt.Add(service.Duration);

        await _availabilityService.EnsureStoreIsOpenAsync(storeId, newStartAt, newEndAt);
        await _availabilityService.EnsureProfessionalIsAvailableAsync(storeId, professionalId, newStartAt, newEndAt);

        appointments.RescheduleAppointment(appointment.Id, newStartAt, newEndAt, _clock);

        await _professionalAppointmentRepository.SaveProfessionalAppointmentsAsync(appointments);

        return appointment;
    }

    public async Task CancelAppointmentAsync(int agentId, int storeId, int professionalId, int appointmentId)
    {
        var staff = await _staffRepository.GetByStoreAsync(storeId);
        var appointments = await _professionalAppointmentRepository.GetByProfessionalAsync(professionalId);

        var appointment = FindAppointment(appointments, appointmentId, storeId, professionalId);

        EnsureAgentCanModifyAppointment(agentId, appointment, staff, _clock.Now);

        appointments.CancelAppointment(appointment.Id, _clock);

        await _professionalAppointmentRepository.SaveProfessionalAppointmentsAsync(appointments);
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

    private static void EnsureAgentCanModifyAppointment(int agentId, Appointment appointment, Staff staff, UtcDateTime now)
    {
        if (!(agentId == appointment.UserId || staff.IsOwner(agentId)))
        {
            throw new DomainException("The user is not authorized to modify this appointment.");
        }

        var hours = (appointment.StartAt - now).TotalHours;

        if (hours <= 0)
        {
            throw new DomainException("Appointment has already started.");
        }

        if (!staff.IsOwner(agentId))
        {
            if (hours < 24 && hours > 0)
                throw new DomainException("Only an owner can modify appointments within 24 hours.");
        }
    }

    private static Appointment FindAppointment(ProfessionalAppointments appointments, int appointmentId, int storeId, int professionalId)
    {
        return appointments.Appointments.FirstOrDefault(a =>
            a.Id == appointmentId &&
            !a.IsDeleted &&
            a.ProfessionalId == professionalId &&
            a.StoreId == storeId)
            ?? throw new DomainException("Appointment not found.");
    }
}