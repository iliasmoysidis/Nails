using Application.DTO.Calendar;

namespace Application.DTO.StaffCalendar;

public sealed record StaffWorkingDayDTO(
    DayOfWeek Day,
    bool IsDayOff,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);