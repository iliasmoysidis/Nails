using Domain.Roster;
using Domain.Appointments;

namespace Application.Appointments.AdjustPrice;

public sealed class AdjustAppointmentPriceContext
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}