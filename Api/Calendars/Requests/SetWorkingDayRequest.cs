using Application.Common.DTO;

namespace Api.Calendars.Requests;

public sealed record SetWorkingDayRequest(
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);
