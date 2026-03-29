namespace Application.Features.Appointments.GetUserAppointments;

public sealed record Query(
    int UserId,
    DateOnly From,
    DateOnly To
);