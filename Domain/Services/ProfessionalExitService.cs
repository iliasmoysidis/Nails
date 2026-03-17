using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public sealed class ProfessionalExitService : IProfessionalExitService
{
    public void LeaveStore(
        Staff staff,
        Assignments? assignments,
        IReadOnlyCollection<Appointment> upcomingAppointments,
        int professionalId,
        IClock clock)
    {
        foreach (var appointment in upcomingAppointments)
        {
            if (appointment.IsTerminal) continue;

            appointment.Cancel(clock, "Professional left the store.");
        }

        assignments?.RemoveByProfessional(professionalId);

        staff.RemoveFromStaff(professionalId);
    }
}