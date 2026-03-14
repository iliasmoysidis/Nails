using Application.DTO.Calendar;

namespace Application.DTO.StaffCalendar;

public sealed record StaffCalendarExceptionDTO(
    DateOnly Date,
    bool IsDayOff,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);