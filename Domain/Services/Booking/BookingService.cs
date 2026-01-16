using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Services.Booking;

public sealed class BookingService
{
    private readonly IAppointmentAuthorizationPolicy _appointmentAuthorizationPolicy;
    private readonly IClock _clock;
    private readonly RuleEngine _ruleEngine;

    public BookingService(IAppointmentAuthorizationPolicy appointmentAuthorizationPolicy, RuleEngine ruleEngine, IClock clock)
    {
        _appointmentAuthorizationPolicy = appointmentAuthorizationPolicy;
        _ruleEngine = ruleEngine;
        _clock = clock;
    }

    public Appointment ScheduleAppointment(BookingContext ctx, int userId, int offeringId, int professionalId, int storeId, UtcDateTime startAt, string? notes = null)
    {
        _ruleEngine.EnsureAllSatisfied(ctx, offeringId, professionalId, startAt);
        var offering = ctx.StoreCatalog.GetOffering(offeringId)
            ?? throw new DomainException("Offering not found.");

        return Appointment.Create(userId, professionalId, offeringId, storeId, offering.Price, startAt, offering.Duration, _clock, notes);
    }

    public void RescheduleAppointment(BookingContext ctx, Appointment appointment, int agentId, int professionalId, UtcDateTime newStartAt)
    {
        _appointmentAuthorizationPolicy.EnsureCanModify(agentId, appointment, ctx.Staff, _clock.Now);
        _ruleEngine.EnsureAllSatisfied(ctx, appointment.OfferingId, professionalId, newStartAt, appointment.Id);
        appointment.Reschedule(newStartAt, _clock);
    }

    public void CancelAppointment(BookingContext ctx, Appointment appointment, int agentId)
    {
        _appointmentAuthorizationPolicy.EnsureCanModify(agentId, appointment, ctx.Staff, _clock.Now);
        appointment.Cancel(_clock);
    }
}