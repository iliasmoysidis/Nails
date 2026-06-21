namespace Application.Appointments.GetProfessionalAppointments;

public sealed record Query(
    int ProfessionalId,
    DateOnly From,
    DateOnly To
);