using Application.Abstractions;

namespace Application.UseCases.Booking.CancelAppointment;

public sealed record CancelAppointmentCommand(
    int AppointmentId,
    string? Reason = null
) : ICommand;