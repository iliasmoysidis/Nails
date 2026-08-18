namespace Api.Appointments.Requests;

public sealed record GetUserAppointmentsRequest(
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
);
