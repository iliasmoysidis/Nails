namespace Application.Queries.Appointments;

public sealed record GetUserAppointmentsQuery(
    int UserId,
    DateOnly From,
    DateOnly To
);