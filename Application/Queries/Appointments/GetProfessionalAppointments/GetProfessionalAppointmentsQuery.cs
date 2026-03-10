namespace Application.Queries.Appointments;

public sealed record GetProfessionalAppointmentsQuery(
    int ProfessionalId,
    DateOnly From,
    DateOnly To
);