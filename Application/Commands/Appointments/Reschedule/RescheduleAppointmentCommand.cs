namespace Application.Commands.Appointments;

public sealed record RescheduleAppointmentCommand(int AppointmentId, DateTime NewStartAt);