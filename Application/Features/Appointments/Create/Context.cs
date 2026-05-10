using Domain.Entities;

namespace Application.Features.Appointments.Create;

public sealed class Context
{
    public BookingSchedule BookingSchedule { get; set; } = default!;
    public UserSchedule UserSchedule { get; set; } = default!;
}
