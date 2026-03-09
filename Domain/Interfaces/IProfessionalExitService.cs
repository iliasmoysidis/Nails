using Domain.Entities;

namespace Domain.Interfaces;

public interface IProfessionalExitService
{
    void LeaveStore(
        Staff staff,
        Assignments? assignments,
        IReadOnlyCollection<Appointment> upcomingAppointments,
        int professionalId,
        IClock clock
    );
}