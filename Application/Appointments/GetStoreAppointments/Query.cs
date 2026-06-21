namespace Application.Appointments.GetStoreAppointments;

public sealed record Query(
    int StoreId,
    DateOnly From,
    DateOnly To
);