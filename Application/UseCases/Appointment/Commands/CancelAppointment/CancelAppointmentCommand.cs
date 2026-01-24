using Application.Abstractions;

namespace Application.UseCases.Appointment.Commands.CancelAppointment;

public sealed record CancelAppointmentCommand(
    int AppointmentId,
    string? Reason = null
) : ICommand;