using Domain.Roster;
using Domain.Appointments;

namespace Application.Appointments.MarkNoShow;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}