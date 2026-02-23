namespace Application.Commands.Appointments;

public sealed record RescheduleCommand(int AppointmentId, DateTime NewStartAt);