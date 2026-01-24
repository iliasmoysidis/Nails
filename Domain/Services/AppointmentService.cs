using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Services;

public sealed class AppointmentService
{
    private readonly IClock _clock;

    public AppointmentService(IClock clock)
    {
        _clock = clock;
    }

    public Appointment ScheduleAppointment(
        int userId,
        int professionalId,
        int offeringId,
        decimal price,
        string currency,
        UtcDateTime startAt,
        TimeSpan duration,
        string? notes = null
        )
    {
        return Appointment.Create(
            userId,
            professionalId,
            offeringId,
            price,
            currency,
            startAt,
            duration,
            _clock,
            notes
            );
    }

    public void RescheduleAppointment(Appointment appointment, UtcDateTime newStartAt)
    {
        appointment.Reschedule(newStartAt, _clock);
    }

    public void CancelAppointment(Appointment appointment, string? reason = null)
    {
        appointment.Cancel(_clock, reason);
    }
}