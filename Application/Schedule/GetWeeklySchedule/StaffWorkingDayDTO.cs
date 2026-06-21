using Application.Common.DTO;

namespace Application.Schedule.GetWeeklySchedule;

public sealed record StaffWorkingDayDTO(
    DayOfWeek Day,
    bool IsDayOff,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);