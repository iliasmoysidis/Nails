using Domain.Entities;

namespace Application.Commands.Professionals;

public sealed class LeaveProfessionalStoreContext
{
    public Staff Staff { get; set; } = default!;
    public Domain.Entities.Assignments? Assignments { get; set; }
    public IReadOnlyCollection<Appointment> UpcomingAppointments { get; set; } = [];
}