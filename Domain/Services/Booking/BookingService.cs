using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Policies;
using Domain.Repositories;
using Domain.ValueObjects.Time;

namespace Domain.Services.Booking;

public class BookingService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAppointmentAuthorizationPolicy _appointmentAuthorizationPolicy;
    private readonly IClock _clock;
    private readonly RuleEngine _ruleEngine;
    private readonly BookingBookingContextFactory _BookingBookingContextFactory;

    public BookingService(IAppointmentRepository appointmentRepository, IAppointmentAuthorizationPolicy appointmentAuthorizationPolicy, RuleEngine ruleEngine, BookingBookingContextFactory BookingBookingContextFactory, IClock clock)
    {
        _appointmentRepository = appointmentRepository;
        _appointmentAuthorizationPolicy = appointmentAuthorizationPolicy;
        _ruleEngine = ruleEngine;
        _BookingBookingContextFactory = BookingBookingContextFactory;
        _clock = clock;
    }

    public async Task<Appointment> ScheduleAppointmentAsync(int userId, int offeringId, int professionalId, int storeId, UtcDateTime startAt, string? notes = null)
    {
        var ctx = await _BookingBookingContextFactory.CreateAsync(storeId, professionalId);

        _ruleEngine.EnsureAllSatisfied(ctx, offeringId, professionalId, startAt);

        var offering = ctx.StoreCatalog.GetOffering(offeringId)!;

        var appointment = Appointment.Create(userId, professionalId, offeringId, storeId, offering.Price, startAt, offering.Duration, _clock, notes);

        await _appointmentRepository.AddAsync(appointment);

        return appointment;
    }

    public async Task<Appointment> RescheduleAppointmentAsync(int agentId, int storeId, int professionalId, int appointmentId, UtcDateTime newStartAt)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId)
            ?? throw new DomainException("Appointment not found.");

        var ctx = await _BookingBookingContextFactory.CreateAsync(storeId, professionalId);

        _appointmentAuthorizationPolicy.EnsureCanModify(agentId, appointment, ctx.Staff, _clock.Now);

        _ruleEngine.EnsureAllSatisfied(ctx, appointment.OfferingId, professionalId, newStartAt, appointment.Id);

        appointment.Reschedule(newStartAt, _clock);

        await _appointmentRepository.UpdateAsync(appointment);

        return appointment;
    }

    public async Task CancelAppointmentAsync(int agentId, int storeId, int professionalId, int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId)
            ?? throw new DomainException("Appointment not found.");

        var ctx = await _BookingBookingContextFactory.CreateAsync(storeId, professionalId);
        _appointmentAuthorizationPolicy.EnsureCanModify(agentId, appointment, ctx.Staff, _clock.Now);

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