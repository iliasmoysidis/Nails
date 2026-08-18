using Application.Common.DTO;
using MediatR;

namespace Application.Calendars.SetWorkingDay;

public sealed record SetCalendarWorkingDayCommand(
    int StoreId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;