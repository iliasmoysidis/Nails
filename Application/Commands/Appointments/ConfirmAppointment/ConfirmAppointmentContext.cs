using Domain.Entities;

namespace Application.Commands.Appointments;

public sealed class ConfirmAppointmentContext
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}