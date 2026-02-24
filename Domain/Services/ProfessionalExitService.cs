using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Services;

public sealed class ProfessionalExitService : IProfessionalExitService
{
    public void LeaveStore(
        Staff staff,
        ProfessionalOfferings? assignments,
        IReadOnlyCollection<Appointment> upcomingAppointments,
        int professionalId,
        IClock clock)
    {
        if (upcomingAppointments.Any())
            throw new InvariantException("Cannot leave store with upcoming appointments.");

        assignments?.UnassignAllForProfessional(professionalId);

        staff.RemoveFromStaff(professionalId, clock);
    }
}