namespace Api.Appointments.Requests;

public sealed record GetProfessionalAppointmentsRequest(
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
);
