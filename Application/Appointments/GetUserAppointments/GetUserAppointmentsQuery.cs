namespace Application.Appointments.GetUserAppointments;

public sealed record GetUserAppointmentsQuery(
    int UserId,
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
);
