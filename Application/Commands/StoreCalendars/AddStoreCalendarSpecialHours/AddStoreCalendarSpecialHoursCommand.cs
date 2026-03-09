using Application.DTO.Calendar;

namespace Application.Commands.StoreCalendars;

public sealed record AddStoreCalendarSpecialHoursCommand(
    int StoreId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);