namespace Application.Queries.StoreCalendars;

public sealed record GetStoreProfessionalCalendarQuery(
    int StoreId,
    int ProfessionalId,
    DateOnly From,
    DateOnly To
);