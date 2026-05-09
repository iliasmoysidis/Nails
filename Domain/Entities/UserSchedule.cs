using Domain.Exceptions;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class UserSchedule
{
    public int UserId { get; }
    private readonly List<Appointment> _appointments = [];

    public UserSchedule(int userId, IReadOnlyCollection<Appointment> appointments)
    {
        UserId = userId;

        foreach (var appointment in appointments)
        {
            if (appointment.UserId != userId)
                throw new InvariantException("Appointment does not belong to this user.");

            _appointments.Add(appointment);
        }
    }

    public void Add(Appointment appointment)
    {
        if (appointment.UserId != UserId)
            throw new InvariantException("Appointment does not belong to this user.");

        EnsureAvailable(appointment.StartAt, appointment.EndAt);

        _appointments.Add(appointment);
    }

    public void EnsureAvailable(UtcDateTime startAt, UtcDateTime endAt)
    {
        foreach (var appointment in _appointments)
        {
            if (appointment.ConflictsWith(startAt, endAt))
                throw new InvariantException("User already has an appointment during this time.");
        }
    }
}
