using Domain.Rosters;
using Domain.Appointments;

namespace Application.Appointments.Complete;

public sealed class CompleteAppointmentContext
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}