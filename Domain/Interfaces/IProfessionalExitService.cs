using Domain.Entities;

namespace Domain.Interfaces;

public interface IProfessionalExitService
{
    void LeaveStore(
        Staff staff,
        ProfessionalOfferings? assignments,
        IReadOnlyCollection<Appointment> upcomingAppointments,
        int professionalId,
        IClock clock
    );
}