namespace Application.Features.Professionals.GetProfessionalAppointments;

public sealed record Query(
    int ProfessionalId,
    DateOnly From,
    DateOnly To
);