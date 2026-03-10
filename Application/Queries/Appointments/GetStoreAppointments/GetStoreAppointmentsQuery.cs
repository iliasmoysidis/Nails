namespace Application.Queries.Appointments;

public sealed record GetStoreAppointmentsQuery(
    int StoreId,
    DateOnly From,
    DateOnly To
);