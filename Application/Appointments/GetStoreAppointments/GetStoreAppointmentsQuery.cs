namespace Application.Appointments.GetStoreAppointments;

public sealed record GetStoreAppointmentsQuery(
    int StoreId,
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
);
