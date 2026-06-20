using Domain.Roster;
using Domain.Appointments;

namespace Application.Features.Appointments.Cancel;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}