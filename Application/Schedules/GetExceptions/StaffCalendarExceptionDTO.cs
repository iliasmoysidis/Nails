using Application.Common.DTO;

namespace Application.Schedules.GetExceptions;

public sealed record StaffCalendarExceptionDTO(
    DateOnly Date,
    bool IsDayOff,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);