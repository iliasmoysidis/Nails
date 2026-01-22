using Application.Abstractions;

namespace Application.UseCases.Commands.Booking.CancelAppointment;

public sealed record CancelAppointmentCommand(
    int AppointmentId,
    string? Reason = null
) : ICommand;