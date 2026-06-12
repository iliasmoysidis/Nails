using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Services;

public sealed class UserDeletion
{
    private readonly User _user;
    private readonly IReadOnlyCollection<Appointment> _appointments;

    public UserDeletion(User user, IReadOnlyCollection<Appointment> appointments)
    {
        ValidateComposition(user, appointments);

        _user = user;
        _appointments = appointments;
    }

    public void Delete(IClock clock)
    {
        foreach (var appointment in _appointments)
        {
            if (appointment.IsTerminal)
                continue;

            appointment.Cancel(clock, "User account deleted.");
        }
    }

    private static void ValidateComposition(User user, IReadOnlyCollection<Appointment> appointments)
    {
        foreach (var appointment in appointments)
        {
            if (appointment.UserId != user.Id)
                throw new InvariantException("Appointment does not belong to the user.");
        }
    }
}
