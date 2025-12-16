using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class ProfessionalAgenda
{
    public int ProfessionalId { get; }

    private readonly List<Appointment> _appointments = new();
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    private ProfessionalAgenda(int professionalId)
    {
        ProfessionalId = professionalId;
    }

    public static ProfessionalAgenda Create(int professionalId)
        => new(professionalId);

    public Appointment Schedule(int userId, int storeId, int offeringId, decimal price, UtcDateTime startAt, UtcDateTime endAt, IClock clock, string? notes = null)
    {
        EnsureNoConflict(startAt, endAt);

        var appointment = Appointment.Create(userId, ProfessionalId, offeringId, storeId, price, startAt, endAt, clock, notes);

        _appointments.Add(appointment);

        return appointment;
    }

    public void Reschedule(int appointmentId, UtcDateTime newStart, UtcDateTime newEnd, IClock clock)
    {
        var appointment = GetActive(appointmentId);

        EnsureNoConflictExcluding(appointment, newStart, newEnd);

        appointment.Reschedule(clock, newStart, newEnd);
    }

    public void Cancel(int appointmentId, IClock clock, string? reason = null)
    {
        var appointment = GetActive(appointmentId);

        appointment.Cancel(clock, reason);
    }

    public void Confirm(int appointmentId, IClock clock)
    {
        var appointment = GetActive(appointmentId);
        appointment.Confirm(clock);
    }

    public void Complete(int appointmentId, IClock clock)
    {
        var appointment = GetActive(appointmentId);
        appointment.Complete(clock);
    }

    public void MarkAsNoShow(int appointmentId, IClock clock)
    {
        var appointment = GetActive(appointmentId);
        appointment.MarkAsNoShow(clock);
    }

    public Appointment GetActive(int appointmentId)
        => _appointments.FirstOrDefault(a => a.Id == appointmentId && !a.IsDeleted)
            ?? throw new DomainException("Appointment not found.");

    private void EnsureNoConflict(UtcDateTime start, UtcDateTime end)
    {
        if (_appointments.Any(a => a.ConflictsWith(start, end)))
            throw new DomainException("Professional already has an appointment at this time.");
    }

    private void EnsureNoConflictExcluding(Appointment appointment, UtcDateTime start, UtcDateTime end)
    {
        if (_appointments.Any(a => a != appointment && a.ConflictsWith(start, end)))
            throw new DomainException("Reschedule conflict detected.");
    }
}