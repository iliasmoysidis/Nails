namespace Application.Calendar.GetCalendar;

public sealed record Query(
    int StoreId,
    DateOnly From,
    DateOnly To
);