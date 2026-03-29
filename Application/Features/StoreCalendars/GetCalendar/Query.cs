namespace Application.Features.StoreCalendars.GetCalendar;

public sealed record Query(
    int StoreId,
    DateOnly From,
    DateOnly To
);