using Application.Common.DTO;

namespace Application.Schedule.GetExceptions;

public sealed record StaffCalendarExceptionDTO(
    DateOnly Date,
    bool IsDayOff,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);