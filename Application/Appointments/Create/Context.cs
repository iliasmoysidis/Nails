using Domain.UserSchedules;
using Domain.Appointments.Services;
using Domain.Users;

namespace Application.Appointments.Create;

public sealed class Context
{
    public AppointmentBooking AppointmentBooking { get; set; } = default!;
    public UserSchedule UserSchedule { get; set; } = default!;
    public User User { get; set; } = null!;
}
