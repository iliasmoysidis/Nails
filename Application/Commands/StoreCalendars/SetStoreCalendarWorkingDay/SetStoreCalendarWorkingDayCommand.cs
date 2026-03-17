using Application.DTO.Calendar;
using MediatR;

namespace Application.Commands.StoreCalendars;

public sealed record SetStoreCalendarWorkingDayCommand(
    int StoreId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;