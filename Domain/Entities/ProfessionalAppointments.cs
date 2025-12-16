using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class ProfessionalAppointments
{
    public int ProfessionalId { get; private set; }

    private readonly List<Appointment> _appointments = new();
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    private ProfessionalAppointments() { }

    public static ProfessionalAppointments Create(int professionalId)
    {
        return new ProfessionalAppointments
        {
            ProfessionalId = professionalId
        };
    }

    public Appointment ScheduleAppointment(int userId, int storeId, int serviceId, decimal price, UtcDateTime startAt, UtcDateTime endAt, IClock clock, string? notes = null)
    {
        if (HasConflict(startAt, endAt))
        {
            throw new DomainException("Professional already has an appointment at this time.");
        }

        var appointment = Appointment.Create(userId, ProfessionalId, serviceId, storeId, price, startAt, endAt, clock, notes);
        _appointments.Add(appointment);

        return appointment;
    }

    public Appointment RescheduleAppointment(int appointmentId, UtcDateTime newStartAt, UtcDateTime newEndAt, IClock clock)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId && !a.IsDeleted);
        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        if (HasConflictExcludingAppointment(appointmentId, newStartAt, newEndAt))
        {
            throw new DomainException("Reschedule conflict: professional already has an appointment at this time.");
        }

        appointment.Reschedule(clock, newStartAt, newEndAt);
        return appointment;
    }

    public void CancelAppointment(int appointmentId, IClock clock, string? reason = null)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId && !a.IsDeleted);

        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        appointment.Cancel(clock, reason);
    }

    public bool HasConflict(UtcDateTime startAt, UtcDateTime endAt)
    {
        return _appointments.Any(
            a => startAt < a.EndAt &&
            endAt > a.StartAt &&
            (a.Status == AppointmentStatus.Confirmed ||
            a.Status == AppointmentStatus.PendingConfirmation));
    }

    public bool HasConflictExcludingAppointment(int appointmentId, UtcDateTime startAt, UtcDateTime endAt)
    {
        return _appointments.Any(a => a.Id != appointmentId &&
        !a.IsDeleted &&
        a.Status == AppointmentStatus.Confirmed &&
        startAt < a.EndAt &&
        endAt > a.StartAt);
    }
}