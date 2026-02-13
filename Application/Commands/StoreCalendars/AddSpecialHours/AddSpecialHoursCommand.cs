using Application.DTO.Calendar;

namespace Application.Commands.StoreCalendars;

public sealed record AddSpecialHoursCommand(
    int StoreId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);