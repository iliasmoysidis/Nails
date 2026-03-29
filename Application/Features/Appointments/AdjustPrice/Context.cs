using Domain.Entities;

namespace Application.Features.Appointments.AdjustPrice;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}