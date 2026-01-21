using Application.Abstractions;

namespace Application.Booking.CancelAppointment;

public sealed record CancelAppointmentCommand(
    int AppointmentId,
    string? Reason = null
) : ICommand;