namespace Application.Commands.Appointments;

public sealed record CancelCommand(int AppointmentId, string? Reason);