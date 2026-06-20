using Domain.Appointments;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjects;

namespace Domain.UserSchedules;

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
        EnsureBelongsToUser(appointment);
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

    private void EnsureBelongsToUser(Appointment appointment)
    {
        if (appointment.UserId != UserId)
            throw new InvariantException("Appointment does not belong to this user.");
    }
}
