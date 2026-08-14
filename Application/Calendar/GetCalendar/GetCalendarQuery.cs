namespace Application.Calendar.GetCalendar;

public sealed record GetCalendarQuery(
    int StoreId,
    DateOnly From,
    DateOnly To
);