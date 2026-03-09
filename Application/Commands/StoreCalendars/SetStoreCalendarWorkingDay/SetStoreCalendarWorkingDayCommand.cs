using Application.DTO.Calendar;

namespace Application.Commands.StoreCalendars;

public sealed record SetStoreCalendarWorkingDayCommand(
    int StoreId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);