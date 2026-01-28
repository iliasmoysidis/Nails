namespace Application.Commands.Appointments;

public sealed record CancelAppointmentCommand(int AppointmentId, string? Reason);