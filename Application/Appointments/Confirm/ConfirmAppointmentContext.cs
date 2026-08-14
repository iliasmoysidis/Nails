using Domain.Roster;
using Domain.Appointments;

namespace Application.Appointments.Confirm;

public sealed class ConfirmAppointmentContext
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}
