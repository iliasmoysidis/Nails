namespace Application.Queries.StoreCalendars;

public sealed record GetStoreCalendarQuery(
    int StoreId,
    DateOnly From,
    DateOnly To
);