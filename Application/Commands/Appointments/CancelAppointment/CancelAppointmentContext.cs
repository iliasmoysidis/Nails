using Domain.Entities;

namespace Application.Commands.Appointments;

public sealed class CancelAppointmentContext
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}