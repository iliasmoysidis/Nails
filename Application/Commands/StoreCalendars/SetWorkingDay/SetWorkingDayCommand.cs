using Application.DTO.Calendar;

namespace Application.Commands.StoreCalendars;

public sealed record SetWorkingDayCommand(
    int StoreId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);