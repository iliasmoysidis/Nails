using Domain.Roster;
using Domain.Appointments;

namespace Application.Appointments.Cancel;

public sealed class CancelAppointmentContext
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}