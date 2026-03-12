namespace Application.DTO.Calendar;

public sealed record CalendarDayDTO(
    DateOnly Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);