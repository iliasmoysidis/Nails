namespace Application.Commands.Appointments;

public sealed record ReassignCommand(
    int AppointmentId,
    int ProfessionalId
);