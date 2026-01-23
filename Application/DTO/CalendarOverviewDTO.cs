using Domain.ValueObjects.Calendar;

namespace Application.DTO;

public sealed record CalendarOverviewDTO(
    bool IsWorkingDay,
    IReadOnlyCollection<TimeRange> TimeRanges
);