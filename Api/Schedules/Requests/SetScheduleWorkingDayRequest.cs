using Application.Common.DTO;

namespace Api.Schedules.Requests;

public sealed record SetScheduleWorkingDayRequest(
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);
