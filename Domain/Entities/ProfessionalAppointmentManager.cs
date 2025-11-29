using Domain.Exceptions;

namespace Domain.Entities;

public class ProfessionalAppointmentManager
{
    public int ProfessionalId { get; private set; }

    private readonly List<Appointment> _appointments = new();
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    private ProfessionalAppointmentManager() { }

    public static ProfessionalAppointmentManager Create(int professionalId)
    {
        return new ProfessionalAppointmentManager
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

    public void CancelAppointment(int appointmentId)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);

        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        appointment.SoftDelete();
        appointment.MarkAsUpdated();
    }

    public bool HasConflict(DateTime startAt, DateTime endAt)
    {
        return _appointments.Any(a => startAt < a.EndAt && endAt > a.StartAt);
    }
}