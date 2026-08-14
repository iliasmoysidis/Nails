namespace Application.Appointments.GetProfessionalAppointments;

public sealed record GetProfessionalAppointmentsQuery(
    int ProfessionalId,
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
);
