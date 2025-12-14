using Domain.Enums;
using Domain.Exceptions;

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

    public Appointment ScheduleAppointment(int userId, int storeId, int serviceId, decimal price, DateTime startAt, DateTime endAt, string? notes = null)
    {
        if (HasConflict(startAt, endAt))
        {
            throw new DomainException("Professional already has an appointment at this time.");
        }

        var appointment = Appointment.Create(userId, ProfessionalId, serviceId, storeId, price, startAt, endAt, notes);
        _appointments.Add(appointment);

        return appointment;
    }

    public Appointment RescheduleAppointment(int appointmentId, DateTime newStartAt, DateTime newEndAt)
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

        appointment.Reschedule(newStartAt, newEndAt);
        return appointment;
    }

    public void CancelAppointment(int appointmentId, string? reason = null)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId && !a.IsDeleted);

        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        appointment.Cancel(reason);
    }

    public bool HasConflict(DateTime startAt, DateTime endAt)
    {
        return _appointments.Any(
            a => startAt < a.EndAt &&
            endAt > a.StartAt &&
            a.Status == AppointmentStatus.Confirmed);
    }

    public bool HasConflictExcludingAppointment(int appointmentId, DateTime startAt, DateTime endAt)
    {
        return _appointments.Any(a => a.Id != appointmentId && startAt < a.EndAt && endAt > a.StartAt);
    }
}