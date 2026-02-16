namespace Application.Commands.Appointments;

public sealed record UpdateAppointmentNotesCommand(int AppointmentId, string? Notes);