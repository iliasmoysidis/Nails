using Application.Common.DTO;

namespace Application.Calendar.Common.DTO;

public sealed record CalendarDayDTO(
    DateOnly Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);