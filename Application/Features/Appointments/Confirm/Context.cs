using Domain.Entities;

namespace Application.Features.Appointments.Confirm;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}
