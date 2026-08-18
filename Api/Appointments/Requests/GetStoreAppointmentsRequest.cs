namespace Api.Appointments.Requests;

public sealed record GetStoreAppointmentsRequest(
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
);
