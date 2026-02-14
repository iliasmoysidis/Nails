namespace Application.Commands.Appointments;

public sealed record ReassignAppointmentCommand(
    int AppointmentId,
    int ProfessionalId
);