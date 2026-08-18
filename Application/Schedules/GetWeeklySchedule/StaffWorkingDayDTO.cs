using Application.Common.DTO;

namespace Application.Schedules.GetWeeklySchedule;

public sealed record StaffWorkingDayDTO(
    DayOfWeek Day,
    bool IsDayOff,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);