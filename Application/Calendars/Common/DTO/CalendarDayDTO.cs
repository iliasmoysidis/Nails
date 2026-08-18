using Application.Common.DTO;

namespace Application.Calendars.Common.DTO;

public sealed record CalendarDayDTO(
    DateOnly Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);