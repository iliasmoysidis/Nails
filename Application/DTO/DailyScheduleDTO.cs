using Domain.ValueObjects.Calendar;

namespace Application.DTO;

public sealed record DailyScheduleDTO(
    DayOfWeek Day,
    IReadOnlyCollection<TimeRange> TimeRanges
);